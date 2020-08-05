using Engine.Core;
using Engine.GLApi;
using MathNet.Numerics;
using MathNet.Numerics.Integration;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Statistics;
using OpenTK;
using OpenTK.Graphics.ES11;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;


/// <summary>
/// Class for URoom, ICP implementation
/// </summary>

namespace Engine.Processing
{


    public class ICP
    {
        private const float ErrorThreshold = 0.0001f;
        private const int RandomSampleCount = 100;

        private List<Vector<float>> _target;
        private List<Vector<float>> _source;
        private Stopwatch _watch = new Stopwatch();


        public Matrix4 Transformation { get; private set; }
        public List<Vector3> Result
        {
            get
            {
                List<Vector3> r = new List<Vector3>();
                for (int i = 0; i < _source.Count; i++)
                {
                    r.Add(new Vector3(_source[i][0], _source[i][1], _source[i][2]));
                }
                return r;
            }
        }

        public ICP(List<Vector3> source, List<Vector3> target)
        {
            _target = new List<Vector<float>>();
            _source = new List<Vector<float>>();

            for (int i = 0; i < target.Count; i++)
            {
                _target.Add(Vector<float>.Build.Dense(new float[] { target[i].X, target[i].Y, target[i].Z }));
            }

            for (int i = 0; i < source.Count; i++)
            {
                _source.Add(Vector<float>.Build.Dense(new float[] { source[i].X, source[i].Y, source[i].Z }));
            }

            Transformation = Matrix4.Identity;
        }

        private Vector<float> GetMean(List<Vector<float>> vector)
        {
            Vector<float> sum = Vector<float>.Build.Dense(3, 0);
            foreach (var item in vector)
            {
                sum += item;
            }
            return sum / vector.Count;
        }

        public int Align(int maxIter)
        {
            KdTree.KdTree<float, float> targetTree = new KdTree.KdTree<float, float>(3, new KdTree.Math.FloatMath());
            Matrix<float> rotation = Matrix<float>.Build.Dense(3, 3, 0);
            Vector<float> translation = Vector<float>.Build.Dense(3, 0);
            
            var targetCenter = GetMean(_target);
            var rng = new Random();

            for (int i = 0; i < _target.Count; i++)
            {
                targetTree.Add(_target[i].ToArray(), 100);
            }

            int it = 0;
            float error = float.PositiveInfinity;
            while (it++ < maxIter && error > ErrorThreshold)
            {

                Matrix<float> W = Matrix<float>.Build.Dense(3, 3, 0);
                
                var sourceCenter = GetMean(_source);

                error = 0;
                for (int i = 0; i < RandomSampleCount; i++)
                {
                    var rn = rng.Next(Math.Min(_source.Count, _target.Count));

                    var n = targetTree.GetNearestNeighbours(_source[rn].AsArray(), 1);

                    if (n.Length > 0)
                    {
                        var p = Vector<float>.Build.Dense(n[0].Point);
                        var qs = p - targetCenter;
                        var qd = _source[rn] - sourceCenter;

                        W += qs.OuterProduct(qd);

                        var v = _target[rn] - rotation * _source[rn] - translation;
                        error += v.DotProduct(v);
                    }
                }
                error = (float)Math.Sqrt(error);
                error /= RandomSampleCount;

                var svd = W.Svd(true);
                rotation = svd.U * svd.VT;

                if (rotation.Determinant() < 0)
                {
                    Logger.Log("reflection needed");
                    var v = svd.VT.Transpose();
                    var col = v.Column(2).AsArray();
                    col[0] *= -1;
                    col[1] *= -1;
                    col[2] *= -1;
                    v.SetColumn(2, col);

                    rotation = v * svd.U.Transpose();
                }

                translation = targetCenter - rotation * sourceCenter;

                for (int i = 0; i < _source.Count; i++)
                {
                    _source[i] = rotation * _source[i] + translation;
                }

            }

            Logger.Log($"sample count {RandomSampleCount}");
            Logger.Log($"iteration count {it}");
            Logger.Log($"error: {error}");
            return 0;
        }

    }

    public struct SicpParameters
    {
        public bool UsePenalty;
        public double P;
        public double Mu;
        public double Alpha;
        public double MaxMu;
        public int MaxIcp;
        public int MaxInner;
        public int MaxOuter;
        public double Stop;

        public SicpParameters(bool usePenalty = false, double p = 1.0, int maxIcp = 100, double mu = 10.0, double alpha = 1.2, double maxMu = 1e5, int maxInner = 1, int maxOuter = 100, double stop = 1e-5)
        {
            UsePenalty = usePenalty;
            P = p;
            Mu = mu;
            Alpha = alpha;
            MaxMu = maxMu;
            MaxIcp = maxIcp;
            MaxInner = maxInner;
            MaxOuter = maxOuter;
            Stop = stop;
        }
    }

    /// <summary>
    /// Based on ADMM over point-to-point from paper "Sparse Iterative Closest Point,2013"
    /// </summary>
    public class SICP
    {
        private Matrix<double> _target;
        private Matrix<double> _source;

        public List<Vector3> Result
        {
            get
            {
                List<Vector3> r = new List<Vector3>();
                for (int i = 0; i < _source.ColumnCount; i++)
                {
                    var row = _source.Column(i);
                    r.Add(new Vector3((float)row[0], (float)row[1], (float)row[2]));
                }
                return r;
            }
        }

        public SICP(List<Vector3> source, List<Vector3> target)
        {
            Logger.Log($"Can use blas { MathNet.Numerics.Control.TryUseNativeOpenBLAS()}");

            _target = Matrix<double>.Build.Dense(3, target.Count, 0);
            _source = Matrix<double>.Build.Dense(3, source.Count, 0);

            
            for (int i = 0; i < target.Count; i++)
            {
                _target.SetColumn(i, new double[] { target[i].X, target[i].Y, target[i].Z });
            }

            for (int i = 0; i < source.Count; i++)
            {
                _source.SetColumn(i, new double[] { source[i].X, source[i].Y, source[i].Z });
            }

        }

        private Matrix<double> Shrink(int k, Matrix<double> Q, double mu, double p)
        {
            double ba = Math.Pow((2.0 / mu) * (1.0 - p), 1.0 / (2.0 - p));
            double ha = ba + (p / mu) * Math.Pow(ba, p - 1.0);

            Parallel.For(0, Q.ColumnCount, (i) => 
            {
                double n = Q.Column(i).Norm(2.0);
                double w = 0.0;
                if (n > ha)
                {
                    w = Shrinkage(k, mu, n, p, (ba / n + 1.0) / 2.0);
                }
                Q.SetColumn(i, Q.Column(i) * w);
            });
            return Q;
        }

        private double Shrinkage(int i, double mu, double n, double p, double s)
        {
            if (i > 0)
            {
                return Shrinkage(i - 1, mu, n, p, 1.0 - (p / mu) * Math.Pow(n, p - 2.0) * Math.Pow(s, p - 1.0));
            }
            return s;
        }

        

        private void Transform(ref Matrix<double> X, ref Matrix<double> Y, Vector<double> w)
        {
        
            var xMean = X.RowSums() / X.ColumnCount;
            var yMean = Y.RowSums() / Y.ColumnCount;

            Func<Vector<double>, Vector<double>> addX = x => x + xMean;
            Func<Vector<double>, Vector<double>> addY = x => x + yMean;
            Func<Vector<double>, Vector<double>> subX = x => x - xMean;
            Func<Vector<double>, Vector<double>> subY = x => x - yMean;


            Vector<double> r = Vector<double>.Build.Dense(3, 0);
            for (int i = 0; i < X.ColumnCount; i++)
            {
                X.Column(i, r);
                var outV = subX(r); 
                X.SetColumn(i, outV);
            }

            for (int i = 0; i < Y.ColumnCount; i++)
            {
                Y.Column(i, r);
                var outV = subY(r);
                Y.SetColumn(i, outV);
            }

            Matrix<double> rotation;
            Matrix<double> sigma = (X * w[0] / w.Sum())  * Y.Transpose();

            var svd = sigma.Svd(true);

            var V = svd.VT.Transpose();
            rotation = V * svd.U.Transpose();

            //reflectivity case
            if (rotation.Determinant() < 0.0)
            {
                var newV = svd.VT.Transpose();
                newV.SetColumn(2, newV.Column(2) * -1);

                rotation = newV * svd.U.Transpose();
            }
            
            var translation = yMean - rotation * xMean;

            for (int i = 0; i < X.ColumnCount; i++)
            {
                X.SetColumn(i, rotation * X.Column(i) + translation + xMean);
            }

            for (int i = 0; i < Y.ColumnCount; i++)
            {
                Y.Column(i, r);
                var outV = addY(r);
                Y.SetColumn(i, outV);
            }

        }

        public void Align(SicpParameters par)
        {

            // Build the tree

            //alglib.kdtreebuild(_target.ToArray(), 3, 0, 2, out alglib.kdtree kdtree);
            //alglib.kdtreeserialize(kdtree, out string asd);

            KdTree.KdTree<double, double> tree = new KdTree.KdTree<double, double>(3, new KdTree.Math.DoubleMath());
            for (int i = 0; i < _target.ColumnCount; i++)
            {
                tree.Add(_target.Column(i).AsArray(), 100);
            }

            Matrix<double> Q = Matrix<double>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<double> Z = Matrix<double>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<double> C = Matrix<double>.Build.Dense(3, _source.ColumnCount, 0);

            Matrix<double> Xo1 = Matrix<double>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<double> Xo2 = Matrix<double>.Build.Dense(3, _source.ColumnCount, 0);
            _source.CopyTo(Xo1);
            _source.CopyTo(Xo2);

            int icp = 0;
            for (; icp < par.MaxIcp; icp++)
            {
                //for (int i = 0; i < _source.ColumnCount; i++)
                //{
                //    double[,] nn = new double[1, 3];
                //    alglib.kdtreequeryknn(kdtree, _source.Column(i).AsArray(), 1);
                //    alglib.kdtreequeryresultsx(kdtree, ref nn);
                //    Q.SetColumn(i, new double[] { nn[0, 0], nn[0, 1], nn[0, 2] });
                //}

                Parallel.For(0, _source.ColumnCount, (i) =>
                 {
                     var n = tree.GetNearestNeighbours(_source.Column(i).AsArray(), 1);
                     if (n.Length > 0)
                     {
                         Q.SetColumn(i, n[0].Point);
                     }
                 });

                double mu = par.Mu;

                for (int outer = 0; outer < par.MaxOuter; outer++)
                {
                    double dual = 0.0;
                    for (int inner = 0; inner < par.MaxInner; inner++)
                    {
                        Z = _source - Q + C / mu;

                        Z = Shrink(3, Z, mu, par.P);

                        Matrix<double> U = Q + Z - C / mu;
                        Transform(ref _source, ref U, Vector<double>.Build.Dense(_source.ColumnCount,1.0));

                        dual = (_source - Xo1).NormalizeColumns(2.0).Enumerate().Max();
                        _source.CopyTo(Xo1);

                        if (dual < par.Stop)
                        {
                            break;
                        }
                    }

                    Matrix<double> P = _source - Q - Z;
                    if (!par.UsePenalty)
                    {
                        C += (mu * P);
                    }

                    if (mu < par.MaxMu)
                    {
                        mu *= par.Alpha;
                    }

                    double primal = P.NormalizeColumns(2.0).Enumerate().Max();
                    if (primal < par.Stop && dual < par.Stop)
                    {
                        break;
                    }
                }

                double stop = (_source - Xo2).NormalizeColumns(2.0).Enumerate().Max();
                _source.CopyTo(Xo2);
                
                if (stop < par.Stop)
                {
                    break;
                }

            }
            Logger.Log($"iteration count {icp}");

        }

    }

}

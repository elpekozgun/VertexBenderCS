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
        public float P;
        public float Mu;
        public float Alpha;
        public float MaxMu;
        public int MaxIcp;
        public int MaxInner;
        public int MaxOuter;
        public float Stop;

        public SicpParameters(bool usePenalty = false, float p = 2.0f, int maxIcp = 100, float mu = 10.0f, float alpha = 1.2f, float maxMu = 1e5f, int maxInner = 1, int maxOuter = 100, float stop = 1e-4f)
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
        private Matrix<float> _target;
        private Matrix<float> _source;

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

            _target = Matrix<float>.Build.Dense(3, target.Count, 0);
            _source = Matrix<float>.Build.Dense(3, source.Count, 0);

            
            for (int i = 0; i < target.Count; i++)
            {
                _target.SetColumn(i, new float[] { target[i].X, target[i].Y, target[i].Z });
            }

            for (int i = 0; i < source.Count; i++)
            {
                _source.SetColumn(i, new float[] { source[i].X, source[i].Y, source[i].Z });
            }

        }

        private Matrix<float> Shrink(int k, Matrix<float> Q, float mu, float p)
        {
            float ba = (float)Math.Pow((2.0 / mu) * (1.0 - p), 1.0 / (2.0 - p));
            float ha = ba + (p / mu) * (float)Math.Pow(ba, p - 1.0);

            Parallel.For(0, Q.ColumnCount, (i) => 
            {
                float n = (float)Q.Column(i).Norm(2.0);
                float w = 0.0f;
                if (n > ha)
                {
                    w = Shrinkage(k, mu, n, p, (ba / n + 1.0f) / 2.0f);
                }
                Q.SetColumn(i, Q.Column(i) * w);
            });
            return Q;
        }

        private float Shrinkage(int i, float mu, float n, float p, float s)
        {
            if (i > 0)
            {
                return Shrinkage(i - 1, mu, n, p, (float)(1.0- (p / mu) * Math.Pow(n, p - 2.0) * Math.Pow(s, p - 1.0)));
            }
            return s;
        }

        

        private void Transform(ref Matrix<float> X, ref Matrix<float> Y, Vector<float> w)
        {
        
            var xMean = X.RowSums() / X.ColumnCount;
            var yMean = Y.RowSums() / Y.ColumnCount;

            Func<Vector<float>, Vector<float>> addX = x => x + xMean;
            Func<Vector<float>, Vector<float>> addY = x => x + yMean;
            Func<Vector<float>, Vector<float>> subX = x => x - xMean;
            Func<Vector<float>, Vector<float>> subY = x => x - yMean;


            Vector<float> r = Vector<float>.Build.Dense(3, 0);
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

            Matrix<float> rotation;
            Matrix<float> sigma = (X * w[0] / w.Sum())  * Y.Transpose();

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

        public int Align(SicpParameters par)
        {

            // Build the tree

            //alglib.kdtreebuild(_target.ToArray(), 3, 0, 2, out alglib.kdtree kdtree);
            //alglib.kdtreeserialize(kdtree, out string asd);

            //KdTree.KdTree<float, float> tree = new KdTree.KdTree<float, float>(3, new KdTree.Math.floatMath());
            //for (int i = 0; i < _target.ColumnCount; i++)
            //{
            //    tree.Add(_target.Column(i).AsArray(), 100);
            //}

            KdTree<int> tree = new SqrEuclid<int>(3, _target.ColumnCount);
            for (int i = 0; i < _target.ColumnCount; i++)
            {
                tree.AddPoint(_target.Column(i).AsArray(), i);
            }

            Matrix<float> Q = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<float> Z = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<float> C = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);

            Matrix<float> Xo1 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<float> Xo2 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            _source.CopyTo(Xo1);
            _source.CopyTo(Xo2);

            int icp = 0;
            for (; icp < par.MaxIcp; icp++)
            {
                //for (int i = 0; i < _source.ColumnCount; i++)
                //{
                //    float[,] nn = new float[1, 3];
                //    alglib.kdtreequeryknn(kdtree, _source.Column(i).AsArray(), 1);
                //    alglib.kdtreequeryresultsx(kdtree, ref nn);
                //    Q.SetColumn(i, new float[] { nn[0, 0], nn[0, 1], nn[0, 2] });
                //}

                var src = _source.AsColumnMajorArray();

                //Parallel.For(0, _source.ColumnCount, (i) =>
                for (int i = 0; i < _source.ColumnCount; i++)
                {
                     //var n = tree.GetNearestNeighbours(_source.Column(i).AsArray(), 1);
                     //if (n.Length > 0)
                     //{
                     //    Q.SetColumn(i, n[0].Point);
                     //}
                    // var n = tree.NearestNeighbor(_source.Column(i).AsArray(), 0, 1, true);
                    var n = tree.NearestNeighbor(src, i * 3, 1, false);
                     if (n.Count > 0)
                     {
                         Q.SetColumn(i, _target.Column(n[0].value));
                     }

                 }

                float mu = par.Mu;

                for (int outer = 0; outer < par.MaxOuter; outer++)
                {
                    float dual = 0.0f;
                    for (int inner = 0; inner < par.MaxInner; inner++)
                    {
                        Z = _source - Q + C / mu;

                        Z = Shrink(3, Z, mu, par.P);

                        Matrix<float> U = Q + Z - C / mu;
                        Transform(ref _source, ref U, Vector<float>.Build.Dense(_source.ColumnCount,1.0f));

                        //dual = (_source - Xo1).NormalizeColumns(2.0).Enumerate().Max();
                        dual = (float)(_source - Xo1).EnumerateColumns().Select(x => x.Norm(2)).Max();
                        _source.CopyTo(Xo1);

                        if (dual < par.Stop)
                        {
                            break;
                        }
                    }

                    Matrix<float> P = _source - Q - Z;
                    if (!par.UsePenalty)
                    {
                        C += (mu * P);
                    }

                    if (mu < par.MaxMu)
                    {
                        mu *= par.Alpha;
                    }

                    //float primal = P.NormalizeColumns(2.0).Enumerate().Max();
                    float primal = (float)P.EnumerateColumns().Select(x => x.Norm(2)).Max();


                    if (primal < par.Stop && dual < par.Stop)
                    {
                        break;
                    }
                }

                //float stop = (_source - Xo2).NormalizeColumns(2.0).Enumerate().Max();
                float stop = (float)(_source - Xo2).EnumerateColumns().Select(x => x.Norm(2)).Max();
                _source.CopyTo(Xo2);
                
                if (stop < par.Stop)
                {
                    break;
                }

            }
            Logger.Log($"iteration count {icp}");
            return 0;
        }

        public int AlignSC(SicpParameters par)
        {

            float[][] points = new float[_target.ColumnCount][];
            int[] nodes = new int[_target.ColumnCount];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = _target.Column(i).AsArray();
                nodes[i] = i;
            }


            Supercluster.KDTree.KDTree<float, int> ktree = new Supercluster.KDTree.KDTree<float, int>(3, points, nodes, (a, b)=> { return a[0] * b[0] + a[1] * b[1] + a[2] * b[2]; });

            //KdTree<int> tree = new SqrEuclid<int>(3, _target.ColumnCount);
            //for (int i = 0; i < _target.ColumnCount; i++)
            //{
            //    tree.AddPoint(_target.Column(i).AsArray(), i);
            //}

            Matrix<float> Q = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<float> Z = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<float> C = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);

            Matrix<float> Xo1 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            Matrix<float> Xo2 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
            _source.CopyTo(Xo1);
            _source.CopyTo(Xo2);

            int icp = 0;
            for (; icp < par.MaxIcp; icp++)
            {
                //for (int i = 0; i < _source.ColumnCount; i++)
                //{
                //    float[,] nn = new float[1, 3];
                //    alglib.kdtreequeryknn(kdtree, _source.Column(i).AsArray(), 1);
                //    alglib.kdtreequeryresultsx(kdtree, ref nn);
                //    Q.SetColumn(i, new float[] { nn[0, 0], nn[0, 1], nn[0, 2] });
                //}

                var src = _source.AsColumnMajorArray();

                //Parallel.For(0, _source.ColumnCount, (i) =>
                for (int i = 0; i < _source.ColumnCount; i++)
                {
                    //var n = tree.NearestNeighbor(src, i * 3, 1, true);
                    //if (n.Count > 0)
                    //{
                    //    Q.SetColumn(i, _target.Column(n[0].value));
                    //}

                    var n = ktree.NearestNeighbors(_source.Column(i).AsArray(), 1);
                    if (n != null)
                    {
                        Q.SetColumn(i, _target.Column(n[0].Item2));
                    }

                }

                float mu = par.Mu;

                for (int outer = 0; outer < par.MaxOuter; outer++)
                {
                    float dual = 0.0f;
                    for (int inner = 0; inner < par.MaxInner; inner++)
                    {
                        Z = _source - Q + C / mu;

                        Z = Shrink(3, Z, mu, par.P);

                        Matrix<float> U = Q + Z - C / mu;
                        Transform(ref _source, ref U, Vector<float>.Build.Dense(_source.ColumnCount, 1.0f));

                        //dual = (_source - Xo1).NormalizeColumns(2.0).Enumerate().Max();
                        dual = (float)(_source - Xo1).EnumerateColumns().Select(x => x.Norm(2)).Max();
                        _source.CopyTo(Xo1);

                        if (dual < par.Stop)
                        {
                            break;
                        }
                    }

                    Matrix<float> P = _source - Q - Z;
                    if (!par.UsePenalty)
                    {
                        C += (mu * P);
                    }

                    if (mu < par.MaxMu)
                    {
                        mu *= par.Alpha;
                    }

                    //float primal = P.NormalizeColumns(2.0).Enumerate().Max();
                    float primal = (float)P.EnumerateColumns().Select(x => x.Norm(2)).Max();


                    if (primal < par.Stop && dual < par.Stop)
                    {
                        break;
                    }
                }

                //float stop = (_source - Xo2).NormalizeColumns(2.0).Enumerate().Max();
                float stop = (float)(_source - Xo2).EnumerateColumns().Select(x => x.Norm(2)).Max();
                _source.CopyTo(Xo2);

                if (stop < par.Stop)
                {
                    break;
                }

            }
            Logger.Log($"iteration count {icp}");
            return 0;
        }


    }

}

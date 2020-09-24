//using Engine.Core;
//using Engine.GLApi;
//using MathNet.Numerics;
//using MathNet.Numerics.Integration;
//using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.LinearAlgebra.Complex;
//using MathNet.Numerics.Optimization;
//using MathNet.Numerics.Statistics;
//using Microsoft.SqlServer.Server;
//using OpenTK;
//using OpenTK.Graphics.ES11;
//using OpenTK.Graphics.OpenGL;
//using OpenTK.Platform.Windows;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography;
//using System.Threading.Tasks;
//using System.Windows.Forms;

///// <summary>
///// Class for URoom, ICP implementation
///// </summary>

//namespace Engine.Processing
//{
  
//    /// <summary>
//    /// Based on ADMM over point-to-point from paper "Sparse Iterative Closest Point,2013"
//    /// </summary>
//    public class SICP2
//    {
//        private Matrix<float> _target;
//        private Matrix<float> _source;

//        public List<Vector3> Result
//        {
//            get
//            {
//                List<Vector3> r = new List<Vector3>();
//                for (int i = 0; i < _source.ColumnCount; i++)
//                {
//                    var row = _source.Column(i);
//                    r.Add(new Vector3((float)row[0], (float)row[1], (float)row[2]));
//                }
//                return r;
//            }
//        }

//        public SICP2(List<Vector3> source, List<Vector3> target)
//        {
//            Logger.Log($"Can use blas { MathNet.Numerics.Control.TryUseNativeOpenBLAS()}");

//            _target = Matrix<float>.Build.Dense(3, target.Count, 0);
//            _source = Matrix<float>.Build.Dense(3, source.Count, 0);


//            for (int i = 0; i < target.Count; i++)
//            {
//                _target.SetColumn(i, new float[] { target[i].X, target[i].Y, target[i].Z });
//            }

//            for (int i = 0; i < source.Count; i++)
//            {
//                _source.SetColumn(i, new float[] { source[i].X, source[i].Y, source[i].Z });
//            }

//        }

//        private Matrix<float> Shrink(int k, Matrix<float> Q, float mu, float p)
//        {
//            float ba = Math.Pow((2.0 / mu) * (1.0 - p), 1.0 / (2.0 - p));
//            float ha = ba + (p / mu) * Math.Pow(ba, p - 1.0);

//            Parallel.For(0, Q.ColumnCount, (i) =>
//            {
//                float n = Q.Column(i).Norm(2.0);
//                float w = 0.0;
//                if (n > ha)
//                {
//                    w = Shrinkage(k, mu, n, p, (ba / n + 1.0) / 2.0);
//                }
//                Q.SetColumn(i, Q.Column(i) * w);
//            });
//            return Q;
//        }

//        private float Shrinkage(int i, float mu, float n, float p, float s)
//        {
//            if (i > 0)
//            {
//                return Shrinkage(i - 1, mu, n, p, 1.0 - (p / mu) * Math.Pow(n, p - 2.0) * Math.Pow(s, p - 1.0));
//            }
//            return s;
//        }



//        private void Transform(ref Matrix<float> X, ref Matrix<float> Y, Vector<float> w)
//        {

//            var xMean = X.RowSums() / X.ColumnCount;
//            var yMean = Y.RowSums() / Y.ColumnCount;

//            Func<Vector<float>, Vector<float>> addX = x => x + xMean;
//            Func<Vector<float>, Vector<float>> addY = x => x + yMean;
//            Func<Vector<float>, Vector<float>> subX = x => x - xMean;
//            Func<Vector<float>, Vector<float>> subY = x => x - yMean;


//            Vector<float> r = Vector<float>.Build.Dense(3, 0);
//            for (int i = 0; i < X.ColumnCount; i++)
//            {
//                X.Column(i, r);
//                var outV = subX(r);
//                X.SetColumn(i, outV);
//            }

//            for (int i = 0; i < Y.ColumnCount; i++)
//            {
//                Y.Column(i, r);
//                var outV = subY(r);
//                Y.SetColumn(i, outV);
//            }

//            Matrix<float> rotation;
//            Matrix<float> sigma = (X * w[0] / w.Sum()) * Y.Transpose();

//            var svd = sigma.Svd(true);

//            var V = svd.VT.Transpose();
//            rotation = V * svd.U.Transpose();

//            //reflectivity case
//            if (rotation.Determinant() < 0.0)
//            {
//                var newV = svd.VT.Transpose();
//                newV.SetColumn(2, newV.Column(2) * -1);

//                rotation = newV * svd.U.Transpose();
//            }

//            var translation = yMean - rotation * xMean;

//            for (int i = 0; i < X.ColumnCount; i++)
//            {
//                X.SetColumn(i, rotation * X.Column(i) + translation + xMean);
//            }

//            for (int i = 0; i < Y.ColumnCount; i++)
//            {
//                Y.Column(i, r);
//                var outV = addY(r);
//                Y.SetColumn(i, outV);
//            }

//        }

//        public int AlignAbs(SicpParameters par)
//        {

//            // Build the tree

//            //alglib.kdtreebuild(_target.ToArray(), 3, 0, 2, out alglib.kdtree kdtree);
//            //alglib.kdtreeserialize(kdtree, out string asd);

//            KdTree<float[]> tree = new SqrEuclid<float[]>(3, _target.ColumnCount);
//            for (int i = 0; i < _target.ColumnCount; i++)
//            {
//                tree.AddPoint(_target.Column(i).AsArray(), _target.Column(i).AsArray());
//            }
//            //KdTree.KdTree<float, float> kdTree = new KdTree.KdTree<float, float>(3, new KdTree.Math.floatMath());
//            //for (int i = 0; i < _target.ColumnCount; i++)
//            //{
//            //    kdTree.Add(_target.Column(i).AsArray(), 100);
//            //}

//            Matrix<float> Q = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> Z = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> C = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);

//            Matrix<float> Xo1 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> Xo2 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            _source.CopyTo(Xo1);
//            _source.CopyTo(Xo2);

//            int icp = 0;
//            for (; icp < par.MaxIcp; icp++)
//            {
//                //for (int i = 0; i < _source.ColumnCount; i++)
//                //{
//                //    float[,] nn = new float[1, 3];
//                //    alglib.kdtreequeryknn(kdtree, _source.Column(i).AsArray(), 1);
//                //    alglib.kdtreequeryresultsx(kdtree, ref nn);
//                //    Q.SetColumn(i, new float[] { nn[0, 0], nn[0, 1], nn[0, 2] });
//                //}

//                //Parallel.For(0, _source.ColumnCount, (i) =>
//                for (int i = 0; i < _source.ColumnCount; i++)
//                {
//                    //var n = kdTree.GetNearestNeighbours(_source.Column(i).AsArray(), 1);
//                    //if (n.Length > 0)
//                    //{
//                    //    Q.SetColumn(i, n[0].Point);
//                    //}
//                    var n = tree.NearestNeighbor(_source.Column(i).AsArray(), 0, 1, true);
//                    if (n.Count > 0)
//                    {
//                        Q.SetColumn(i, n[0].value);
//                    }
//                }

//                float mu = par.Mu;

//                for (int outer = 0; outer < par.MaxOuter; outer++)
//                {
//                    float dual = 0.0;
//                    for (int inner = 0; inner < par.MaxInner; inner++)
//                    {
//                        Z = _source - Q + C / mu;

//                        Z = Shrink(3, Z, mu, par.P);

//                        Matrix<float> U = Q + Z - C / mu;
//                        Transform(ref _source, ref U, Vector<float>.Build.Dense(_source.ColumnCount, 1.0));

//                        dual = (_source - Xo1).Enumerate().Select(v=>Math.Abs(v)).Max();//.NormalizeColumns(2.0).Enumerate().Max();
//                        _source.CopyTo(Xo1);

//                        if (dual < par.Stop)
//                        {
//                            break;
//                        }
//                    }

//                    Matrix<float> P = _source - Q - Z;
//                    if (!par.UsePenalty)
//                    {
//                        C += (mu * P);
//                    }

//                    if (mu < par.MaxMu)
//                    {
//                        mu *= par.Alpha;
//                    }

//                    float primal = P.Enumerate().Select(v => Math.Abs(v)).Max();//.NormalizeColumns(2.0).Enumerate().Max();
//                    if (primal < par.Stop && dual < par.Stop)
//                    {
//                        break;
//                    }
//                }

//                float stop = (_source - Xo2).Enumerate().Select(v => Math.Abs(v)).Max();//.NormalizeColumns(2.0).Enumerate().Max();
//                _source.CopyTo(Xo2);

//                if (stop < par.Stop)
//                {
//                    break;
//                }

//            }

//            Logger.Log($"iteration count {icp}");
//            return 0;
//        }

//        public int AlignSum(SicpParameters par)
//        {

//            // Build the tree

//            //alglib.kdtreebuild(_target.ToArray(), 3, 0, 2, out alglib.kdtree kdtree);
//            //alglib.kdtreeserialize(kdtree, out string asd);

//            KdTree.KdTree<float, float> tree = new KdTree.KdTree<float, float>(3, new KdTree.Math.floatMath());
//            for (int i = 0; i < _target.ColumnCount; i++)
//            {
//                tree.Add(_target.Column(i).AsArray(), 100);
//            }

//            Matrix<float> Q = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> Z = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> C = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);

//            Matrix<float> Xo1 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> Xo2 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            _source.CopyTo(Xo1);
//            _source.CopyTo(Xo2);

//            int icp = 0;
//            for (; icp < par.MaxIcp; icp++)
//            {
//                //for (int i = 0; i < _source.ColumnCount; i++)
//                //{
//                //    float[,] nn = new float[1, 3];
//                //    alglib.kdtreequeryknn(kdtree, _source.Column(i).AsArray(), 1);
//                //    alglib.kdtreequeryresultsx(kdtree, ref nn);
//                //    Q.SetColumn(i, new float[] { nn[0, 0], nn[0, 1], nn[0, 2] });
//                //}

//                Parallel.For(0, _source.ColumnCount, (i) =>
//                {
//                    var n = tree.GetNearestNeighbours(_source.Column(i).AsArray(), 1);
//                    if (n.Length > 0)
//                    {
//                        Q.SetColumn(i, n[0].Point);
//                    }
//                });

//                float mu = par.Mu;

//                for (int outer = 0; outer < par.MaxOuter; outer++)
//                {
//                    float dual = 0.0;
//                    for (int inner = 0; inner < par.MaxInner; inner++)
//                    {
//                        Z = _source - Q + C / mu;

//                        Z = Shrink(3, Z, mu, par.P);

//                        Matrix<float> U = Q + Z - C / mu;
//                        Transform(ref _source, ref U, Vector<float>.Build.Dense(_source.ColumnCount, 1.0));

//                        dual = (_source - Xo1).L1Norm();//.NormalizeColumns(2.0).Enumerate().Max();
//                        _source.CopyTo(Xo1);

//                        if (dual < par.Stop)
//                        {
//                            break;
//                        }
//                    }

//                    Matrix<float> P = _source - Q - Z;
//                    if (!par.UsePenalty)
//                    {
//                        C += (mu * P);
//                    }

//                    if (mu < par.MaxMu)
//                    {
//                        mu *= par.Alpha;
//                    }

//                    float primal = P.L1Norm();//.NormalizeColumns(2.0).Enumerate().Max();
//                    if (primal < par.Stop && dual < par.Stop)
//                    {
//                        break;
//                    }
//                }

//                float stop = (_source - Xo2).L1Norm();//.NormalizeColumns(2.0).Enumerate().Max();
//                _source.CopyTo(Xo2);

//                if (stop < par.Stop)
//                {
//                    break;
//                }

//            }

//            Logger.Log($"iteration count {icp}");
//            return 0;
//        }

//        public int AlignSumSq(SicpParameters par)
//        {

//            // Build the tree

//            //alglib.kdtreebuild(_target.ToArray(), 3, 0, 2, out alglib.kdtree kdtree);
//            //alglib.kdtreeserialize(kdtree, out string asd);

//            KdTree.KdTree<float, float> tree = new KdTree.KdTree<float, float>(3, new KdTree.Math.floatMath());
//            for (int i = 0; i < _target.ColumnCount; i++)
//            {
//                tree.Add(_target.Column(i).AsArray(), 100);
//            }

//            Matrix<float> Q = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> Z = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> C = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);

//            Matrix<float> Xo1 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            Matrix<float> Xo2 = Matrix<float>.Build.Dense(3, _source.ColumnCount, 0);
//            _source.CopyTo(Xo1);
//            _source.CopyTo(Xo2);
//            var stop = Math.Pow(par.Stop, 2);
//            int icp = 0;
//            for (; icp < par.MaxIcp; icp++)
//            {
//                Logger.Log($"iteration: {icp + 1}");
//                //for (int i = 0; i < _source.ColumnCount; i++)
//                //{
//                //    float[,] nn = new float[1, 3];
//                //    alglib.kdtreequeryknn(kdtree, _source.Column(i).AsArray(), 1);
//                //    alglib.kdtreequeryresultsx(kdtree, ref nn);
//                //    Q.SetColumn(i, new float[] { nn[0, 0], nn[0, 1], nn[0, 2] });
//                //}

//                Parallel.For(0, _source.ColumnCount, (i) =>
//                {
//                    var n = tree.GetNearestNeighbours(_source.Column(i).AsArray(), 1);
//                    if (n.Length > 0)
//                    {
//                        Q.SetColumn(i, n[0].Point);
//                    }
//                });

//                float mu = par.Mu;

//                for (int outer = 0; outer < par.MaxOuter; outer++)
//                {
//                    float dual = 0.0;
//                    for (int inner = 0; inner < par.MaxInner; inner++)
//                    {
//                        Z = _source - Q + C / mu;

//                        Z = Shrink(3, Z, mu, par.P);

//                        Matrix<float> U = Q + Z - C / mu;
//                        Transform(ref _source, ref U, Vector<float>.Build.Dense(_source.ColumnCount, 1.0));

//                        var i = (_source - Xo1);

//                        var j = i.EnumerateColumns().Select(v=>v.Select(c=>c*c).Sum()).Max();//.NormalizeColumns(2.0).Enumerate().Max();
//                        _source.CopyTo(Xo1);
//                        dual = j;
//                        if (dual < stop)
//                        {
//                            break;
//                        }
//                    }

//                    Matrix<float> P = _source - Q - Z;
//                    if (!par.UsePenalty)
//                    {
//                        C += (mu * P);
//                    }

//                    if (mu < par.MaxMu)
//                    {
//                        mu *= par.Alpha;
//                    }

//                    float primal = P.EnumerateColumns().Select(v => v.Select(c => c * c).Sum()).Max();//.NormalizeColumns(2.0).Enumerate().Max();
//                    if (primal < stop && dual < stop)
//                    {
//                        break;
//                    }
//                }

//                float stop2 = (_source - Xo2).EnumerateColumns().Select(v => v.Select(c => c * c).Sum()).Max();//.NormalizeColumns(2.0).Enumerate().Max();
//                _source.CopyTo(Xo2);

//                if (stop2 < stop)
//                {
//                    break;
//                }

//            }

//            Logger.Log($"iteration count {icp}");
//            return 0;
//        }
//    }

//}

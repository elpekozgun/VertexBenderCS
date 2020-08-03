using Engine.Core;
using Engine.GLApi;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Statistics;
using OpenTK;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Class for URoom, ICP implementation
/// </summary>

namespace Engine.Processing
{
    public class ICP
    {
        private const float ErrorThreshold = 0.0000001f;
        private const float MaxIter = 1000;
        private const int RandomSampleCount = 200;

        private List<Vector<float>> _constant;
        private List<Vector<float>> _addition;
        private Stopwatch _watch = new Stopwatch();


        public Matrix4 Transformation { get; private set; }
        public List<Vector3> Result
        {
            get
            {
                List<Vector3> r = new List<Vector3>();
                for (int i = 0; i < _addition.Count; i++)
                {
                    r.Add(new Vector3(_addition[i][0], _addition[i][1], _addition[i][2]));
                }
                return r;
            }
        }

        public ICP(List<Vector3> addition, List<Vector3> constant)
        {
            _constant = new List<Vector<float>>();
            _addition = new List<Vector<float>>();

            for (int i = 0; i < constant.Count; i++)
            {
                _constant.Add(Vector<float>.Build.Dense(new float[] { constant[i].X, constant[i].Y, constant[i].Z }));
            }

            for (int i = 0; i < addition.Count; i++)
            {
                _addition.Add(Vector<float>.Build.Dense(new float[] { addition[i].X, addition[i].Y, addition[i].Z }));
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

        public void Align()
        {
            _watch.Reset();
            _watch.Start();

            KdTree.KdTree<float, float> constantTree = new KdTree.KdTree<float, float>(3, new KdTree.Math.FloatMath());
            Matrix<float> rotation = Matrix<float>.Build.Dense(3, 3, 0);
            Vector<float> translation = Vector<float>.Build.Dense(3, 0);
            
            var constantCenter = GetMean(_constant);
            var rng = new Random();

            for (int i = 0; i < _constant.Count; i++)
            {
                constantTree.Add(_constant[i].ToArray(), 100);
            }

            int it = 0;
            float error = float.PositiveInfinity;
            while (it++ < MaxIter && error > ErrorThreshold)
            {

                Matrix<float> W = Matrix<float>.Build.Dense(3, 3, 0);
                
                var additionCenter = GetMean(_addition);

                error = 0;
                for (int i = 0; i < RandomSampleCount; i++)
                {
                    var rn = rng.Next(_addition.Count);

                    var n = constantTree.GetNearestNeighbours(_addition[rn].AsArray(), 1);

                    if (n.Length > 0)
                    {
                        var p = Vector<float>.Build.Dense(n[0].Point);
                        var qs = p - constantCenter;
                        var qd = _addition[rn] - additionCenter;

                        W += qs.OuterProduct(qd);

                        var v = _constant[rn] - rotation * _addition[rn] - translation;
                        error += v.DotProduct(v);
                    }
                }
                error /= RandomSampleCount;

                var svd = W.Svd(true);
                rotation = svd.U * svd.VT;
                translation = constantCenter - rotation * additionCenter;

                for (int i = 0; i < _addition.Count; i++)
                {
                    _addition[i] = rotation * _addition[i] + translation;
                }

            }

            Logger.Log($"sample count {RandomSampleCount}");
            Logger.Log($"iteration count {it}");
            Logger.Log($"error: {error}");
            _watch.Stop();
            Logger.Log($"ellapsed: {_watch.ElapsedMilliseconds} ms");
        }

    }
}

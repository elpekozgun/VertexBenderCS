using Engine.Core;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.Optimization;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class for URoom, ICP implementation
/// </summary>

namespace Engine.Processing
{
    public class ICP
    {
        private const float ERROR_THRESHOLD = 0.00001f;
        private const float MAX_ITER = 400;
        private const int RND_SAMPLE_COUNT = 50;

        private List<Vector<float>> _constant;
        private List<Vector<float>> _addition;

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
            KdTree.KdTree<float, float> constantTree = new KdTree.KdTree<float, float>(3, new KdTree.Math.FloatMath());

            for (int i = 0; i < _constant.Count; i++)
            {
                constantTree.Add(_constant[i].ToArray(), 100);
            }

            var constantCenter = GetMean(_constant);

            var rng = new Random();

            int it = 0;
            float error = float.PositiveInfinity;
            while (it++ < MAX_ITER && error > ERROR_THRESHOLD)
            {
                


                // Random Sampling.
                List<Vector<float>> correspondence = new List<Vector<float>>();

                for (int i = 0; i < _addition.Count; i++)
                {
                    var n = constantTree.GetNearestNeighbours(_addition[i].AsArray(), 1);

                    if (n.Length > 0 /* && maxDistance < maxDistanceThreshold */)
                    {
                        correspondence.Add(Vector<float>.Build.Dense(new float[] { n[0].Point[0], n[0].Point[1], n[0].Point[2] }));
                    }
                }

                var additionCenter = GetMean(_addition);

                for (int i = 0; i < _addition.Count; i++)
                {
                    correspondence[i] -= additionCenter;
                }








                Matrix<float> W = Matrix<float>.Build.Dense(3, 3, 0);

                for (int i = 0; i < _addition.Count; i++)
                {
                    W += correspondence[i].OuterProduct(_constant[i]);
                }
                var svd = W.Svd(true);

                var rotation = svd.U * svd.VT;
                var translation = constantCenter - rotation * additionCenter;

                float err = 0;
                for (int i = 0; i < _addition.Count; i++)
                {
                    var v = _constant[i] - rotation * _addition[i] - translation;
                    err += v.DotProduct(v);
                }
                err /= _addition.Count;

                for (int i = 0; i < _addition.Count; i++)
                {
                    _addition[i] = rotation * _addition[i] + translation;
                }

                error = err;
            }
        }


    }
}

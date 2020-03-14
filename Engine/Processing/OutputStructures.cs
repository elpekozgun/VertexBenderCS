using System.Collections.Generic;

namespace Engine.Processing
{
    public struct ShortestPathOutput
    {
        public float TargetDistance;
        public List<int> Path;

        public ShortestPathOutput(float targetDistance, List<int> path)
        {
            TargetDistance = targetDistance;
            Path = path;
        }
    }

    public struct DijkstraOutput
    {
        public float[] AllDistances;
        public float MaxDistance;

        public DijkstraOutput(float[] allDistances, float maxDistance)
        {
            AllDistances = allDistances;
            MaxDistance = maxDistance;
        }
    }

    public struct SampleOutput
    {
        public List<GraphNode> SamplePoints;
        public List<int> SampleIndices;

        public SampleOutput(List<GraphNode> samplePoints, List<int> sampelIndices)
        {
            SamplePoints = samplePoints;
            SampleIndices = sampelIndices;
        }
    }

    public struct IsoCurveOutput
    {
        public List<List<OpenTK.Vector3>> IsoCurves;
        //public List<List<Vector3>> IsoCurves;
        public float[] IsoCurveDistances;

        public IsoCurveOutput(List<List<OpenTK.Vector3>> isoCurves, float[] isoCurveDistances)
        {
            IsoCurves = isoCurves;
            IsoCurveDistances = isoCurveDistances;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Processing
{
    public enum eOutputType
    {
        shortestPath,
        DijkstraComplete,
        Sampling,
        IsoCurve,
        AverageGeodesicDistance,
        GeodesicMatrix
    }

    public interface IOutput
    {
        long Duration { get; }
        eOutputType Type { get; }
        string Info { get; }
    }

    public struct CancelOutput : IOutput
    {
        long IOutput.Duration => 0;
        eOutputType IOutput.Type => eOutputType.shortestPath;
        public string Info => "Cancelled by user";
    }

    public struct ShortestPathOutput : IOutput
    {
        public float TargetDistance;
        public List<int> Path;
        public long Duration { get; private set; }
        public eOutputType Type => eOutputType.shortestPath;
        public eShortestPathMethod Method { get; private set; }
        public string Info => "Shortest path with " +  Enum.GetName(typeof(eShortestPathMethod), Method) + " calculated in: " + Duration.ToString() + " ms." + "Target Distance: " + TargetDistance;

        public ShortestPathOutput(eShortestPathMethod method, float targetDistance, List<int> path, long duration)
        {
            Method = method;
            TargetDistance = targetDistance;
            Path = path;
            Duration = duration;
        }
        
    }

    public struct DijkstraOutput : IOutput
    {
        public float[] AllDistances;
        public float MaxDistance;
        public long Duration { get; private set; }
        public eOutputType Type => eOutputType.DijkstraComplete;
        public string Info => "Dijkstra calculated: " + Duration.ToString() + " ms." + "Max Distance: " + MaxDistance;

        public DijkstraOutput(float[] allDistances, float maxDistance, long duration)
        {
            AllDistances = allDistances;
            MaxDistance = maxDistance;
            Duration = duration;
        }
    }

    public struct SampleOutput : IOutput
    {
        public List<GraphNode> SamplePoints;
        public List<int> SampleIndices;
        public long Duration { get; private set; }
        public eOutputType Type => eOutputType.Sampling;
        public string Info => "Farthest point sample calculated in: " + Duration.ToString() + " ms." + "Sample Count: " + SampleIndices.Count + ". samples: " + ConvertIndicesTostring(); 

        public SampleOutput(List<GraphNode> samplePoints, List<int> sampelIndices, long duration)
        {
            SamplePoints = samplePoints;
            SampleIndices = sampelIndices;
            Duration = duration;
        }

        private string ConvertIndicesTostring()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in SampleIndices)
            {
                sb.Append(item.ToString() + " ");
            }
            return sb.ToString();
        }
    }

    public struct IsoCurveOutput : IOutput
    {
        public List<List<OpenTK.Vector3>> IsoCurves;
        public float[] IsoCurveDistances;
        public long Duration { get; private set; }
        public eOutputType Type => eOutputType.IsoCurve;
        public int SourceIndex;
        public string Info => "Iso-Curve Signature calculated in: " + Duration.ToString() + " ms.";

        public IsoCurveOutput(List<List<OpenTK.Vector3>> isoCurves, float[] isoCurveDistances, int sourceIndex, long duration)
        {
            IsoCurves = isoCurves;
            IsoCurveDistances = isoCurveDistances;
            Duration = duration;
            SourceIndex = sourceIndex;
        }

    }

    public struct GeodesicMatrixOutput : IOutput
    {
        public float[][] Matrix;
        public long Duration { get; private set; }
        public eOutputType Type => eOutputType.GeodesicMatrix;
        public string Info => "Shortest Path Calculated in: " + Duration.ToString() + " ms.";

        public GeodesicMatrixOutput(float[][] matrix, long duration)
        {
            Matrix = matrix;
            Duration = duration;
        }
    }

    public struct AverageGeodesicOutput : IOutput
    {
        public float[] Distances;
        public long Duration { get; private set; }
        public eOutputType Type => eOutputType.AverageGeodesicDistance;
        public string Info => "Average geodesic distances calculated in: " + Duration.ToString() + " ms.";

        public AverageGeodesicOutput(float[] distances, long duration)
        {
            Distances = distances;
            Duration = duration;
        }


    }
}

﻿using OpenTK;
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
        GeodesicMatrix,
        DiscParametrization,
        SphereParametrization
    }

    public interface IOutput
    {
        eOutputType Type { get; }
    }

    public struct CancelOutput : IOutput
    {
        eOutputType IOutput.Type => eOutputType.shortestPath;
    }

    public struct ShortestPathOutput : IOutput
    {
        public float TargetDistance;
        public List<int> Path;
        public eOutputType Type => eOutputType.shortestPath;
        public eShortestPathMethod Method { get; private set; }

        public ShortestPathOutput(eShortestPathMethod method, float targetDistance, List<int> path, long duration)
        {
            Method = method;
            TargetDistance = targetDistance;
            Path = path;
        }
        
    }

    public struct DijkstraOutput : IOutput
    {
        public float[] AllDistances;
        public float MaxDistance;
        public eOutputType Type => eOutputType.DijkstraComplete;

        public DijkstraOutput(float[] allDistances, float maxDistance, long duration)
        {
            AllDistances = allDistances;
            MaxDistance = maxDistance;
        }
    }

    public struct SampleOutput : IOutput
    {
        public List<GraphNode> SamplePoints;
        public List<int> SampleIndices;
        public eOutputType Type => eOutputType.Sampling;

        public SampleOutput(List<GraphNode> samplePoints, List<int> sampelIndices, long duration)
        {
            SamplePoints = samplePoints;
            SampleIndices = sampelIndices;
        }
    }

    public struct IsoCurveOutput : IOutput
    {
        public List<List<OpenTK.Vector3>> IsoCurves;
        public float[] IsoCurveDistances;
        public eOutputType Type => eOutputType.IsoCurve;
        public int SourceIndex;

        public IsoCurveOutput(List<List<OpenTK.Vector3>> isoCurves, float[] isoCurveDistances, int sourceIndex, long duration)
        {
            IsoCurves = isoCurves;
            IsoCurveDistances = isoCurveDistances;
            SourceIndex = sourceIndex;
        }

    }

    public struct GeodesicMatrixOutput : IOutput
    {
        public float[][] Matrix;
        public eOutputType Type => eOutputType.GeodesicMatrix;

        public GeodesicMatrixOutput(float[][] matrix, long duration)
        {
            Matrix = matrix;
        }
    }

    public struct AverageGeodesicOutput : IOutput
    {
        public float[] Distances;
        public eOutputType Type => eOutputType.AverageGeodesicDistance;

        public AverageGeodesicOutput(float[] distances, long duration)
        {
            Distances = distances;
        }


    }

    public struct DiscParameterizeOutput : IOutput
    {
        public List<Vector2> Output;

        public DiscParameterizeOutput(List<Vector2> output)
        {
            Output = output;
        }

        public eOutputType Type => eOutputType.DiscParametrization;

        public Vector2[] NormalizedUVCoords()
        {
            Vector2[] UVcoords = new Vector2[Output.Count];

            float minY = float.MaxValue;
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i = 0; i < Output.Count; i++)
            {
                minY = Output[i].Y < minY ? Output[i].Y : minY;
                minX = Output[i].X < minX ? Output[i].X : minX;
                maxX = Output[i].X > maxX ? Output[i].X : maxX;
                maxY = Output[i].Y > maxY ? Output[i].Y : maxY;
            }

            var normalizer = Math.Max(maxX - minX, maxY - minY);

            for (int i = 0; i < Output.Count; i++)
            {
                UVcoords[i] = new Vector2(Output[i].X - minX, Output[i].Y - minY) / normalizer;
            }

            return UVcoords;
        }

    }

}

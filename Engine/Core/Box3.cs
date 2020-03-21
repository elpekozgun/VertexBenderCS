﻿using OpenTK;
using System;
using System.Collections.Generic;

namespace Engine.Core
{
    public struct Box3
    {
        public float Top;
        public float Bottom;
        public float Right;
        public float Left;
        public float Front;
        public float Back;

        public Box3(float top, float bottom, float right, float left, float front, float back)
        {
            Top = top;
            Bottom = bottom;
            Right = right;
            Left = left;
            Front = front;
            Back = back;
        }

        public static Box3 CalculateBoundingBox(List<Vertex> vertices)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            foreach (var item in vertices)
            {
                minX = minX > item.Coord.X ? item.Coord.X : minX;
                minY = minY > item.Coord.Y ? item.Coord.Y : minY;
                minZ = minZ > item.Coord.Z ? item.Coord.Z : minZ;
                maxX = maxX < item.Coord.X ? item.Coord.X : maxX;
                maxY = maxY < item.Coord.Y ? item.Coord.Y : maxY;
                maxZ = maxZ < item.Coord.Z ? item.Coord.Z : maxZ;
            }

            return new Box3(maxY, minY, maxX, minX, maxZ, minZ);
        }

        public static Box3 CalculateBoundingBox(Dictionary<int,Vertex> vertices)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            foreach (var item in vertices)
            {
                minX = minX > item.Value.Coord.X ? item.Value.Coord.X : minX;
                minY = minY > item.Value.Coord.Y ? item.Value.Coord.Y : minY;
                minZ = minZ > item.Value.Coord.Z ? item.Value.Coord.Z : minZ;
                maxX = maxX < item.Value.Coord.X ? item.Value.Coord.X : maxX;
                maxY = maxY < item.Value.Coord.Y ? item.Value.Coord.Y : maxY;
                maxZ = maxZ < item.Value.Coord.Z ? item.Value.Coord.Z : maxZ;
            }

            return new Box3(maxY, minY, maxX, minX, maxZ, minZ);
        }

        public float Volume
        {
            get
            {
                return Math.Abs((Top - Bottom) * (Right - Left) * (Front - Back));
            }
        }

        public Vector3 Center
        {
            get
            {
                return new Vector3
                (
                    0.5f * (Right + Left),
                    0.5f * (Top + Bottom),
                    0.5f * (Front + Back)
                ); 
            }
        }


        public bool Contains(Vector3 point)
        {
            return point.X <= Right && point.X >= Left && point.Y <= Top && point.Y >= Bottom && point.Z <= Front && point.Z >= Back;
        }
    }
}

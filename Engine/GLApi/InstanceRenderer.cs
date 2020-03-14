﻿using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Engine.GLApi
{

    //TODO: Use this for multi same object rendering such as cubes in FarthestPointSampling.
    public class InstanceRenderer : IDisposable, IRenderable
    {
        public Shader Shader { get; set; }
        public Vector3 Center { get; private set; }

        private GpuVertex[] vertices;
        private int[] indices;

        private bool _initialized;

        private int _VAO;
        private int _VBO;
        private int _EBO;

        public List<Mesh> Mesh { get; set; }

        private Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Coord += _position;
                }
            }
        }

        public Transform Transform { get; set; }

        private void ExtractVertices(Mesh mesh)
        {
            vertices = new GpuVertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertices[i] = new GpuVertex()
                {
                    Coord = mesh.Vertices[i].Coord,
                    Normal = mesh.Vertices[i].Normal,
                    Color = new Vector3(0.0f, 0.0f, 0.0f)
                };
            }

            indices = new int[mesh.Triangles.Count * 3];
            for (int i = 0; i < mesh.Triangles.Count; i++)
            {
                var tri = mesh.Triangles[i];

                indices[i * 3] = tri.V1;
                indices[i * 3 + 1] = tri.V2;
                indices[i * 3 + 2] = tri.V3;
            }
        }

        public InstanceRenderer(List<Mesh> mesh)
        {
            ExtractVertices(mesh[0]);
            Setup();
            _initialized = true;
            Mesh = new List<Mesh>();
        }

        private void Setup()
        {
            GL.GenVertexArrays(1, out _VAO);
            GL.GenBuffers(1, out _VBO);
            GL.GenBuffers(1, out _EBO);

            GL.BindVertexArray(_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * GpuVertex.Size, vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * 4, indices, BufferUsageHint.StaticDraw);



            //coord
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, GpuVertex.Size, 0);

            //normal
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, GpuVertex.Size, 12);

            //normal
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, GpuVertex.Size, 24);

            GL.BindVertexArray(0);

        }

        public void SetColorBuffer(Vector3[] color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = color[i];
            }
            Setup();
        }

        public void Render(Camera cam, eRenderMode mode = eRenderMode.shaded)
        {
            GL.BindVertexArray(_VAO);

            if ((mode & eRenderMode.shaded) == eRenderMode.shaded)
            {
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Fill);
            }
            if ((mode & eRenderMode.wireFrame) == eRenderMode.wireFrame)
            {
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
                GL.PolygonOffset(1.0f, 1.0f);
                GL.LineWidth(1.5f);
                GL.Enable(EnableCap.LineSmooth);
            }
            if ((mode & eRenderMode.pointCloud) == eRenderMode.pointCloud)
            {
                GL.PolygonMode(MaterialFace.Back, PolygonMode.Point);
            }
            GL.DrawElements(BeginMode.Triangles, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

            //int diffuse = 1;
            //int specular = 1;
            //int normal = 1;
            //int height = 1;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.DeleteVertexArray(_VAO);
                    GL.DeleteBuffer(_VBO);
                    GL.DeleteBuffer(_EBO);
                    _initialized = false;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
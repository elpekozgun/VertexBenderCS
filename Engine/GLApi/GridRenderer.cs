using Engine.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Engine.GLApi
{
    public class GridRenderer : Transform, IDisposable, IRenderable
    {
        public Shader Shader { get; set; }
        public Vector3 Center { get; private set; }

        private GpuVertex[] vertices;
        private int[] indices;

        public bool EnableCull { get; set; }
        public bool EnableBlend;
        public bool EnableDepth;

        public bool ShowGrid { get; set; }

        private bool _initialized;

        private int _VAO;
        private int _VBO;
        private int _EBO;

        public Mesh Mesh { get; private set; }

        public Vector4 Color { get; set; }

        public bool ShowBoundingBox { get; set; }

        public void SetMesh(Mesh mesh)
        {
            ExtractVertices(mesh);
            Setup();
            Mesh = mesh;
        }

        private void ExtractVertices(Mesh mesh)
        {
            vertices = new GpuVertex[mesh.Vertices.Count];
            Dictionary<int, int> map = new Dictionary<int, int>();
            int i = 0;
            foreach (var vertex in mesh.Vertices)
            {
                map.Add(vertex.Key, i);
                vertices[i] = new GpuVertex()
                {
                    Coord = vertex.Value.Coord,
                    Normal = vertex.Value.Normal,
                    Color = new Vector3(0.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0.0f, 0.0f)
                };
                i++;
            }

            i = 0;
            indices = new int[mesh.Edges.Count * 2];
            foreach (var edge in mesh.Edges)
            {
                indices[i * 2] = map[edge.Value.Start];
                indices[i * 2 + 1] = map[edge.Value.End];
                i++;
            }

        }

        public GridRenderer(Mesh mesh, string name = "")
            : base(name)
        {
            ExtractVertices(mesh);
            Setup();
            _initialized = true;
            Mesh = mesh;
            Shader = Shader.Unlit;
            EnableCull = true;
            EnableDepth = true;
            EnableBlend = false;
            IsEnabled = true;
            ShowGrid = true;
        }

        public GridRenderer(Mesh mesh, Shader shader, string name = "") : this(mesh, name)
        {
            Shader = shader;
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

            GL.BindVertexArray(0);

            GL.PointSize(1);
            GL.PolygonOffset(1.0f, 2);
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
            if (!ShowGrid)
            {
                return;
            }
            GL.BindVertexArray(_VAO);

            if (EnableBlend)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            }

            if (!EnableDepth)
            {
                GL.Disable(EnableCap.DepthTest);
            }


            if (!EnableCull)
            {
                GL.Disable(EnableCap.CullFace);
            }

            Shader.Use();
            Shader.SetMat4("Model", ModelMatrix);
            Shader.SetMat4("View", cam.View);
            Shader.SetMat4("Projection", cam.Projection);
            Shader.SetVec4("Color", Color);
            Shader.SetFloat("Alpha", Color.W);


            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.DrawElements(BeginMode.Lines, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
            GL.Disable(EnableCap.PolygonSmooth);
            if (!EnableCull)
            {
                GL.Enable(EnableCap.CullFace);
            }
            if (!EnableDepth)
            {
                GL.Enable(EnableCap.DepthTest);
            }
            GL.BindVertexArray(0);

            GL.Disable(EnableCap.Blend);
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

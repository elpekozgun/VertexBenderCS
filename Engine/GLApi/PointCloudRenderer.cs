using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using System.Drawing;
using System.Drawing.Imaging;

namespace Engine.GLApi
{

    public class PointCloudRenderer : Transform, IDisposable, IRenderable
    {
        public Shader Shader { get; set; }
        public Vector3 Center { get; private set; }
        public Texture DiffuseTexture { get; set; }

        private GpuVertex[] vertices;

        private bool _initialized;

        private int _VAO;
        private int _VBO;

        public Mesh Mesh { get; set; }

        public Vector4 Color { get; set; }

        private void ExtractVertices(Mesh mesh, List<Vector3> intensity)
        {
            vertices = new GpuVertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertices[i] = new GpuVertex()
                {
                    Coord = mesh.Vertices[i].Coord,
                    Normal = mesh.Vertices[i].Normal,
                    Color = intensity[i]
                };
            }
        }

        public PointCloudRenderer(Mesh mesh, List<Vector3> intensity, string name = "")
            : base(name)
        {
            ExtractVertices(mesh, intensity);
            Setup();
            _initialized = true;
            Mesh = mesh;
            Shader = Shader.DefaultShader;
        }

        public PointCloudRenderer(Mesh mesh, List<Vector3> intensity, Shader shader, string name = "") : this(mesh, intensity, name)
        {
            Shader = shader;
        }

        private void Setup()
        {
            GL.GenVertexArrays(1, out _VAO);
            GL.GenBuffers(1, out _VBO);

            GL.BindVertexArray(_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * GpuVertex.Size, vertices, BufferUsageHint.StaticDraw);

            //coord
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, GpuVertex.Size, 0);

            //normal
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, GpuVertex.Size, 12);

            //color
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
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, DiffuseTexture.Id);

            GL.BindVertexArray(_VAO);

            var unlit = Shader.DefaultUnlitShader;

            unlit.Use();
            unlit.SetMat4("Model", ModelMatrix);
            unlit.SetMat4("View", cam.View);
            unlit.SetMat4("Projection", cam.Projection);
            unlit.SetVec4("Color", Color);

            var temp = Shader;
            Shader = unlit;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            GL.PointSize(1);

            GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length);
            Shader = temp;

            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.DeleteVertexArray(_VAO);
                    GL.DeleteBuffer(_VBO);
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

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

        public int Min { get; private set; }
        public int Max { get; private set; }

        public void SetMax(int max)
        {
            Max = max;
            //Shader.Use();
            //Shader.SetFloat("MaxIntensity", max / 255.0f);
        }

        public void SetMin(int min)
        {
            Min = min;
        }


        public Mesh Mesh { get; set; }

        public Vector4 Color { get; set; }

        private void ExtractVertices(Mesh mesh, List<int> intensities)
        {
            vertices = new GpuVertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertices[i] = new GpuVertex()
                {
                    Coord = mesh.Vertices[i].Coord,
                    Normal = mesh.Vertices[i].Normal,
                    Color =  new Vector3((float)intensities[i] / 255.0f, 0.5f * (float)intensities[i] / 255.0f, 0)
                };
            }
        }

        public PointCloudRenderer(Mesh mesh, List<int> intensities, int min = 0, int max = 255, string name = "")
            : base(name)
        {
            Max = max;
            Min = min;
            ExtractVertices(mesh, intensities);
            Setup();
            _initialized = true;
            Mesh = mesh;
            Shader = Shader.DefaultPointCloud;
        }

        public PointCloudRenderer(Mesh mesh, List<int> intensity, Shader shader, int min = 0, int max = 255, string name = "") : this(mesh, intensity, min, max, name)
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

            var pointCloudShader = Shader.DefaultPointCloud;

            pointCloudShader.Use();
            pointCloudShader.SetMat4("Model", ModelMatrix);
            pointCloudShader.SetMat4("View", cam.View);
            pointCloudShader.SetMat4("Projection", cam.Projection);
            pointCloudShader.SetVec4("Color", Color);
            pointCloudShader.SetFloat("MaxIntensity", (float)Max / 255.0f);
            pointCloudShader.SetFloat("MinIntensity", (float)Min / 255.0f);

            var temp = Shader;
            Shader = pointCloudShader;

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

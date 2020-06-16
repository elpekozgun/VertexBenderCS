using Engine.Core;
using Engine.Processing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

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

        public bool IsCuberille { get; set; }
        public bool EnableCull { get; set; }
        public int Intensity { get; set; }

        private VolOutput rawVolData;
        private VolOutput subVolData;

        public Vector4 Color { get; set; }

        public bool ShowBoundingBox { get; set; }

        public int DownSample { get; set; }

        private int _currentDownSample;

        public PointCloudRenderer(VolOutput vol, string name = "")
            : base(name)
        {
            rawVolData = vol;
            subVolData = vol;

            Shader = Shader.PointCloud;
            UpdateInputFromVol(rawVolData);

            IsEnabled = true;
            _initialized = true;
            IsCuberille = false;

            DownSample = 4;
            Intensity = 60;
        }

        public PointCloudRenderer(VolOutput vol, Shader shader, string name = "") : this(vol, name)
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

        private void UpdateInputFromVol(VolOutput vol)
        {
            vertices = new GpuVertex[vol.IntensityMap.Length];
            int i = 0;
            foreach (var item in vol.IntensityMap)
            {
                vertices[i] = new GpuVertex()
                {
                    Coord = item.Key * vol.Spacing,
                    Normal = Vector3.Zero,
                    Color = new Vector3((float)item.Value / vol.MaxIntensity , 0.5f * (float)item.Value / vol.MaxIntensity, 0.0f)
                };
                i++;

            }
            Setup();
        }


        public void Render(Camera cam, eRenderMode mode = eRenderMode.shaded)
        {
            if (_currentDownSample != DownSample)
            {
                subVolData = Algorithm.Downsample(rawVolData, DownSample);
                UpdateInputFromVol(subVolData);
                _currentDownSample = DownSample;
            }

            if (EnableCull)
            {
                GL.Enable(EnableCap.CullFace);
            }

            GL.BindVertexArray(_VAO);
            if (IsCuberille)
            {
                Shader = Shader.CuberilleGeometry;
            }
            else
            {
                Shader = Shader.PointCloud;
            }

            Shader.Use();
            Shader.SetMat4("Model", ModelMatrix);
            Shader.SetMat4("View", cam.View);
            Shader.SetMat4("Projection", cam.Projection);
            Shader.SetVec4("OutColor", Color);
            Shader.SetFloat("Intensity", (float)Intensity / subVolData.MaxIntensity);
            Shader.SetFloat("Spacing", subVolData.Spacing);

            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.PointSize(2);

            GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length);

            GL.BindVertexArray(0);

            if (EnableCull)
            {
                GL.Disable(EnableCap.CullFace);
            }
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

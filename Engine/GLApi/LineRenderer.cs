using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Engine.Core;

namespace Engine.GLApi
{

    public class LineRenderer : Transform, IDisposable, IRenderable
    {
        public Shader Shader { get; set; }

        private bool _initialized;
        public bool RenderWithDepth;

        private int _VAO;
        private int _VBO;

        private Vector3[] _vertices;

        public Vector4 Color { get; set; }

        public LineRenderer(List<Vector3> vertices, string name = "") 
            : base(name)
        {
            _vertices = vertices.ToArray();
            Setup();
            Shader = Shader.DefaultUnlitShader;
            Color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
        }

        public LineRenderer(List<Vector3> vertices, Shader shader, string name = "") 
            : this(vertices, name)
        {
            Shader = shader;
        }


        public void Render(Camera cam, eRenderMode mode = eRenderMode.shaded)
        {
            Shader.Use();
            Shader.SetMat4("Model", ModelMatrix);
            Shader.SetMat4("View", cam.View);
            Shader.SetMat4("Projection", cam.Projection);
            Shader.SetVec4("Color", Color);

            GL.CullFace(CullFaceMode.FrontAndBack);
            GL.BindVertexArray(_VAO);

            if ((mode & eRenderMode.shaded) == eRenderMode.shaded || (mode & eRenderMode.wireFrame) == eRenderMode.wireFrame)
            {
                GL.LineWidth(4);
                GL.DrawArrays(PrimitiveType.Lines, 0, _vertices.Length);
            }
            if ((mode & eRenderMode.pointCloud) == eRenderMode.pointCloud)
            {
                Shader.SetVec4("Color", new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
                GL.PointSize(5);
                GL.DrawArrays(PrimitiveType.Points, 0, _vertices.Length);
            }

            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        public void Setup()
        {
            GL.GenVertexArrays(1, out _VAO);
            GL.GenBuffers(1, out _VBO);

            GL.BindVertexArray(_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * 3 * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //coord
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);

            GL.Enable(EnableCap.ProgramPointSize);
            GL.Enable(EnableCap.LineSmooth);
        }

    }
}

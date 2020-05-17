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

    public class PrimitiveRenderer : Transform, IDisposable, IRenderable
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

        private void ExtractVertices(Mesh mesh)
        {
            vertices = new GpuVertex[mesh.Vertices.Count];
            int i = 0;
            foreach(var vertex in mesh.Vertices)
            {
                vertices[i] = new GpuVertex()
                {
                    Coord = vertex.Value.Coord,
                    Normal = vertex.Value.Normal,
                    Color = new Vector3(0.0f, 0.0f, 0.0f)
                };
                i++;
            }
        }

        public PrimitiveRenderer(Mesh mesh, string name = "")
            : base(name)
        {
            ExtractVertices(mesh);
            DiffuseTexture = Texture.LoadTexture(@"Resources\Image\Blank1024.png", eTextureType.Diffuse);
            Setup();
            _initialized = true;
            Mesh = mesh;
            Shader = Shader.DefaultShader;
        }

        public PrimitiveRenderer(Mesh mesh, Shader shader, string name = "") : this(mesh, name)
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

            GL.Disable(EnableCap.CullFace);

            if ((mode & eRenderMode.shaded) == eRenderMode.shaded)
            {
                Shader.Use();
                Shader.SetInt("material.diffuse", (int)TextureUnit.Texture0);
                Shader.SetMat4("Model", ModelMatrix);
                Shader.SetMat4("View", cam.View);
                Shader.SetMat4("Projection", cam.Projection);
                Shader.SetVec4("Color", Color);

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                //GL.Enable(EnableCap.PolygonSmooth);

                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length); 

            }
            if ((mode & eRenderMode.wireFrame) == eRenderMode.wireFrame)
            {
                var unlit = Shader.DefaultUnlitShader;

                unlit = Shader.DefaultUnlitShader;
                unlit.Use();
                unlit.SetMat4("Model", ModelMatrix);
                unlit.SetMat4("View", cam.View);
                unlit.SetMat4("Projection", cam.Projection);
                unlit.SetVec4("Color", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
                var temp = Shader;
                Shader = unlit;

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.LineWidth(2.0f);
                //GL.Enable(EnableCap.LineSmooth);

                GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Length); // vertices.Length);
                Shader = temp;
            }
            if ((mode & eRenderMode.pointCloud) == eRenderMode.pointCloud)
            {
                var unlit = Shader.DefaultUnlitShader;

                unlit.Use();
                unlit.SetMat4("Model", ModelMatrix);
                unlit.SetMat4("View", cam.View);
                unlit.SetMat4("Projection", cam.Projection);
                unlit.SetVec4("Color", new Vector4(1.0f, 1.0f, 0.0f, 1.0f));

                var temp = Shader;
                Shader = unlit;

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
                GL.PointSize(5);

                GL.DrawArrays(PrimitiveType.Points, 0, vertices.Length);
                Shader = temp;
            }

            GL.Enable(EnableCap.CullFace);

            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);

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

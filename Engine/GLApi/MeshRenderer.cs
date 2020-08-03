using Engine.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Engine.GLApi
{
    public enum eRenderMode : byte
    {
        shaded = 1,
        wireFrame = 2,
        pointCloud = 4
    }

    public class MeshRenderer : Transform, IDisposable, IRenderable
    {
        public Shader Shader { get; set; }
        public Vector3 Center { get; private set; }

        public Texture DiffuseTexture { get; set; }

        private GpuVertex[] vertices;
        private int[] indices;

        public bool EnableCull { get; set; }
        public bool EnableBlend;
        public bool EnableDepth;

        private bool _initialized;

        private int _VAO;
        private int _VBO;
        private int _EBO;

        public Mesh Mesh { get; private set; }

        public Vector4 Color { get; set; }

        public bool ShowBoundingBox { get; set; }

        private Shader _normalShader;

        public bool ShowNormals { get; set; }

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
                    Color = new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                    TexCoord = new Vector2(0.0f, 0.0f)
                };
                i++;
            }

            i = 0;
            indices = new int[mesh.Triangles.Count * 3];
            foreach (var tri in mesh.Triangles)
            {
                indices[i * 3] = map[tri.Value.V1];
                indices[i * 3 + 1] = map[tri.Value.V2];
                indices[i * 3 + 2] = map[tri.Value.V3];
                i++;
            }

        }

        public MeshRenderer(Mesh mesh, string name = "")
            : base(name)
        {
            ExtractVertices(mesh);
            DiffuseTexture = Texture.LoadTexture(@"Resources\Image\Blank1024.png", eTextureType.Diffuse);
            Setup();
            _initialized = true;
            Mesh = mesh;
            Shader = Shader.Standard;
            EnableCull = true;
            IsEnabled = true;
            EnableDepth = true;
            _normalShader = Shader.NormalShader;
            ShowNormals = false;
        }

        public MeshRenderer(Mesh mesh, Shader shader, string name = "") : this(mesh, name)
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

            //color
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, GpuVertex.Size, 24);

            //texCoord
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, GpuVertex.Size, 40);

            GL.BindVertexArray(0);

            GL.PointSize(5);
            GL.PolygonOffset(1.0f, 2);
        }

        public void SetColorBuffer(Vector4[] color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = color[i];
            }
            Setup();
        }

        public void SetTextureBuffer(Vector2[] texcoord)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].TexCoord = new Vector2(MathHelper.Clamp(texcoord[i].X, 0, 1), MathHelper.Clamp(texcoord[i].Y, 0, 1));
            }
            Setup();
        }

        public void Render(Camera cam, eRenderMode mode = eRenderMode.shaded)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, DiffuseTexture.Id);

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

            if ((mode & eRenderMode.shaded) == eRenderMode.shaded)
            {

                Shader.Use();
                Shader.SetInt("material.diffuse", (int)TextureUnit.Texture0);
                Shader.SetMat4("Model", ModelMatrix);
                Shader.SetMat4("View", cam.View);
                Shader.SetMat4("Projection", cam.Projection);
                Shader.SetVec4("Color", Color);
                Shader.SetFloat("Alpha", Color.W);


                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                //GL.Enable(EnableCap.PolygonSmooth);

                GL.DrawElements(BeginMode.Triangles, indices.Length * 4, DrawElementsType.UnsignedInt, 0);

                if (ShowNormals)
                {
                    _normalShader.Use();
                    _normalShader.SetMat4("Model", ModelMatrix);
                    _normalShader.SetMat4("View", cam.View);
                    _normalShader.SetMat4("Projection", cam.Projection);
                    GL.LineWidth(1.5f);
                    GL.DrawElements(BeginMode.Points, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
                }
            }
            if ((mode & eRenderMode.wireFrame) == eRenderMode.wireFrame)
            {
                var unlit = Shader.Unlit;

                unlit = Shader.Unlit;
                unlit.Use();
                unlit.SetMat4("Model", ModelMatrix);
                unlit.SetMat4("View", cam.View);
                unlit.SetMat4("Projection", cam.Projection);
                unlit.SetVec4("Color", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
                var temp = Shader;
                Shader = unlit;

                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

                GL.Enable(EnableCap.PolygonOffsetFill);
                GL.LineWidth(1.1f);

                GL.DrawElements(BeginMode.Triangles, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
                GL.Disable(EnableCap.PolygonOffsetLine);

                Shader = temp;
            }
            if ((mode & eRenderMode.pointCloud) == eRenderMode.pointCloud)
            {
                var unlit = Shader.Unlit;

                unlit.Use();
                unlit.SetMat4("Model", ModelMatrix);
                unlit.SetMat4("View", cam.View);
                unlit.SetMat4("Projection", cam.Projection);
                unlit.SetVec4("Color", new Vector4(1.0f, 1.0f, 0.0f, 1.0f));

                var temp = Shader;
                Shader = unlit;

                GL.PolygonMode(MaterialFace.Front, PolygonMode.Point);


                GL.Enable(EnableCap.PolygonOffsetFill);
                GL.DrawElements(BeginMode.Points, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
                GL.Disable(EnableCap.PolygonOffsetLine);

                Shader = temp;
            }
            if (!EnableCull)
            {
                GL.Enable(EnableCap.CullFace);
            }
            if (!EnableDepth)
            {
                GL.Enable(EnableCap.DepthTest);
            }
            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);

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

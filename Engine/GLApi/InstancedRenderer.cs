using Engine.Core;
using OpenTK;
using System;
using OpenTK.Graphics.OpenGL4;

namespace Engine.GLApi
{
    public class InstancedRenderer : Transform, IRenderable, IDisposable
    {
        public Shader Shader { get; set; }
        public Vector3 Center { get; private set; }
        //public Texture DiffuseTexture { get; set; }

        private GpuVertex[] vertices;
        private int[] indices;

        private bool _initialized;

        private int _VAO;
        private int _VBO;
        private int _EBO;


        /*
            Send Cube mesh. Normalized.
            Send positionList for vertices
            do a 3way loop to set position of cubes using vertices.
            Push to buffer in uniform matrix.
            Rip.
        */ 




        public Mesh Mesh { get; private set; }

        public Vector4 Color { get; set; }

        public void SetMesh(Mesh mesh)
        {
            ExtractVertices(mesh);
            Setup();
            Mesh = mesh;
        }

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
                    Color = new Vector3(0.0f, 0.0f, 0.0f),
                    TexCoord = new Vector2(0.0f, 0.0f)
                };
                i++;
            }
            i = 0;
            indices = new int[mesh.Triangles.Count * 3];
            foreach(var tri in mesh.Triangles)
            {
                indices[i * 3] = tri.Value.V1;
                indices[i * 3 + 1] = tri.Value.V2;
                indices[i * 3 + 2] = tri.Value.V3;
                i++;
            }

            //vertices = new GpuVertex[mesh.Vertices.Count];
            //for (int i = 0; i < mesh.Vertices.Count; i++)
            //{
            //    vertices[i] = new GpuVertex()
            //    {
            //        Coord = mesh.Vertices[i].Coord,
            //        Normal = mesh.Vertices[i].Normal,
            //        Color = new Vector3(0.0f, 0.0f, 0.0f),
            //        TexCoord = new Vector2(0.0f, 0.0f)
            //    };
            //}

            //indices = new int[mesh.Triangles.Count * 3];
            //for (int i = 0; i < mesh.Triangles.Count; i++)
            //{
            //    var tri = mesh.Triangles[i];

            //    indices[i * 3] = tri.V1;
            //    indices[i * 3 + 1] = tri.V2;
            //    indices[i * 3 + 2] = tri.V3;
            //}
        }

        public InstancedRenderer(Mesh mesh, int count, string name = "")
            : base(name)
        {
            ExtractVertices(mesh);
            Setup();
            _initialized = true;
            Mesh = mesh;
            Shader = Shader.DefaultShader;
        }

        public InstancedRenderer(Mesh mesh, Shader shader, int count, string name = "") : this(mesh, count, name)
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
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, GpuVertex.Size, 24);

            //texCoord
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, GpuVertex.Size, 36);

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
            GL.BindVertexArray(_VAO);


            if ((mode & eRenderMode.shaded) == eRenderMode.shaded)
            {

                Shader.Use();
                Shader.SetInt("material.diffuse", (int)TextureUnit.Texture0);
                Shader.SetMat4("Model", ModelMatrix);
                Shader.SetMat4("View", cam.View);
                Shader.SetMat4("Projection", cam.Projection);
                Shader.SetVec4("Color", Color);


                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Enable(EnableCap.PolygonSmooth);

                GL.DrawElements(BeginMode.Triangles, indices.Length * 4, DrawElementsType.UnsignedInt, 0);

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

                GL.DrawElements(BeginMode.Triangles, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
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

                GL.PolygonMode(MaterialFace.Front, PolygonMode.Point);
                GL.PointSize(5);

                GL.DrawElements(BeginMode.Points, indices.Length * 4, DrawElementsType.UnsignedInt, 0);
                Shader = temp;
            }
            GL.BindVertexArray(0);




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

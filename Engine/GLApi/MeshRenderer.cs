using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Engine.GLApi
{
    public enum eRenderMode : byte
    {
        shaded = 1,
        wireFrame = 2,
        pointCloud = 4
    }

    public struct GpuVertex
    {
        public Vector3 Coord;
        public Vector3 Normal;

        public static int Size
        {
            get
            {
                return Vector3.SizeInBytes * 2;
            }
        }
    }

    public struct GpuTexture
    {

    }

    public class MeshRenderer : IDisposable, IRenderable
    {
        public Shader Shader { get; set; }
        public Vector3 Center { get; private set; }

        private GpuVertex[] vertices;
        private int[] indices;

        private bool _initialized;

        private int _VAO;
        private int _VBO;
        private int _EBO;

        private void ExtractVertices(Mesh mesh)
        {
            vertices = new GpuVertex[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertices[i] = new GpuVertex()
                {
                    Coord = mesh.Vertices[i].Coord,
                    Normal = mesh.Vertices[i].Normal
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

        public MeshRenderer(Mesh mesh)
        {
            ExtractVertices(mesh);
            Setup();
            _initialized = true;
        }

        private void Setup()
        {
            GL.GenVertexArrays(1,out _VAO);
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
                        
        }

        public void Render(eRenderMode mode = eRenderMode.shaded)
        {
            GL.BindVertexArray(_VAO);

            if ((mode & eRenderMode.shaded)== eRenderMode.shaded)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            if((mode & eRenderMode.wireFrame) == eRenderMode.wireFrame)
            {
                GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
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

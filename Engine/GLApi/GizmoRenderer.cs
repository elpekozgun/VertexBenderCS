using Engine.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Engine.GLApi
{
    public class GizmoRenderer 
    {
        public Vector2 Position;
        public float Aspect;
        public float Radius;

        public Shader Shader;

        private int _VAO;
        private int _VBO;
        private float[] vertices =
        {
             0.1f, -0.1f, 0.0f, 1.0f, 0.0f,
             -0.1f, 0.1f, 0.0f, 0.0f, 1.0f,
             0.1f,  0.1f, 0.0f, 1.0f, 1.0f,
             -0.1f,  -0.1f, 0.0f, 0.0f, 0.0f,
             -0.1f, 0.1f, 0.0f, 0.0f, 1.0f,
             0.1f, -0.1f, 0.0f, 1.0f, 0.0f
        };
        
        public GizmoRenderer()
        {
            Shader = Shader.Gizmo;

            GL.GenVertexArrays(1, out _VAO);
            GL.GenBuffers(1, out _VBO);

            GL.BindVertexArray(_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * 5 * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            //coord
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 12);

            GL.BindVertexArray(0);
        }

        public void Render()
        {
            GL.Disable(EnableCap.CullFace);
            GL.BindVertexArray(_VAO);
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            Shader.Use();
            Shader.SetFloat("border", 0.1f);
            Shader.SetVec2("center", Position);
            Shader.SetFloat("aspect", Aspect);
            Shader.SetVec4("color", new Vector4(1,0.2f,0.2f,0));
            Shader.SetFloat("radius", Radius);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.Enable(EnableCap.PolygonSmooth);

            GL.DrawArrays(PrimitiveType.Triangles,0 , 6);

            GL.Enable(EnableCap.CullFace);
            GL.Disable(EnableCap.Blend);

            GL.BindVertexArray(0);
        }

    }
}

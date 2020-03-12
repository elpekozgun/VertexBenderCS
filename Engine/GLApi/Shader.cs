using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Engine.GLApi
{

    public class Shader
    {
        #region Static Items

        private static Shader _defaultShader = ShaderBuilder.CreateShader
        (
            "shaded", 
            ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\ModelLoadingVertexShaderWithLight.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\ModelLoadingFragmentShaderLighted.glsl", ShaderType.FragmentShader)
        );

        private static Shader _defaultUnlit = ShaderBuilder.CreateShader
        (
            "shaded",
            ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\WireframeVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"D:\DEV\repo\VertexBenderCS\VertexBenderCS\Resources\Shader\WireframeFragment.glsl", ShaderType.FragmentShader)
        );
        
        public static Shader DefaultShader => _defaultShader;
        public static Shader DefaultUnlitShader=> _defaultUnlit;

        #endregion


        public string Name { get; set; }
        private readonly int _id;

        public Shader(string name, int id)
        {
            this.Name = name;
            this._id = id;
        }

        public void Use()
        {
            GL.UseProgram(_id);
        }

        public void SetInt(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(_id, name), value);
        }

        public void SetFloat(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(_id, name), value);
        }

        public void SetVec2(string name, Vector2 value)
        {
            GL.Uniform2(GL.GetUniformLocation(_id, name), value);
        }

        public void SetVec2(string name, float v1, float v2)
        {
            GL.Uniform2(GL.GetUniformLocation(_id, name), v1, v2);
        }

        public void SetVec3(string name, Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(_id, name), value);
        }

        public void SetVec3(string name, float v1, float v2, float v3)
        {
            GL.Uniform3(GL.GetUniformLocation(_id, name), v1, v2, v3);
        }

        public void SetVec4(string name, Vector4 value)
        {
            GL.Uniform4(GL.GetUniformLocation(_id, name), value);
        }

        public void SetVec4(string name, float v1, float v2, float v3, float v4)
        {
            GL.Uniform4(GL.GetUniformLocation(_id, name), v1, v2, v3, v4);
        }

        public void SetMat2(string name, Matrix2 value)
        {
            GL.UniformMatrix2(GL.GetUniformLocation(_id, name), false, ref value);
        }

        public void SetMat3(string name,  Matrix3 value)
        {
            GL.UniformMatrix3(GL.GetUniformLocation(_id, name), false, ref value);
        }

        public void SetMat4(string name, Matrix4 value)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(_id, name), false, ref value);
        }

        // TODO: implement later, not deadly
        public void SetUniform<T>(string name, T value) where T : struct 
        {
            if (value.GetType() == typeof(Vector2))
            {
                //GL.Uniform2(GL.GetUniformLocation(_id, name),value);
            }
        }
    }
}

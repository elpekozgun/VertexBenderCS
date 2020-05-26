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
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\StandardVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\StandardFragment.glsl", ShaderType.FragmentShader)
        );

        private static Shader _defaultUnlit = ShaderBuilder.CreateShader
        (
            "unlit",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\UnlitVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\UnlitFragment.glsl", ShaderType.FragmentShader)
        );

        private static Shader _defaultCuberille = ShaderBuilder.CreateShader
        (
            "cuberille",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\CuberilleVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\CuberilleGeometry.glsl", ShaderType.GeometryShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\CuberilleFragment.glsl", ShaderType.FragmentShader)
        );

        private static Shader _defaultPointCloud = ShaderBuilder.CreateShader
        (
            "pointCloud",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\PointCloudVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\PointCloudFragment.glsl", ShaderType.FragmentShader)
        );

        private static Shader _marchingCubesComputeTribased = ShaderBuilder.CreateShader
        (
            "marchingCubesTri",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\MarchingCubesTriBased.glsl", ShaderType.ComputeShader)
            //ShaderBuilder.CreateShaderSource(@"Resources\Shader\TestCompute.glsl", ShaderType.ComputeShader)
        );

        private static Shader _marchingCubesComputeVertexBased = ShaderBuilder.CreateShader
        (
            "marchingCubesVertex",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\MarchingCubesVertexBased.glsl", ShaderType.ComputeShader)
        //ShaderBuilder.CreateShaderSource(@"Resources\Shader\TestCompute.glsl", ShaderType.ComputeShader)
        );

        private static Shader _intersectionCompute = ShaderBuilder.CreateShader
        (
            "intersection Compute",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\IntersectionCompute.glsl", ShaderType.ComputeShader)
        //ShaderBuilder.CreateShaderSource(@"Resources\Shader\TestCompute.glsl", ShaderType.ComputeShader)
        );


        private static Shader _computePaint = ShaderBuilder.CreateShader
        (
            "ComputePaint",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\ComputeVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\ComputeFragment.glsl", ShaderType.FragmentShader)
        );

        private static Shader _gizmoShader = ShaderBuilder.CreateShader
        (
            "gizmo",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\GizmoVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\GizmoFragment.glsl", ShaderType.FragmentShader)
        );


        public static Shader DefaultShader => _defaultShader;
        public static Shader DefaultUnlitShader=> _defaultUnlit;
        public static Shader DefaultPointCloud => _defaultPointCloud;
        public static Shader DefaultCuberille => _defaultCuberille;
        public static Shader MarchingComputeTriBased => _marchingCubesComputeTribased;
        public static Shader MarchingComputeVertexBased => _marchingCubesComputeVertexBased;
        public static Shader IntersectionCompute => _intersectionCompute;
        public static Shader DefaultComputePaint => _computePaint;
        public static Shader DefaultGizmo => _gizmoShader;
        //public static Shader DefaultIndicator => _defaultIndicator;

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

        public void SetBool(string name, bool value)
        {
            GL.Uniform1(GL.GetUniformLocation(_id, name), value == true ? 1 : 0);
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

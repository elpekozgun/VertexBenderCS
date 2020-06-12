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

        private static readonly Shader _standard = ShaderBuilder.CreateShader
        (
            "shaded",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\StandardVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\StandardFragment.glsl", ShaderType.FragmentShader)
        );

        private static readonly Shader _gouraud = ShaderBuilder.CreateShader
        (
            "gouraud",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\GouraudVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\GouraudFragment.glsl", ShaderType.FragmentShader)
        );
    
        private static readonly Shader _unlit = ShaderBuilder.CreateShader
        (
            "unlit",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\UnlitVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\UnlitFragment.glsl", ShaderType.FragmentShader)
        );

        private static readonly Shader _indicator = ShaderBuilder.CreateShader
        (
        "indicator",
        ShaderBuilder.CreateShaderSource(@"Resources\Shader\indicator.vert", ShaderType.VertexShader),
        ShaderBuilder.CreateShaderSource(@"Resources\Shader\indicator.frag", ShaderType.FragmentShader)
        );

        private static readonly Shader _cuberilleGeometry = ShaderBuilder.CreateShader
        (
            "cuberille",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\CuberilleVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\CuberilleGeometry.glsl", ShaderType.GeometryShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\CuberilleFragment.glsl", ShaderType.FragmentShader)
        );

        private static readonly Shader _pointCloud = ShaderBuilder.CreateShader
        (
            "pointCloud",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\PointCloudVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\PointCloudFragment.glsl", ShaderType.FragmentShader)
        );

        private static readonly Shader _marchingCubesTriCompute = ShaderBuilder.CreateShader
        (
            "marchingCubesTri",
            ShaderBuilder.CreateShaderSource(@"Resources\Compute\MarchingCubesTriBased.glsl", ShaderType.ComputeShader)
        );

        private static readonly Shader _marchingCubesVertexCompute = ShaderBuilder.CreateShader
        (
            "marchingCubesVertex",
            ShaderBuilder.CreateShaderSource(@"Resources\Compute\MarchingCubesVertexBased.glsl", ShaderType.ComputeShader)
        );

        private static readonly Shader _intersectionCompute = ShaderBuilder.CreateShader
        (
            "intersection Compute",
            ShaderBuilder.CreateShaderSource(@"Resources\Compute\IntersectionCompute.glsl", ShaderType.ComputeShader)
        );

        private static readonly Shader _volEditorCompute = ShaderBuilder.CreateShader
        (
            "vol editor Compute",
            ShaderBuilder.CreateShaderSource(@"Resources\Compute\VolEditorCompute.glsl", ShaderType.ComputeShader)
        );

        private static readonly Shader _smoothenCompute = ShaderBuilder.CreateShader
        (
            "Smoothen Compute",
            ShaderBuilder.CreateShaderSource(@"Resources\Compute\SmoothenCompute.glsl", ShaderType.ComputeShader)
        );

        private static readonly Shader _directComputePaint = ShaderBuilder.CreateShader
        (
            "ComputePaint",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\ComputeVertex.glsl", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\ComputeFragment.glsl", ShaderType.FragmentShader)
        );

        private static readonly Shader _EditorSphere = ShaderBuilder.CreateShader
        (
            "Editor Sphere",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\EditorSphere.vert", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\EditorSphere.frag", ShaderType.FragmentShader)
        );

        private static readonly Shader _normalShader = ShaderBuilder.CreateShader
        (
            "normal",
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\normal.vert", ShaderType.VertexShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\normal.geom", ShaderType.GeometryShader),
            ShaderBuilder.CreateShaderSource(@"Resources\Shader\normal.frag", ShaderType.FragmentShader)
        );

        public static Shader Standard => _standard;
        public static Shader Gouraud => _gouraud;
        public static Shader Unlit => _unlit;
        public static Shader PointCloud => _pointCloud;
        public static Shader CuberilleGeometry => _cuberilleGeometry;
        public static Shader MarchingComputeTriCompute => _marchingCubesTriCompute;
        public static Shader MarchingComputeVertexCompute => _marchingCubesVertexCompute;
        public static Shader IntersectionCompute => _intersectionCompute;
        public static Shader VolEditorCompute => _volEditorCompute;
        public static Shader SmoothenCompute => _smoothenCompute;
        public static Shader DirectComputePaint => _directComputePaint;
        public static Shader EditorSphere => _EditorSphere;
        public static Shader NormalShader => _normalShader;
        public static Shader Indicator => _indicator;

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

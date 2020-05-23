﻿using Engine.Core;
using Engine.Processing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Platform.Windows;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Windows.Forms;

namespace Engine.GLApi
{
    public class VolumeRenderer : Transform, IDisposable, IRenderable
    {
        public Shader Shader { get; set; }
        public Shader ComputeShader { get; set; }

        public Vector3 Center { get; private set; }
        public Texture DiffuseTexture { get; set; }

        private GpuVertex[] vertices;
        private VolOutput rawVolData;
        private VolOutput subVolData;
        private Vector4[] input;

        private int _drawCount = 0;
        private bool _isEfficient = true;

        private bool _initialized;

        private int _currentDownSample;

        private int _VAO;
        private int _VBO;
        private int _SSBO_in;
        private int _SSBO_out;
        private int _UBO_mc;
        private int _ACBO;
        private int _EBO;

        private Stopwatch _watch;

        public Vector4 Color { get; set; }

        public VolumeRenderer(VolOutput vol, string name = "")
            : base(name)
        {
            rawVolData = vol;
            subVolData = vol;

            _watch = new Stopwatch();
            DiffuseTexture = Texture.LoadTexture(@"Resources\Image\Blank1024.png", eTextureType.Diffuse);

            Color = new Vector4(0.3f, 0.1f, 0.1f, 1.0f);

            Shader = Shader.DefaultComputePaint;
            ComputeShader = Shader.DefaultMarchingCompute;

            Init();
            UpdateInputFromVol(rawVolData);

            _initialized = true;
        }

        public VolumeRenderer(VolOutput output, Shader shader, Shader computeShader, string name = "") : this(output, name)
        {
            Shader = shader;
            ComputeShader = computeShader;
        }

        private void Init()
        {
            GL.GenBuffers(1, out _UBO_mc);
            GL.GenBuffers(1, out _SSBO_in);
            GL.GenBuffers(1, out _EBO);
            GL.GenBuffers(1, out _SSBO_out);
            GL.GenBuffers(1, out _ACBO);
            GL.GenVertexArrays(1, out _VAO);

            GL.BindBuffer(BufferTarget.UniformBuffer, _UBO_mc);
            GL.BufferData(BufferTarget.UniformBuffer, (256 * 16) * sizeof(float), MarchingCubesTables.TriangleConnectionTableLinear, BufferUsageHint.StaticRead);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, _UBO_mc);

        }

        private void UpdateInputFromVol(VolOutput vol)
        {
            input = new Vector4[vol.IntensityMap.Count];
            int i = 0;
            foreach (var item in vol.IntensityMap)
            {
                input[i] = new Vector4(item.Key, item.Value);
                i++;
            }
        }

        public void Compute(int intensity, int downSample, bool testBottleNeck = false)
        {
            if (testBottleNeck)
            {
                ComputeShader = Shader.DefaultMarchingCompute;
                ComputeBottleNeck(intensity, downSample);
            }
            else
            {
                ComputeShader = Shader.MarchingComputeEfficient;
                ComputeEfficient(intensity, downSample);
            }
        }

        public void ComputeBottleNeck(int intensity, int downSample)
        {
            VolOutput vol = Algorithm.Downsample(rawVolData, downSample);
            UpdateInputFromVol(vol);

            GL.DeleteBuffers(1, ref _UBO_mc);
            GL.DeleteBuffers(1, ref _SSBO_in);
            GL.DeleteBuffers(1, ref _SSBO_out);
            GL.DeleteBuffers(1, ref _EBO);
            GL.DeleteBuffers(1, ref _ACBO);

            GL.GenBuffers(1, out _UBO_mc);
            GL.GenBuffers(1, out _SSBO_in);
            GL.GenBuffers(1, out _EBO);
            GL.GenBuffers(1, out _SSBO_out);
            GL.GenBuffers(1, out _ACBO);

            GL.BindBuffer(BufferTarget.UniformBuffer, _UBO_mc);
            GL.BufferData(BufferTarget.UniformBuffer, (256 * 16) * sizeof(float), MarchingCubesTables.TriangleConnectionTableLinear, BufferUsageHint.StaticRead);
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, _UBO_mc);

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _SSBO_in);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, input.Length * 4 * sizeof(float), input, BufferUsageHint.StaticRead);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 2, _SSBO_in);

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _SSBO_out);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, input.Length * 3 * 4 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, _SSBO_out);

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _EBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, input.Length * 4 * sizeof(float) , IntPtr.Zero, BufferUsageHint.DynamicCopy);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 4, _EBO);

            GL.BindBuffer(BufferTarget.AtomicCounterBuffer, _ACBO);
            GL.BufferData(BufferTarget.AtomicCounterBuffer, sizeof(uint) * 4, IntPtr.Zero, BufferUsageHint.DynamicRead);
            GL.BindBufferBase(BufferRangeTarget.AtomicCounterBuffer, 5, _ACBO);

            
            int dimx = vol.XCount;
            int dimy = vol.YCount;
            int dimz = vol.ZCount;

            ComputeShader.Use();
            ComputeShader.SetInt("xCount", dimx);
            ComputeShader.SetInt("yCount", dimy);
            ComputeShader.SetInt("zCount", dimz);
            ComputeShader.SetInt("intensity", intensity);
            ComputeShader.SetInt("counter", 0);
            ComputeShader.SetInt("aa", 0);
            ComputeShader.SetInt("bb", 0);
            ComputeShader.SetInt("cc", 0);

            var a = dimx + (8 - dimx % 8);
            var b = dimy + (8 - dimy % 8);
            var c = dimz + (8 - dimz % 8);



            GL.DispatchCompute(a / 8, b / 8, c / 8);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderStorageBarrierBit | MemoryBarrierFlags.AtomicCounterBarrierBit);

            uint[] counter = new uint[4];


            GL.GetBufferSubData
            (
                BufferTarget.AtomicCounterBuffer,
                IntPtr.Zero,
                sizeof(uint),
                ref counter[0]
            );

            GL.BindBuffer(BufferTarget.AtomicCounterBuffer, _ACBO);
            GL.GetBufferSubData
            (
                BufferTarget.AtomicCounterBuffer,
                IntPtr.Add(IntPtr.Zero, 4),
                sizeof(uint),
                ref counter[1]
            );

            GL.GetBufferSubData
            (
                BufferTarget.AtomicCounterBuffer,
                IntPtr.Add(IntPtr.Zero, 8),
                sizeof(uint),
                ref counter[2]
            );

            GL.GetBufferSubData
            (
                BufferTarget.AtomicCounterBuffer,
                IntPtr.Add(IntPtr.Zero, 12),
                sizeof(uint),
                ref counter[3]
            );

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _SSBO_out);
            ComputeTriangle[] tris = new ComputeTriangle[counter[0]];
            GL.GetBufferSubData
            (
                BufferTarget.ShaderStorageBuffer,
                IntPtr.Zero,
                tris.Length * 3 * 4 * sizeof(float),
                tris
            );

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _EBO);
            Vector4[] ids = new Vector4[counter[0]];
            GL.GetBufferSubData
            (
                BufferTarget.ShaderStorageBuffer,
                IntPtr.Zero,//IntPtr.Add(IntPtr.Zero, tris.Length * 3 * 4 * sizeof(float)),
                ids.Length * sizeof(float),
                ids
            );

            var asd = ids.Where(x => x.X != x.Y && x.X != x.Z && x.Y != x.Z).ToArray();

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
            GL.BindBuffer(BufferTarget.AtomicCounterBuffer, 0);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            // This part i think can be processed directly in gpu from compute to vertex shader with ssbo?
            vertices = new GpuVertex[tris.Length * 3];
            for (int i = 0; i < tris.Length; i++)
            {
                Vector3 normal = new Vector3(tris[i].v0.W, tris[i].v1.W, tris[i].v2.W).Normalized();

                vertices[3 * i] = new GpuVertex()
                {
                    Coord = tris[i].v0.Xyz * -vol.Spacing,
                    Normal = normal,
                    Color = new Vector3(0.0f, 0.0f, 0.0f)
                };

                vertices[3 * i + 1] = new GpuVertex()
                {
                    Coord = tris[i].v1.Xyz * -vol.Spacing,
                    Normal = normal,
                    Color = new Vector3(0.0f, 0.0f, 0.0f)
                };

                vertices[3 * i + 2] = new GpuVertex()
                {
                    Coord = tris[i].v2.Xyz * -vol.Spacing,
                    Normal = normal,
                    Color = new Vector3(0.0f, 0.0f, 0.0f)
                };
            }

            _drawCount = vertices.Length;

            Setup();
        }

        public void ComputeEfficient(int intensity, int downSample)
        {
            if (downSample != _currentDownSample)
            {
                subVolData = Algorithm.Downsample(rawVolData, downSample);
                UpdateInputFromVol(subVolData);
                _currentDownSample = downSample;
            }


            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _SSBO_in);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, input.Length * 4 * sizeof(float), input, BufferUsageHint.DynamicCopy);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 2, _SSBO_in);

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _SSBO_out);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, input.Length * 6 * 4 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, _SSBO_out);

            uint counter = 0;
            GL.BindBuffer(BufferTarget.AtomicCounterBuffer, _ACBO);
            GL.BufferData(BufferTarget.AtomicCounterBuffer, sizeof(uint), ref counter, BufferUsageHint.DynamicRead);
            GL.BindBufferBase(BufferRangeTarget.AtomicCounterBuffer, 5, _ACBO);

            int dimx = subVolData.XCount;
            int dimy = subVolData.YCount;
            int dimz = subVolData.ZCount;

            ComputeShader.Use();
            ComputeShader.SetInt("xCount", dimx);
            ComputeShader.SetInt("yCount", dimy);
            ComputeShader.SetInt("zCount", dimz);
            ComputeShader.SetInt("intensity", intensity);
            ComputeShader.SetInt("counter", 0);
            ComputeShader.SetFloat("spacing", subVolData.Spacing);

            var a = dimx + (8 - dimx % 8);
            var b = dimy + (8 - dimy % 8);
            var c = dimz + (8 - dimz % 8);

            GL.DispatchCompute(a / 8, b / 8, c / 8);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);

            GL.BindBuffer(BufferTarget.AtomicCounterBuffer, _ACBO);
            GL.GetBufferSubData
            (
                BufferTarget.AtomicCounterBuffer,
                IntPtr.Zero,
                sizeof(uint),
                ref counter
            );

            _drawCount = (int)counter * 3;

            GL.BindVertexArray(_VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _SSBO_out);

            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 16);
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);
            Logger.Log(counter.ToString());
        }


        private void Setup()
        {
            GL.DeleteBuffers(1, ref _VAO);
            GL.DeleteBuffers(1, ref _VBO);

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

            if (_isEfficient)
            {
                Shader = Shader.DefaultComputePaint;
            }
            else
            {
                Shader = Shader.DefaultShader;
            }

            Shader.Use();
            Shader.SetInt("material.diffuse", (int)TextureUnit.Texture0);
            Shader.SetMat4("Model", ModelMatrix);
            Shader.SetMat4("View", cam.View);
            Shader.SetMat4("Projection", cam.Projection);
            Shader.SetVec4("Color", Color);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PointSize(2);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _drawCount);

            GL.Enable(EnableCap.CullFace);

            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_initialized)
                {
                    GL.DeleteVertexArray(_VAO);
                    GL.DeleteBuffer(_VBO);
                    GL.DeleteBuffer(_SSBO_in);
                    GL.DeleteBuffer(_SSBO_out);
                    GL.DeleteBuffer(_UBO_mc);
                    GL.DeleteBuffer(_ACBO);
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

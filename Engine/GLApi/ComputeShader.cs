using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace Engine.GLApi
{
    public struct ComputeTriangle
    {
        public Vector4 v0;
        public Vector4 v1;
        public Vector4 v2;
    };

    public enum BufferUsage
    { 
        source = 1,
        target = 2,
        counter = 3
    }

    public abstract class ComputeBuffer
    {
        protected abstract void Bind(int size, BufferUsageHint hint, BufferUsage usage);
        public abstract int Id { get; }
        public abstract void Release();
    }

    public class ComputeBuffer<T> : ComputeBuffer where T : struct
    {
        public T[] Data;
        public int _id;
        public BufferUsage Usage;

        public ComputeBuffer(T[] data, int size, BufferUsageHint hint, BufferUsage usage)
        {
            Data = data;
            _id = -1;
            Bind(size, hint, usage);
        }

        public override int Id => _id;

        public override void Release()
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        protected override void Bind(int size, BufferUsageHint hint, BufferUsage usage)
        {
            GL.GenBuffers(1, out _id);
            if (usage == BufferUsage.counter)
            {
                var v = 0u;
                GL.BindBuffer(BufferTarget.AtomicCounterBuffer, _id);
                GL.BufferData(BufferTarget.AtomicCounterBuffer, sizeof(uint), ref v, hint);
                GL.BindBufferBase(BufferRangeTarget.AtomicCounterBuffer, (int)usage, _id);
            }

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _id);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, Data.Length * size, Data, hint);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, (int)usage, _id);
            //GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        
    }


    public class ComputeShader
    {
        private int _id;

        private List<ComputeBuffer> _computeBuffers;

        public ComputeShader(string name, int id)
        {
            _id = id;
            _computeBuffers = new List<ComputeBuffer>();
        }

        public T[] GetBufferData<T>(ComputeBuffer<T> buffer) where T : struct
        {
            return buffer.Data;
        }

        public void Use()
        {
            GL.UseProgram(_id);
        }

        public void Dispatch(int xGroup, int yGroup, int zGroup)
        {
            GL.UseProgram(_id);

            GL.DispatchCompute(xGroup, yGroup, zGroup);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderStorageBarrierBit);

            var counter = _computeBuffers[1];

            var asdsad = (ComputeBuffer<ComputeTriangle>)counter;
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, counter.Id);
            GL.GetBufferSubData(BufferTarget.ShaderStorageBuffer,IntPtr.Zero, sizeof(uint),asdsad.Data);
        }

        public void AddBuffer<T>(ComputeBuffer<T> buffer) where T :struct
        {
            _computeBuffers.Add(buffer);
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

        public void SetMat3(string name, Matrix3 value)
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

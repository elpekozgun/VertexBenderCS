﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Linq;

namespace Engine.GLApi
{
    public static class ShaderBuilder
    {
        public static string SHADERPATH = @"Resources\Shader\";

        public static int CreateShaderSource(string path, ShaderType type)
        {
            var id = GL.CreateShader(type);
            var fileInput = File.ReadAllLines(path);

            for (int i = 0; i < fileInput.Length; i++)
            {
                if (fileInput[i].Contains("#include"))
                {
                    var includeFile = fileInput[i].Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries).Last();
                    fileInput[i] = File.ReadAllText(SHADERPATH + includeFile);
                }
            }
            var source = string.Join("\n", fileInput);

            GL.ShaderSource(id, source);
            GL.CompileShader(id);
            CheckCompileError(id, "");
            return id;
        }

        public static Shader CreateShader(string name, params int[] shaders)
        {
            int id = GL.CreateProgram();
            for (int i = 0; i < shaders.Length; i++)
            {
                GL.AttachShader(id, shaders[i]);
            }
            GL.LinkProgram(id);

            for (int i = 0; i < shaders.Length; i++)
            {
                GL.DeleteShader(shaders[i]);
            }

            return new Shader(name, id);

        }
        private static void CheckCompileError(int id, string type)
        {
            int success;
            char[] infolog = new char[1024];
            if (type != "PROGRAM" || type != "program")
            {
                //Logger.Log(GL.GetShaderInfoLog(id));
                //glGetShaderiv(id, GL_COMPILE_STATUS, &success);
                //if (!success)
                //{
                //    glGetShaderInfoLog(id, sizeof(infolog), NULL, infolog);
                //    //TITAN_LOG(type << " Compilation Error: " << infolog << "\n" << std::endl);
                //}
            }
            else
            {
                //glGetShaderiv(id, GL_COMPILE_STATUS, &success);
                //if (!success)
                //{
                //Logger.Log(GL.GetShaderInfoLog(id));
                //glGetShaderInfoLog(id, sizeof(infolog), NULL, infolog);
                //TITAN_LOG(type << "Linking Error: " << infolog << "\n" << std::endl);
                //}
            }
        }

    }
}

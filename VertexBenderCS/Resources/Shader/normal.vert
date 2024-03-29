﻿#version 450

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out VS_OUT {
    vec3 normal;
} vs_out;

uniform mat4 View;
uniform mat4 Model;

void main()
{
    gl_Position = View * Model * vec4(aPos, 1.0); 
    mat3 normalMatrix = mat3(transpose(inverse(View * Model)));
    vs_out.normal = normalize(vec3(vec4(normalMatrix * aNormal, 0.0)));
}
#version 420

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inColor;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

out VS_OUT
{
	vec3 Color;
} vs_out;

void main()
{
	gl_Position = Projection * View * Model * vec4(inPos,1.0f);
	vs_out.Color = inColor;
}

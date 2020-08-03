#version 420

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec4 inColor;



out VS_OUT
{
	vec3 Color;
} vs_out;

void main()
{
	gl_Position = vec4(inPos,1.0f);
	vs_out.Color = inColor;
}

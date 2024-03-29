#version 420

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec4 inColor;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform float Alpha;

out vec4 OutColor;

//out vec4 OutColor;

void main()
{
	gl_Position = Projection * View * Model * vec4(inPos,1.0f);
	//OutColor = Color;
	OutColor = vec4(inColor.xyz,Alpha);
}

#version 430

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;


void main()
{
	gl_Position = Projection * View * Model * vec4(inPos, 1);
}

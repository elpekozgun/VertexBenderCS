#version 420

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;

out vec3 FragmentPosition;
out vec3 FragmentNormal;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

void main()
{
	gl_Position = Projection * View * Model * vec4(inPos,1.0f);
	FragmentPosition = (Model * vec4(inPos, 1.0f)).xyz;
	FragmentNormal = mat3(transpose(inverse(Model))) * inNormal;
}
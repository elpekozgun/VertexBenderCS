#version 420

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inColor;
layout (location = 3) in vec2 inTexCoord;

out vec2 TexCoord;
out vec3 FragmentPosition;
out vec3 FragmentNormal;
out vec3 FragmentColor;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

void main()
{
	gl_Position = Projection * View * Model * vec4(inPos,1.0f);
	FragmentPosition = (Model * vec4(inPos, 1.0f)).xyz;
	FragmentNormal = mat3(transpose(inverse(Model))) * inNormal;
	FragmentColor = vec3(inColor);
	TexCoord = inTexCoord;
}
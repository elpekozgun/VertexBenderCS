#version 420

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inColor;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

//uniform float MaxIntensity;
//uniform float MinIntensity;

out vec3 OutColor;

//out vec4 OutColor;

void main()
{
	OutColor = inColor;

//	if(OutColor.x >= MinIntensity && OutColor.x <= MaxIntensity)
//	{
//	}
	gl_Position = Projection * View * Model * vec4(inPos,1.0f);

	//OutColor = Color;
}

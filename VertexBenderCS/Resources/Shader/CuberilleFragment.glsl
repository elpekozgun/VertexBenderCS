#version 420 core

out vec4 FragColor;
in vec3 OutColor;

uniform float Intensity;

void main()
{
	if(OutColor.x < Intensity)
	{
		discard;
	}
	FragColor = vec4(OutColor,1.0f); 
}

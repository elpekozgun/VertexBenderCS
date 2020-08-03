#version 420 core

out vec4 FragColor;
in vec4 OutColor;

uniform float Intensity;

void main()
{
	if(OutColor.r < Intensity)
	{
		discard;
	}
	FragColor = vec4(OutColor.xyz,1.0f); 
}

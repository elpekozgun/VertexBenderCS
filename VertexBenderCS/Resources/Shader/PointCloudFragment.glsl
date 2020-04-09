#version 420 core

out vec4 FragColor;
in vec3 OutColor;
//in vec4 OutColor;

uniform float MaxIntensity;
uniform float MinIntensity;

void main()
{
	if(OutColor.x < MinIntensity || OutColor.x > MaxIntensity)
	{
		discard;
	}
	FragColor = vec4(OutColor,1.0f); 

}

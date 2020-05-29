#version 420 core

out vec4 FragColor;
uniform vec4 Color;
in vec4 OutColor;
//in vec4 OutColor;

void main()
{
	if(Color.length() > 0)
	{
		FragColor = vec4(OutColor); 
	}
	else
	{
		FragColor = Color;
	}

}

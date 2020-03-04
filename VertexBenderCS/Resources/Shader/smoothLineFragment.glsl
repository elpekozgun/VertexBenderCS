#version 450 core

//uniform vec4  Color;
//uniform float LineWidth;
//uniform float BlendFactor; //1.5..2.5

in vec4 Color;
in float LineWidth;
in float BlendFactor;

in vec2 LineCenter;
out vec4 FragColor;

void main(void)
{
		FragColor = Color;

      vec4 col = Color;        
      double d = length(LineCenter - gl_FragCoord.xy);
      double w = LineWidth;
      if (d>w)
        col.w = 0;
      else
        col.w *= pow(float((w-d)/w), BlendFactor);
      FragColor = col;
};

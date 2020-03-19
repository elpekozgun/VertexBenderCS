#version 450 core

layout (location = 0) in vec3 aPos;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

uniform vec4  Color;
uniform vec2 ViewPort; 
uniform float LineWidth;
uniform float BlendFactor; //1.5..2.5

out vec2 LineCenter;
out vec4 oColor;
out float oLineWidth;
out float oBlendFactor;

void main(void)
{
	oBlendFactor = BlendFactor;
	oLineWidth= LineWidth;
	oColor = Color;

	vec4 pp = Projection * View * Model * vec4(aPos.x, aPos.y, aPos.z, 0.0f);
	gl_Position = pp;
	vec2 vp = ViewPort;
	LineCenter = 0.5 * (pp.xy + vec2(1, 1)) * vp;
};
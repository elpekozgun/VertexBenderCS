#version 430

layout(location = 0) in vec3 pos;
layout(location = 1) in vec2 texCoord;

uniform vec2 center;
uniform float aspect;
uniform float radius;

out vec2 outCenter;
out vec2 outTexCoord;
out float outRadius;

void main()
{
	gl_Position = vec4(radius * pos.x + center.x, radius * pos.y * aspect + center.y, pos.z, 1.0);
	outCenter = center;
	outTexCoord = texCoord;
	outRadius = radius;
}

#version 430

uniform float border;
uniform vec4 color;

in vec2 outCenter;
in vec2 outTexCoord;
in float outRadius;

void main()
{
	float diff = length(outTexCoord - vec2(0.5, 0.5)) * 2 * outRadius;

	if( diff > outRadius || diff < (1 - border) * outRadius)
		discard;

	gl_FragColor = vec4(color.xyz, 0.2f);

}



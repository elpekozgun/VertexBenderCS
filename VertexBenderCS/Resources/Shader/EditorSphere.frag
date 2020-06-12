#version 430

in vec3 fragNormal;
in vec3 fragPos;
uniform vec3 cameraPosition;

void main()
{
	float dotVal = dot(normalize(fragNormal), normalize(cameraPosition - fragPos));

	if(dotVal > 0.99f)
	{
		gl_FragColor = vec4(0,1,0,1);
	}
//	else if(dotVal < 0.4f)
	else
	{
		//gl_FragColor = (1 - dotVal) * vec4(0,1,0,1);
		gl_FragColor = smoothstep(0.2, 0.8, 1 - dotVal) * vec4(0,1,0,1);
	}

}
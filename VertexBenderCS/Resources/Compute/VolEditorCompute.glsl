#version 430

layout(std430, binding = 2) buffer pointBuffer_in
{
	vec4 pointCloud[];
};

//layout(binding = 4) uniform atomic_uint counter;

layout(local_size_x = 512 , local_size_y = 1, local_size_z = 1) in;

uniform vec3 point;
uniform float radius;
uniform float spacing;
uniform float pressure;
uniform int direction;

uniform mat4 Model;

void main()
{
	uint a = gl_GlobalInvocationID.x;

	vec4 pos = Model * pointCloud[a] * -spacing;

	float dif = length(pos.xyz - point );

	if(dif < radius)
	{
		pointCloud[a].w += pressure * direction;
	}
}

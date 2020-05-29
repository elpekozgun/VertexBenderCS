#version 430	

layout(std430, binding = 3) buffer triangleBuffer_out
{
	vec4[] vertices;
};

layout(std430, binding = 6) buffer triangleBuffer_out2
{
	vec4[] verticesOut;
};

layout(binding = 4) uniform atomic_uint counter;

layout(local_size_x = 1024 , local_size_y = 1, local_size_z = 1) in;

uniform float radius;
uniform int count;

void main()
{
	int it = 1;
	while(it-- > 0)
	{
		//uint i = atomicCounterIncrement(counter);
		uint i = gl_GlobalInvocationID.x;
		vec3 coord = vertices[2 * i].xyz;
		vec3 normal = vertices[2 * i + 1].xyz;
		vec3 lu;


		int k = 0;
		for(int j = 0; j < count; j++)
		{
			float dif = length(vertices[2 * j].xyz - coord);
			if(dif < radius)
			{
				if(j != i)
				{
					lu += vertices[2 * j].xyz; 
					normal += vertices[2 * j + 1].xyz;
					k++;
				}
				//if (k == 6)
//					break;
			}
		}
		if(k == 0)
		{
			verticesOut[2 * i] = vertices[2 * i];
			verticesOut[2 * i + 1] = vertices[2 * i + 1];
		}

		lu /= k;
		lu -= coord;
	
		verticesOut[2 * i] = vec4(vertices[2 * i].xyz + 0.5f * lu, 31);
		verticesOut[2 * i + 1] = vec4(normalize(normal), 69);
	}
}

#version 430

layout(std430, binding = 3) buffer triangleBuffer_out
{
	vec4[] vertices;
};

layout(std430, binding = 5) buffer intersect_buffer
{
	float t;
};


layout(local_size_x = 512) in;

uniform vec3 origin;
uniform vec3 direction;
uniform mat4 Model;

float IntersectPoint(vec3 a, vec3 b, vec3 c,  vec3 o, vec3 d)
{
	// OpenGL matrices are column major, if this fails test on another one. 
	d = normalize(d);

	mat3 A;

	A[0] = vec3(a.x - b.x, a.x - c.x, d.x);
	A[1] = vec3(a.y - b.y, a.y - c.y, d.y);
	A[2] = vec3(a.z - b.z, a.z - c.z, d.z);

	float det = determinant(A);
	float invDet = 1 / det;

	mat3 Beta;

	Beta[0] = vec3(a.x - o.x, a.x - c.x, d.x);
	Beta[1] = vec3(a.y - o.y, a.y - c.y, d.y);
	Beta[2] = vec3(a.z - o.z, a.z - c.z, d.z);

	float beta = determinant(Beta) * invDet;

	if(beta < 0.001f || beta > 0.999f)
	{
		return -1;
	}

	mat3 Gamma;
	
	Gamma[0] = vec3(a.x - b.x, a.x - o.x, d.x);
	Gamma[1] = vec3(a.y - b.y, a.y - o.y, d.y);
	Gamma[2] = vec3(a.z - b.z, a.z - o.z, d.z);

	float gamma = determinant(Gamma) * invDet;
	
	if( gamma < 0.001f || gamma > 0.999f)
	{
		return -1;
	}

	mat3 TMat;

	TMat[0] = vec3(a.x - b.x, a.x - c.x, a.x - o.x);
	TMat[1] = vec3(a.y - b.y, a.y - c.y, a.y - o.y);
	TMat[2] = vec3(a.z - b.z, a.z - c.z, a.z - o.z);

	float t = determinant(TMat) * invDet;

	if(beta + gamma < 1)
	{
		return t;
	}
	return -1;
}


void main()
{
	uint a = gl_GlobalInvocationID.x;
	{
		vec4 v0 =  vertices[a * 6 ];
		vec4 v1 =  vertices[a * 6 + 2];
		vec4 v2 =  vertices[a * 6 + 4];

		float temp = IntersectPoint(v0.xyz, v1.xyz, v2.xyz, origin, direction);
		if(length(temp) != 0.0 && temp != -1)
		{
			if(t == 0)
			{
				t = temp;
			}
			else
			{
				t = min(t, temp);	
			}
		}
	}

}


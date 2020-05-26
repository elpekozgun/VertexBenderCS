#version 430

layout(std430, binding = 2) buffer intersect_buffer
{
	vec4 location;
};

layout(std430, binding = 3) buffer triangleBuffer_out
{
	vec4[] vertices;
};

layout(binding = 5) uniform atomic_uint counter;

layout(local_size_x = 128) in;

uniform vec3 origin;
uniform vec3 direction;

vec4 IntersectPoint(vec4 a, vec4 b, vec4 c,  vec4 o, vec4 d)
{
	// OpenGL matrices are column major, if this fails test on another one. 

	d = normalize(d);

	mat3 A;

//	A[0] = vec3(a.x - b.x, a.y - b.y, a.z - b.z);
//	A[1] = vec3(a.x - c.x, a.y - c.y, a.z - c.z);
//	A[2] = vec3(d.x, d.y, d.z);

	A[0] = vec3(a.x - b.x, a.x - c.x, d.x);
	A[1] = vec3(a.y - b.y, a.y - c.y, d.y);
	A[2] = vec3(a.z - b.z, a.z - c.z, d.z);

	float det = determinant(A);
	float invDet = 1 / det;

	mat3 Beta;

//	Beta[0] = vec3(a.x - o.x, a.y - o.y, a.z - o.z);
//	Beta[1] = vec3(a.x - c.x, a.y - c.y, a.z - c.z);
//	Beta[2] = vec3(d.x, d.y, d.z);

	Beta[0] = vec3(a.x - o.x, a.x - c.x, d.x);
	Beta[1] = vec3(a.y - o.y, a.y - c.y, d.y);
	Beta[2] = vec3(a.z - o.z, a.z - c.z, d.z);

	float beta = determinant(Beta) * invDet;

	if( beta < -0.00001f || beta > 0.9999999f)
		return vec4(0,0,0,0);

	mat3 Gamma;
//
//	Gamma[0] = vec3(a.x - b.x, a.y - b.y, a.z - b.z);
//	Gamma[1] = vec3(a.x - o.x, a.y - o.y, a.z - o.z);
//	Gamma[2] = vec3(d.x, d.y, d.z);
//	
	Gamma[0] = vec3(a.x - b.x, a.x - o.x, d.x);
	Gamma[1] = vec3(a.y - b.y, a.y - o.y, d.y);
	Gamma[2] = vec3(a.z - b.z, a.z - o.z, d.z);

	float gamma = determinant(Gamma) * invDet;
	
	if( gamma < -0.00001f || gamma > 0.9999999f)
		return vec4(0,0,0,0);

	mat3 TMat;

//	TMat[0] = vec3(a.x - b.x, a.y - b.y, a.z - b.z);
//	TMat[1] = vec3(a.x - c.x, a.y - c.y, a.z - c.z);
//	TMat[2] = vec3(a.x - o.x, a.y - o.y, a.z - o.z);

	TMat[0] = vec3(a.x - b.x, a.x - c.x, a.x - o.x);
	TMat[1] = vec3(a.y - b.y, a.y - c.y, a.y - o.y);
	TMat[2] = vec3(a.z - b.z, a.z - c.z, a.z - o.z);

	float t = determinant(TMat) * invDet;

	if(beta + gamma < 1)
		return o + t * d;

}


void main()
{
	uint a = atomicCounterIncrement(counter);

	//if(gl_GlobalInvocationID.x % 6 == 0)
	{
		//uint a = gl_GlobalInvocationID.x;

		vec4 v0 = vertices[a * 6];
		vec4 v1 = vertices[a * 6 + 2];
		vec4 v2 = vertices[a * 6 + 4];


//		float t = intersect(v0.xyz, v1.xyz, v2.xyz, origin, direction);
//		if(t > 0)
//		{
//			location = vec4(origin - t * direction,t);
//		}
		
		vec4 temp = IntersectPoint(v0, v1, v2, vec4(origin,0), vec4(direction,0));
		if(length(temp) != 0.0 && length(location) == 0.0)
		{
			location = vec4(temp.xyz, a);
		}
	}

}


#version 430 core
//#extension GL_ARB_compute_shader: enable
//#extension GL_ARB_shader_storage_buffer_object: enable
//#extension ARB_arrays_of_arrays: enable

// align 16bytes
struct Triangle
{
	vec4 a;
	vec4 b;
	vec4 c;
};

 int cornerIndexEdgeV0[12] = 
    {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        0,
        1,
        2,
        3
    };

    int cornerIndexEdgeV1[12] = 
    {
        1,
        2,
        3,
        0,
        5,
        6,
        7,
        4,
        4,
        5,
        6,
        7
    };

uniform int downSample;
uniform int xCount;
uniform int yCount;
uniform int zCount;
uniform int intensity;


// Layouts
layout(local_size_x = 8, local_size_y = 8, local_size_z = 8) in;

layout (std430, binding = 3) buffer marchingCubes
{
    int[4096] triangulations;
};

layout(std430, binding = 4) buffer pointBuffer_in
{
	vec4 pointCloud[];
};

layout(std430, binding = 5) buffer triangleBuffer_in
{
	Triangle triangles[];
};

layout(std430, binding = 5) buffer test_in
{
	vec4 test[];
};

layout(binding = 6) uniform atomic_uint counter;

int indexFromCoord(int x, int y, int z)
{
	return z * xCount * yCount + y * xCount + x;
};

vec3 interpolate(vec4 v1, vec4 v2)
{
	float t = (intensity - v1.w) / (v2.w - v1.w);
	return v1.xyz + t * (v2.xyz - v1.xyz);
};

void main()
{
	if(gl_GlobalInvocationID.x >= xCount - 1 || gl_GlobalInvocationID.y >= yCount - 1 || gl_GlobalInvocationID.z >= zCount - 1)
    {
	    return;	
    };
	    
	ivec3 id = ivec3(gl_GlobalInvocationID.x, gl_GlobalInvocationID.y, gl_GlobalInvocationID.z);
    
    int idd = indexFromCoord(id.x, id.y, id.z);
	
    vec4 corners[8] = 
	{
	    pointCloud[indexFromCoord(id.x, id.y, id.z)],
        pointCloud[indexFromCoord(id.x + 1, id.y, id.z)],
        pointCloud[indexFromCoord(id.x + 1, id.y, id.z + 1)],
        pointCloud[indexFromCoord(id.x, id.y, id.z + 1)],
        pointCloud[indexFromCoord(id.x, id.y + 1, id.z)],
        pointCloud[indexFromCoord(id.x + 1, id.y + 1, id.z)],
        pointCloud[indexFromCoord(id.x + 1, id.y + 1, id.z + 1)],
        pointCloud[indexFromCoord(id.x, id.y + 1, id.z + 1)]
	};

    uint cubeIndex = 0;
    if (corners[0].w >= intensity) cubeIndex |= 1;
    if (corners[1].w >= intensity) cubeIndex |= 2;
    if (corners[2].w >= intensity) cubeIndex |= 4;
    if (corners[3].w >= intensity) cubeIndex |= 8;
    if (corners[4].w >= intensity) cubeIndex |= 16;
    if (corners[5].w >= intensity) cubeIndex |= 32;
    if (corners[6].w >= intensity) cubeIndex |= 64;
    if (corners[7].w >= intensity) cubeIndex |= 128;

    for(int i = 0; i < 16; i+=3) 
    {
        if( triangulations[cubeIndex * 16 + i] == -1)
        {
            break;
        }
        int a0 = cornerIndexEdgeV0[triangulations[cubeIndex * 16 + i]];
        int b0 = cornerIndexEdgeV1[triangulations[cubeIndex * 16 + i]];
        
        int a1 = cornerIndexEdgeV0[triangulations[cubeIndex * 16 + i + 1]];
        int b1 = cornerIndexEdgeV1[triangulations[cubeIndex * 16 + i + 1]];
        
        int a2 = cornerIndexEdgeV0[triangulations[cubeIndex * 16 + i + 2]];
        int b2 = cornerIndexEdgeV1[triangulations[cubeIndex * 16 + i + 2]];

        Triangle tri;
        tri.c= vec4(interpolate(corners[a0], corners[b0]), 0);
        tri.b= vec4(interpolate(corners[a1], corners[b1]), 0);
        tri.a= vec4(interpolate(corners[a2], corners[b2]), 0);

        triangles[atomicCounterIncrement(counter)] = tri;
        
    };

};
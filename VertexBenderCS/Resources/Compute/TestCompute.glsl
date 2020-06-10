#version 430 compatibility
//#extension GL_ARB_compute_shader: enable
//#extension GL_ARB_shader_storage_buffer_object: enable

#include "..\Shader\MarchingCubeTable.glsl"

// align 16bytes
struct Triangle
{
	vec4 a;
	vec4 b;
	vec4 c;
};

uniform int xCount;
uniform int yCount;
uniform int zCount;

// Layouts
layout(local_size_x = 8, local_size_y = 8, local_size_z = 8) in;

layout(std430, binding = 4) buffer pointBuffer_in
{
	vec4 pointCloud[];
};

layout(std430, binding = 5) buffer triangleBuffer_in
{
	vec4 pointCloudOut[];
};

layout(binding = 6) uniform atomic_uint counter;

int indexFromCoord(ivec3 coord)
{
	return coord.z * xCount * yCount + coord.y * xCount + coord.x;
};

void main()
{
//	if(gl_GlobalInvocationID.x >= xCount - 1 || gl_GlobalInvocationID.y >= yCount - 1 || gl_GlobalInvocationID.z >= zCount - 1)
//    {
//	    return;	
//    };

	ivec3 id = ivec3(gl_GlobalInvocationID.x, gl_GlobalInvocationID.y, gl_GlobalInvocationID.z);
 
	uint a = atomicCounterIncrement(counter);
	int idd = indexFromCoord(id);

    pointCloudOut[idd] = vec4(id.x, id.y, id.z, a); 

};
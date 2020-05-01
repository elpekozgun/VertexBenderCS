#version 450

uvec3 gl_NumWorkGroups;
uvec3 gl_WorkGroupSize;
uvec3 gl_WorkGroupID;
uvec3 gl_LocalInvocationID;
uvec3 gl_GlobalInvocationID;
uvec3 gl_LocalInvocationIndex;

layout(local_size_x = 1, local_size_y = 1, local_size_z = 1) in;
layout(rgba32f, binding = 0) uniform image3D img_output;


void main()
{
	vec4 pixel = vec4(0,0,0,1);
	ivec3 pixel_coords = ivec3(gl_GlobalInvocationID.xyz);
	
	imageStore(img_output, pixel_coords, pixel);
}

#version 430


layout(std430, binding = 5) buffer Triangles
{
	vec4 a;
	vec4 b;
	vec4 c;
} triangles;

out vec3 FragmentPosition;
out vec3 FragmentNormal;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

void main()
{

	gl_Position = Projection * View * Model * triangles.a;
	
	FragmentPosition = (Model * triangles.a).xyz;
	FragmentNormal = mat3(transpose(inverse(Model))) * vec3(triangles.a.w, triangles.b.w, triangles.c.w);
};

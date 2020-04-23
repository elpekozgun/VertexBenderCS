#version 330 core

layout (points) in;
layout (points, max_vertices = 10) out;

in VS_OUT
{
	vec3 Color;
} gs_in[];

out vec3 OutColor;
//uniform float Spacing;

void MakeCube(vec4 position, float spacing)
{
	float s = spacing * 0.5f;

	gl_Position = position + vec4( -s, s, -s , 0);
	EmitVertex();

	gl_Position = position + vec4( -s, s, s , 0);
	EmitVertex();

	gl_Position = position + vec4( s, s, s , 0);
	EmitVertex();

	gl_Position = position + vec4( s, s, -s , 0);
	EmitVertex();

//	gl_Position = position + vec4( -s, s, -s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, s, -s , 0);
//	EmitVertex();
//	
//	gl_Position = position + vec4( -s, -s, -s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, -s, -s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, -s, s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, s, -s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, s, s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( -s, s, -s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( -s, s, s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( -s, -s, -s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( -s, -s, s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, -s, s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4(-s, s, s , 0);
//	EmitVertex();
//
//	gl_Position = position + vec4( s, s, s , 0);
//	EmitVertex();
//
	EndPrimitive();
}


void main()
{
	OutColor = gs_in[0].Color;
	//gl_Position = gl_in[0].gl_Position;
	//MakeCube(gl_in[0].gl_Position, gs_in[0].Spacing);	
	float Spacing = 0.1f;
	
	//gl_Position = gl_in[0].gl_Position;
	gl_Position = gl_in[0].gl_Position + vec4( Spacing, 0, 0, 0);
	EmitVertex();

//	gl_Position = gl_in[0].gl_Position + vec4( -Spacing, 0, 0 , 0);
//	EmitVertex();
//
//	gl_Position = gl_in[0].gl_Position + vec4( 0, Spacing, 0 , 0);
//	EmitVertex();
//
//	gl_Position = gl_in[0].gl_Position + vec4( 0, -Spacing, 0 , 0);
//	EmitVertex();

	EndPrimitive();

}
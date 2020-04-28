#version 450

layout (points) in;
layout (triangle_strip, max_vertices = 18) out;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;	

in VS_OUT
{
	vec3 Color;
} gs_in[];

out vec3 OutColor;
uniform float Spacing;

void MakeCube(mat4 mvp, vec4 position)
{
	float s = Spacing * 0.5f;

	gl_Position = mvp * (position + vec4( -s, s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( -s, s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( -s, s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, s, -s , 0));
	EmitVertex();
	
	gl_Position = mvp * (position + vec4( -s, -s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, -s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, -s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( -s, s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( -s, s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( -s, -s, -s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( -s, -s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, -s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4(-s, s, s , 0));
	EmitVertex();

	gl_Position = mvp * (position + vec4( s, s, s , 0));
	EmitVertex();
//
	EndPrimitive();
}


void main()
{
	OutColor = gs_in[0].Color;
	//gl_Position = gl_in[0].gl_Position;
	//MakeCube(gl_in[0].gl_Position, gs_in[0].Spacing);	
	//float Spacing = 0.1f;
	
	MakeCube(Projection * View * Model, gl_in[0].gl_Position);

//	gl_Position = Projection * View * Model * (gl_in[0].gl_Position + vec4(-0.2, -0.2, 0.0, 0.0));    // 1:bottom-left
//    EmitVertex();   
//    gl_Position = Projection * View * Model * (gl_in[0].gl_Position + vec4( 0.2, -0.2, 0.0, 0.0));    // 2:bottom-right
//    EmitVertex();
//    gl_Position = Projection * View * Model * (gl_in[0].gl_Position + vec4(-0.2,  0.2, 0.0, 0.0));    // 3:top-left
//    EmitVertex();
//    gl_Position = Projection * View * Model * (gl_in[0].gl_Position + vec4( 0.2,  0.2, 0.0, 0.0));    // 4:top-right
//    EmitVertex();
//    gl_Position = Projection * View * Model * (gl_in[0].gl_Position + vec4( 0.0,  0.4, 0.0, 0.0));    // 5:top
//    EmitVertex();
		
    EndPrimitive();


}
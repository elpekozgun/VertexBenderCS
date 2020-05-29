#version 430

layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inColor;

#include "light.glsl"

struct Material
{
	sampler2D diffuse;	//ambient and diffuse colors are almost the same for textures.
	sampler2D specular;
	float shineness;
};

uniform vec3 cameraPosition;
uniform Material material;
uniform DirectLight directLight;
uniform vec4 Color;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

out vec4 FragColor;

void main()
{
	vec3 viewdir = normalize(cameraPosition - inPos);
	vec3 lightDir = -normalize(directLight.direction);

	vec3 halfwayDir = normalize(viewdir + lightDir);

	float diff = max(dot(inNormal, lightDir),0);
	float spec = pow( max(dot(inNormal, halfwayDir),0.0f),material.shineness * 4);

	vec3 ambient = directLight.ambient; //* vec3(texture(material.diffuse,TexCoord));
	vec3 diffuse = directLight.diffuse * diff;// * vec3(texture(material.diffuse, TexCoord));
	vec3 specular = directLight.specular * spec;// * vec3(max(texture(material.specular,TexCoord),1.0f));

	gl_Position = Projection * View * Model * vec4(inPos,1.0f);
	FragColor = vec4(max(Color.xyz + ambient + diffuse + specular,vec3(0.0f)),1);

}

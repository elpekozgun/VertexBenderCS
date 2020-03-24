#version 420 core

in vec2 TexCoord;
in vec3 FragmentPosition;
in vec3 FragmentNormal;
in vec3 FragmentColor;

#include "Light.glsl"

#define NR_POINT_LIGHTS 3


struct Material
{
	sampler2D diffuse;	//ambient and diffuse colors are almost the same for textures.
	sampler2D specular;
	float shineness;
};

uniform vec3 cameraPosition;
uniform Material material;
uniform DirectLight directLight;
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotlight;
uniform vec4 Color;

out vec4 FragColor;

void main()
{
	vec3 norm = normalize(FragmentNormal);
	vec3 viewdir = normalize(cameraPosition - FragmentPosition);

	vec3 result = vec3(0.0f);

	result += CalculateDirectLight(directLight, norm, viewdir);
	
	for	(int i = 0; i < NR_POINT_LIGHTS; i++)
	{
		result += CalculatePointLight(pointLights[i], norm, FragmentPosition, viewdir);
	}
	
	result += CalculateSpotLight(spotlight, norm, FragmentPosition, viewdir);

	FragColor = vec4(result, 1.0f);
}

vec3 CalculateDirectLight(DirectLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	float diff = max(dot(normal,lightDir),0.0f);

	float spec = 0.0f;
	if(IsBlinnPhong)
	{
		vec3 halfwayDir = normalize(viewDir + lightDir);
		spec = pow( max(dot(normal, halfwayDir),0.0f),material.shineness * 4);
	}
	else
	{
		vec3 reflectDir = reflect(-lightDir,normal);
		spec = pow( max(dot(viewDir, reflectDir),0.0f),material.shineness);
	}

	vec3 ambient = light.ambient * vec3(texture(material.diffuse,TexCoord));
	vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoord));
	vec3 specular = light.specular * spec;// * vec3(max(texture(material.specular,TexCoord),1.0f));
	
	if(FragmentColor.x != 0.0f || FragmentColor.y != 0.0f || FragmentColor.z != 0)
	{
		return (max(FragmentColor + diffuse * 0.2f + specular,vec3(0.0f)));
	}

	return (max(Color.xyz + ambient + diffuse + specular,vec3(0.0f)));
}

vec3 CalculatePointLight(PointLight light, vec3 normal,vec3 fragPos, vec3 viewDir)
{
	vec3 lightdir = normalize(light.position - fragPos);

	float diff = max(dot(normal, lightdir), 0.0f);

	vec3 reflectDir = reflect(-lightdir,normal);
	float spec = pow( max( dot(viewDir,reflectDir),0.0f),material.shineness);

	float distance = length(light.position - fragPos);
	float attenuation = 1.0f / (light.Kconstant + light.Klinear * distance + light.Kquad * distance * distance );

	vec3 ambient = attenuation * light.ambient * vec3(texture(material.diffuse, TexCoord));
	vec3 diffuse = attenuation * light.diffuse * diff * vec3(texture(material.diffuse, TexCoord));
	vec3 specular = attenuation * light.specular * spec * vec3(texture(material.specular, TexCoord));

	return (max(ambient + diffuse + specular,vec3(0.0f)));
}

vec3 CalculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightdir = normalize(light.position - fragPos);

	float diff = max(dot(normal, lightdir), 0.0f);

	vec3 reflectDir = reflect(-lightdir,normal);
	float spec = pow( max( dot(viewDir,reflectDir),0.0f),material.shineness);

	float theta = dot(lightdir,-light.direction);
	float epsilon = light.cutOff - light.outerCutOff;
	float intensity = clamp((theta - light.cutOff) / epsilon,0.0f,1.0f);

	float distance = length(light.position - fragPos);
	float attenuation = 1.0f / (light.Kconstant + light.Klinear * distance + light.Kquad * distance * distance );

	vec3 ambient = attenuation * light.ambient * vec3(texture(material.diffuse, TexCoord));
	vec3 diffuse = intensity * attenuation * light.diffuse * diff * vec3(texture(material.diffuse, TexCoord));
	vec3 specular = intensity * attenuation * light.specular * spec * vec3(texture(material.specular, TexCoord));

	return (max(ambient + diffuse + specular,vec3(0.0f)));
}
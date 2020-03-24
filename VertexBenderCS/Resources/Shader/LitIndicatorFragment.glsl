#version 420 core

in vec2 TexCoord;
in vec3 FragmentPosition;
in vec3 FragmentNormal;
in vec3 FragmentColor;

struct Material
{
	sampler2D diffuse;	//ambient and diffuse colors are almost the same for textures.
	sampler2D specular;	//ambient and diffuse colors are almost the same for textures.
	float shineness;
};

#include "Light.glsl"

#define NR_POINT_LIGHTS 3

out vec4 FragColor;

uniform vec3 cameraPosition;
uniform Material material;
uniform DirectLight directLight;
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotlight;

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
	vec3 lightdir = normalize(-light.direction);

	float diff = max(dot(normal,lightdir),0.0f);

	vec3 reflectDir = reflect(-lightdir,normal);
	float spec = pow( max(dot( viewDir,reflectDir),0.0f),material.shineness);

	vec3 difSampler = vec3(texture(material.diffuse,TexCoord));
	vec3 specularSampler = vec3(texture(material.specular,TexCoord));

	vec3 ambient = light.ambient; //* vec3(texture(material.diffuse,TexCoord));
	vec3 diffuse = light.diffuse * diff; // * vec3(texture(material.diffuse, TexCoord));
	vec3 specular = light.specular * spec; // * spec * vec3(texture(material.texture_specular,TexCoord));

	return (max(FragmentColor * 0.5 + diffuse + 2 * specular,vec3(0.0f)));

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
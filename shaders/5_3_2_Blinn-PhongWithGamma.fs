#version 330 core 
out vec4 FragColor;

in VS_OUT{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
}fs_in;


uniform sampler2D diffuseTexture;
uniform samplerCube depthCubemap;

uniform vec3 lightPositions[1];
uniform vec3 lightColors[1];
uniform vec3 viewPos;

uniform bool blinn;

uniform bool gammaCorrection;

uniform float far_plane;




in vec3 result;

vec3 sampleOffsetDirections[20] = vec3[]
(
   vec3( 1,  1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1,  1,  1), 
   vec3( 1,  1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1,  1, -1),
   vec3( 1,  1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1,  1,  0),
   vec3( 1,  0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1,  0, -1),
   vec3( 0,  1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0,  1, -1)
);   

vec3 BlinnPhong(vec3 normal,vec3 fragPos,vec3 lightPos,vec3 lightColor)
{
    // diffuse 
    vec3 lightDir = normalize(lightPos - fragPos);
    float diff = max(dot(lightDir,normal),0.0);
    vec3 diffuse = diff * lightColor;
    // specular 
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    vec3 reflectDir = reflect(-lightDir,normal);
    float spec =0.0;
    vec3 halfwayDir = normalize(lightDir+viewDir);
    spec = pow(max(dot(normal,halfwayDir),0.0),64.0);
    vec3 specular = spec*lightColor;
    // simple attenuation
    float max_distance =1.5;
    float distance = length(lightPos-fs_in.FragPos);
    float attenuation = 1.0/(gammaCorrection ? distance*distance:distance);

    
    diffuse*=attenuation;
    specular*=attenuation;

    return diffuse+specular;

}

float ShadowCalculation(vec3 fragPos,vec3 normal,vec3 lightPos)
{
    vec3 fragToLight = fragPos - lightPos;

    float currentDepth = length(fragToLight);

    float shadow = 0.0;
    float bias =0.15;
    int samples = 20;
    float  viewDistance = length(viewPos - fragPos);
    float diskRadius = (1.0+(viewDistance/far_plane))/25.0;
    for(int i=0;i<samples;++i)
    {
        float closestDepth = texture(depthCubemap,fragToLight+sampleOffsetDirections[i]*diskRadius).r;
        closestDepth *=far_plane;
        if(currentDepth - bias > closestDepth)
            shadow+=1.0;
    }
    shadow /= float(samples);

   


    // for debugging
    //FragColor = vec4(vec3(closestDepth/far_plane),1.0);

    return shadow;


}

void main()
{
    vec3 color =texture(diffuseTexture,fs_in.TexCoords).rgb;

    vec3 lighting = vec3(0.0);
    for(int i=0;i<1;i++)
    {
        lighting+=BlinnPhong(normalize(fs_in.Normal),fs_in.FragPos,lightPositions[i],lightColors[i]);
    }



    float shadow = ShadowCalculation(fs_in.FragPos,normalize(fs_in.Normal),lightPositions[0]);

    vec3 ambient=0.01*lightColors[0];
    lighting=lighting*(1-shadow)+ ambient;
    color*=lighting;
    if(gammaCorrection)
        color=pow(color,vec3(1.0/2.2));
    

    FragColor = vec4(color,1.0);

}
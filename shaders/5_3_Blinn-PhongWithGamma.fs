#version 330 core 
out vec4 FragColor;

in VS_OUT{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
    vec4 FragPosLightSpace;
}fs_in;


uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;
uniform vec3 lightPositions[1];
uniform vec3 lightColors[1];
uniform vec3 viewPos;

uniform bool blinn;

uniform bool gammaCorrection;


in vec3 result;

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

float ShadowCalculation(vec4 fragPosLightSpace,vec3 normal,vec3 fragPos,vec3 lightPos)
{
    vec3 lightDir = normalize(lightPos - fragPos);
    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // transform to [0,1] range
    projCoords=projCoords*0.5+0.5;

    // get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;

    float bias = max(0.05*(1.0 - dot(normal,lightDir)),0.005);
    
    // PCF:percentage-closer filltering
    float shadow =0.0;
    vec2 texelSize = 1.0 /textureSize(shadowMap,0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
                // get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
            float pcfDepth = texture(shadowMap,projCoords.xy+vec2(x,y)*texelSize).r;
            // check whether current frag pos is in shadow
            shadow+=currentDepth - bias >pcfDepth?1.0:0.0;
        }
    }
    shadow /= 9.0;

    if(projCoords.z>1.0)
        shadow= 0.0;

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



    float shadow = ShadowCalculation(fs_in.FragPosLightSpace,normalize(fs_in.Normal),fs_in.FragPos,lightPositions[0]);

    float ambient=0.01;
    lighting=lighting*(1-shadow)+ ambient;
    color*=lighting;
    if(gammaCorrection)
        color=pow(color,vec3(1.0/2.2));
    

    FragColor = vec4(color,1.0);

}
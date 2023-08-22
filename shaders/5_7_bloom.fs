#version 330 core 
layout(location =0)out vec4 FragColor;
layout(location =1)out vec4 BrightColor;

in VS_OUT{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
}fs_in;


struct Light{
    vec3 Position;
    vec3 Color;
};


uniform Light lights[4];
uniform sampler2D diffuseTexture;
uniform vec3 viewPos;


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
    float distance = length(fs_in.FragPos-lightPos);
    float attenuation = 1.0/(gammaCorrection ? distance*distance:distance);

    
    diffuse*=attenuation;
    specular*=attenuation;

    return diffuse+specular;

}

void main()
{
    vec3 color =texture(diffuseTexture,fs_in.TexCoords).rgb;

    vec3 lighting = vec3(0.0);
    for(int i=0;i<4;i++)
    {
        lighting+=BlinnPhong(normalize(fs_in.Normal),fs_in.FragPos,lights[i].Position,lights[i].Color);
    }
    float ambient =0.001;
    lighting+=ambient;
    color*=lighting;
    

    FragColor = vec4(color,1.0);
    // if FragColor higher than threshold,then it is brightness color 
    float brightness= dot(FragColor.rgb,vec3(0.2126,0.7152,0.0722));
    if(brightness>1.0)
    {
        BrightColor = vec4(FragColor.rgb,1.0);
    }
    else
    {
        BrightColor = vec4(0.0,0.0,0.0,1.0);
    }

}
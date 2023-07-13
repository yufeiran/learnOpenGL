#version 330 core 
out vec4 FragColor;

in VS_OUT{
    vec3 FragPos;
    vec2 TexCoords;
    vec3 TangentLightPos;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
}fs_in;


uniform sampler2D texture_diffuse;
uniform sampler2D texture_specular;
uniform sampler2D texture_normal;
uniform sampler2D texture_depth;
uniform samplerCube depthCubemap;

uniform vec3 lightPositions[1];
uniform vec3 lightColors[1];
uniform vec3 viewPos;

uniform bool blinn;
uniform bool enableNormalMapping;
uniform bool enableParallaxMapping;

uniform bool gammaCorrection;

uniform float far_plane;

uniform float height_scale;


in vec3 result;

vec3 sampleOffsetDirections[20] = vec3[]
(
   vec3( 1,  1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1,  1,  1), 
   vec3( 1,  1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1,  1, -1),
   vec3( 1,  1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1,  1,  0),
   vec3( 1,  0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1,  0, -1),
   vec3( 0,  1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0,  1, -1)
);   

vec2 ParallaxMapping(vec2 texCoords,vec3 viewDir)
{
    // number of depth layers
    const float minLayers = 8.0;
    const float maxLayers = 32.0;
    float numLayers = mix(maxLayers,minLayers,max(dot(vec3(0.0,0.0,1.0),viewDir),0.0));
    // calculate the size of each layer 
    float layerDepth = 1.0 / numLayers;
    // depth of current layer
    float currentLayerDepth = 0.0;
    // the amount to shift the texture coordinates per layer (from vector P)
    vec2 P = viewDir.xy * height_scale;
    vec2 deltaTexCoords = P / numLayers;

    // get initial values 
    vec2 currentTexCoords = texCoords;
    float currentDepthMapValue = texture(texture_depth,currentTexCoords).r;

    while(currentLayerDepth<currentDepthMapValue)
    {
        // shift texture coordinates along direction of P 
        currentTexCoords -= deltaTexCoords;
        // get depthmap value at current texture coordinates
        currentDepthMapValue = texture(texture_depth,currentTexCoords).r;
        // get depth of next layer
        currentLayerDepth+=layerDepth;
    }

    // get texture coordinates before collision (reverse operations)
    vec2 prevTexCoords = currentTexCoords + deltaTexCoords;

    // get depth after and before collision for linear interpolation
    float afterDepth = currentDepthMapValue - currentLayerDepth;
    float beforeDepth = texture(texture_depth,prevTexCoords).r - currentLayerDepth + layerDepth;

    // interpolation of texture coordinates 
    float weight = afterDepth /(afterDepth - beforeDepth);
    vec2 finalTexCoords = prevTexCoords *weight + currentTexCoords * (1.0 -weight);


    return finalTexCoords;
}

vec3 BlinnPhong(vec3 normal,vec3 fragPos,vec3 lightPos,vec3 lightColor)
{
    // diffuse 
    vec3 lightDir = normalize(lightPos - fragPos);
    float diff = max(dot(lightDir,normal),0.0);
    vec3 diffuse = diff * lightColor;
    // specular 
    vec3 viewDir =normalize(fs_in.TangentViewPos - fragPos);
    vec3 reflectDir = reflect(-lightDir,normal);
    float spec =0.0;
    vec3 halfwayDir = normalize(lightDir+viewDir);
    spec = pow(max(dot(normal,halfwayDir),0.0),64.0);
    vec3 specular = spec*lightColor;
    // simple attenuation
    float max_distance =1.5;
    float distance = length(lightPos- fragPos);
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
    // offset texture coordinates with Parallax Mapping 
    vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.TangentFragPos);
    
    vec2 texCoords=fs_in.TexCoords;
    if(enableParallaxMapping)
    {
        texCoords = ParallaxMapping(fs_in.TexCoords,viewDir);
        if(texCoords.x>1.0||texCoords.y>1.0|| texCoords.x<0.0 || texCoords.y <0.0)
            discard;
    }
    



    vec3 color =texture(texture_diffuse,texCoords).rgb;

    vec3 lighting = vec3(0.0);

    // obtain normal from normal map in range [0,1]
    vec3 normal= texture(texture_normal,texCoords).rgb;

    normal = normalize(normal*2.0-1.0);

    //normal=normalize(fs_in.TBN*normal);
    if(enableNormalMapping==false)
    {
        normal=normalize(vec3(0,0,1));
    }

    for(int i=0;i<1;i++)
    {
        lighting+=BlinnPhong(normal,fs_in.TangentFragPos,fs_in.TangentLightPos,lightColors[i]);
    }



    float shadow = ShadowCalculation(fs_in.FragPos,normal,lightPositions[0]);

    vec3 ambient=0.01*lightColors[0];
    lighting=lighting*(1-shadow)+ ambient;
    color*=lighting;
    if(gammaCorrection)
        color=pow(color,vec3(1.0/2.2));
    

    FragColor = vec4(color,1.0);

}
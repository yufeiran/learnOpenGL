#version 330 core 
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gColorSpec;

uniform int type;

void main()
{
    vec3 color = texture(gPosition,TexCoords).rgb;
    if(type==1)
    {
        color = texture(gPosition,TexCoords).rgb;
    }
    else if(type==2)
    {
        color = texture(gNormal,TexCoords).rgb;
    }
    else if(type==3)
    {
        color = texture(gColorSpec,TexCoords).rgb;
    }
    else if(type==4)
    {
        float specular=texture(gColorSpec,TexCoords).a;
        color =vec3(specular,specular,specular);
    }


    FragColor = vec4(color,1.0);
}
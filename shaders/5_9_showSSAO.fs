#version 330 core 
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D SSAObuffer;

void main()
{
    float color = texture(SSAObuffer,TexCoords).r;

    FragColor = vec4(color,color,color,1.0);

}
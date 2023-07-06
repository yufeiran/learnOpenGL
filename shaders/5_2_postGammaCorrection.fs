#version 330 core 
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D screenTexture;

//uniform bool gammaCorrection;

void main()
{


    FragColor = texture(screenTexture,TexCoords);

    // float gamma = 2.2;
    // if(gammaCorrection)
    // {
    //     FragColor.rgb = pow(FragColor.rgb,vec3(1.0/gamma));
    // }

}
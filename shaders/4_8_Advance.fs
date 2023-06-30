#version 330 core 
out vec4 FragColor;

in vec3 Normal;
in vec3 Position; 

uniform vec3 cameraPos;


uniform samplerCube skybox;

void main()
{
    vec3 I = normalize(Position - cameraPos);
    vec3 R = reflect(I,normalize(Normal));
    FragColor = vec4(texture(skybox,R).rgb,1.0);
    if(gl_FragCoord.x<400)
        FragColor=vec4(1.0,0.0,0.0,1.0);
    else 
        FragColor=vec4(0.0,1.0,0.0,1.0);
    if(gl_FrontFacing)
        FragColor = vec4(1,1,0,1.0);
    else 
        FragColor = vec4(0,1,1,1);
}
#version 330 core 
layout (location = 0) out vec3 gPosition;
layout (location = 1) out vec3 gNormal;
layout (location = 2) out vec3 gAlbedoSpec;

in vec2 TexCoords;
in vec3 FragPos;
in vec3 Normal;

void main()
{
    // store the fragment position vector in the first gbuffer texture
    gPosition = FragPos;
    // store the per-fragment normals into the gbuffer
    gNormal = normalize(Normal);
    // store diffuse per-fragment color , ignore specular
    gAlbedoSpec.rgb = vec3(0.95);
}
#version 330 core 
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoords;
layout(location = 3) in vec3 aTangent;
layout(location = 4) in vec3 aBitangent;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

uniform vec3 lightPositions[1];
uniform vec3 lightColors[1];
uniform vec3 viewPos;


out VS_OUT{
    vec3 FragPos;
    vec2 TexCoords;
    vec3 TangentLightPos;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
}vs_out;


uniform bool reverse_normals;

void main()
{


    vs_out.FragPos = vec3(model*vec4(aPos,1.0));

    vs_out.TexCoords = aTexCoords;
    gl_Position = projection*view*model*vec4(aPos,1.0);

    mat3 normalMatrix =transpose(inverse(mat3(model))); 

    vec3 T= normalize(vec3(normalMatrix*aTangent));
    vec3 N= normalize(vec3(normalMatrix*aNormal));

    // re-orthogonalize T with respect to N
    T = normalize(T - dot(T,N)*N);
    // then retrieve perpendicular vector B with the cross product of T and N
    vec3 B =cross(N,T);


    mat3 TBN =transpose(mat3(T,B,N));
    vs_out.TangentLightPos = TBN*lightPositions[0];
    vs_out.TangentViewPos = TBN*viewPos;
    vs_out.TangentFragPos = TBN*vec3(model*vec4(aPos,1.0));

}
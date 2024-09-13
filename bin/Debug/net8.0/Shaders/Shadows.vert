#version 330 core
layout (location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 3) in vec2 aTexCoord;

uniform mat4 LightSpaceMatrix;
uniform mat4 Model;

out vec2 fragTexCoord;
out vec3 ONormal;
out vec3 OPos;
void main()
{
    ONormal = aNormal;
    fragTexCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0);
}
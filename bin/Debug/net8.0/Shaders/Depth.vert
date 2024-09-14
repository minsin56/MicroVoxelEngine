#version 330 core
layout (location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 3) in vec2 aTexCoord;

uniform mat4 ShadowProjection;
uniform mat4 ShadowView;
uniform mat4 View;

uniform mat4 Transform;

out vec2 fragTexCoord;
out vec3 ONormal;
out vec3 OPos;
void main()
{
    ONormal = aNormal;
    fragTexCoord = aTexCoord;
    gl_Position = ShadowProjection * ShadowView * Transform * vec4(aPosition, 1.0);
}
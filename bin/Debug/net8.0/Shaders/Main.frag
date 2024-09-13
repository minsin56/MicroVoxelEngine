#version 330 core
out vec4 FragColor;

in vec2 fragTexCoord;
in vec3 ONormal;
in vec3 OColor;
in vec3 OPos;

uniform sampler2D Texture;
void main()
{


    FragColor = vec4(OColor,1);
}
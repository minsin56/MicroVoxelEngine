#version 330 core
out vec4 FragColor;

in vec2 fragTexCoord;
in vec3 ONormal;
in vec3 OPos;

float Far = 50;
float Near = 1;

uniform sampler2D Texture;



void main()
{
    float Depth = texture(Texture,fragTexCoord).r;
    float z = Depth * 2.0 - 1.0;
    float Linear = (2.0 * Near * Far) / (Depth * (Far - Near) - (Far + Near));
    float Factor = (Near + Linear) / (Near - Far);

    FragColor = vec4(vec3(Depth / 2),1);
}
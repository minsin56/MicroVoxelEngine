#version 330 core
out vec4 FragColor;

in vec2 fragTexCoord;
in vec3 ONormal;
in vec3 OColor;
in vec4 OPos;
in vec4 OShadowPos;

uniform vec3 CamPos;

vec3 LightPos = vec3(30,40,-10);


uniform sampler2D DepthMap;


float CalcShadow()
{
    vec3 ProjCoords = OShadowPos.xyz / OShadowPos.w;
    float Shadow = 0.0;
    if(ProjCoords.z <= 1.0)
    {
        ProjCoords = (ProjCoords + 1.0) / 2.0;

        float CurrentDepth = ProjCoords.z;

        int SampleRadius = 2;

        vec2 PixelSize = 1.0 / textureSize(DepthMap,0);


        for(int x = -SampleRadius; x < SampleRadius; x++)
        {
            for(int y = -SampleRadius; y < SampleRadius; y++)
            {
                float ClosestDepth = texture(DepthMap, ProjCoords.xy + vec2(x,y) * PixelSize).r;
                if(CurrentDepth > ClosestDepth + 0.0005)
                {
                    Shadow += 1.0;
                }
            }
        }

        Shadow /= pow((SampleRadius * 2 + 1),2);

      

    }

    return max(Shadow,0.0);
}


void main()
{
    vec3 Normal = normalize(ONormal);
    vec3 LightDirection = normalize(LightPos - vec3(OPos));

    float Diffuse = max(dot(Normal,LightDirection),0.0);
    vec3 ViewDirection = normalize(CamPos - OPos.xyz);
    vec3 ReflecDir = reflect(-LightDirection, ONormal);
    float SpecAmount = pow(max(dot(ViewDirection,ReflecDir),0.0),8);
    float Spec = SpecAmount * 2;

    FragColor = vec4(OColor * vec3(Diffuse + 0.2 + Spec * CalcShadow()),1);

}

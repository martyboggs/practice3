// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skybox/Horizontal Skybox"
{
    Properties
    {
        _Color1 ("Top Color", Color) = (1, 1, 1, 0)
        _Color2 ("Horizon Color", Color) = (1, 1, 1, 0)
        _Color3 ("Bottom Color", Color) = (1, 1, 1, 0)

		_Color1_2("Top 2 Color", Color) = (1, 1, 1, 0)
		_Color2_2("Horizon 2 Color", Color) = (1, 1, 1, 0)
		_Color3_2("Bottom 2 Color", Color) = (1, 1, 1, 0)

		_ColorLerp("Color lerp", Float) = 1.0


        _Exponent1 ("Exponent Factor for Top Half", Float) = 1.0
        _Exponent2 ("Exponent Factor for Bottom Half", Float) = 1.0
        _Intensity ("Intensity Amplifier", Float) = 1.0
		_Steps ("Steps", Float) = 6.0
		_Darken ("Darken", Float) = 0.5
		_SkyLineCol ("Skyline col", Color) = (1,1,1,1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    half4 _Color1;
    half4 _Color2;
    half4 _Color3;

	half4 _Color1_2;
	half4 _Color2_2;
	half4 _Color3_2;

	float _ColorLerp;

	half4 _SkyLineCol;
    half _Intensity;
    half _Exponent1;
    half _Exponent2;

	float _Steps;
	float _Darken;
    
    v2f vert (appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos (v.position);
        o.texcoord = v.texcoord;
        return o;
    }
    
    half4 frag (v2f i) : COLOR
    {
		
        float p = normalize (i.texcoord).y;
        float p1 = 1.0f - pow (min (1.0f, 1.0f - p), _Exponent1);
        float p3 = 1.0f - pow (min (1.0f, 1.0f + p), _Exponent2);
        float p2 = 1.0f - p1 - p3;


		float x = floor(normalize(i.texcoord).y * _Steps);
		float t = fmod(x, 2.);

		float4 c1 = lerp(_Color1, _Color1_2, _ColorLerp);
		float4 c2 = lerp(_Color2, _Color2_2, _ColorLerp);
		float4 c3 = lerp(_Color3, _Color3_2, _ColorLerp);
        return (c1 * p1 + c2 * p2 + c3 * p3) * _Intensity + (t * _Darken * _SkyLineCol);
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off

            
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    } 
}

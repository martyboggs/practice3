Shader "Custom/JoepieShaderWobble" 
{
	Properties 
	{
		_Color("Main Color",Color) = (1,1,1,1)
		_ShadowMultiplier("Shadow Multiplier",float) = 1
		_Offset("Z-Offset", int) = 0

		_NormalOffset("Normal Offset", Float) = 0
		_TimeInfluence("Time Influence", Float) = 0
		_YDiv("Ydiv", Float) = 0
	}

	SubShader 
	{
		Tags 
		{ 
			"RenderType" = "Opaque" 
			"Queue" = "Geometry"
			"IgnoreProjector" = "True" 
		}

		Offset [_Offset], 0
		Cull Off

		CGPROGRAM
		#pragma surface surf Unlit vertex:vert addshadow
		#pragma target 3.0

		fixed4 _Color;
		fixed _ShadowMultiplier;

		float _NormalOffset;
		float _TimeInfluence;
		float _YDiv;

		uniform float _Glitch;


		struct Input 
		{
			fixed2 uv_MainTex;
			float3 worldPos;
		};


		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			
			_Glitch = 1.;

			v.vertex.x += o.worldPos.x;
			/*
			v.vertex.x += sin(v.vertex.y * _YDiv + round(_Time.y * _TimeInfluence)) * _NormalOffset * _Glitch;
			v.vertex.y += sin(v.vertex.x * _YDiv + round(_Time.y * _TimeInfluence)) * _NormalOffset * _Glitch;
			v.vertex.z += sin(v.vertex.y * _YDiv + round(_Time.y * _TimeInfluence)) * _NormalOffset * _Glitch;*/

			//v.vertex.xyz += v.normal * (_NormalOffset * (1 + rand(v.normal) * 0.2));

			//v.vertex.x += sin(v.vertex.y * _YDiv + _Time.y * _TimeInfluence) * 0.05;
		}

		fixed4 LightingUnlit ( SurfaceOutput s, fixed3 lightDir, fixed atten )
		{
			fixed4 c;

			c.rgb = s.Albedo;
			c.a = s.Alpha;

			//atten = step(.1,atten) * .5;
			c.rgb *= atten;

			return c * _LightColor0;
		}

		float _AboveYThreshold;

		void surf ( Input i, inout SurfaceOutput o ) 
		{
			fixed4 c = _Color;

			if (i.worldPos.y < _AboveYThreshold)
				c = float4(0, 0, 0, 0);

			o.Albedo = c.rgb;
			o.Alpha = 1;
			
		}
		ENDCG
	}

	Fallback "VertexLit"
}

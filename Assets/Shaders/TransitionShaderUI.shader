// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Transition" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}

		_TransitionTex("Transition Texture", 2D) = "black" {}
	_TransitionAmount("Transition amount", Float) = 0.5

		// these six unused properties are required when a shader
		// is used in the UI system, or you get a warning.
		// look to UI-Default.shader to see these.
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
		// see for example
		// http://answers.unity3d.com/questions/980924/ui-mask-with-shader.html

	}

		SubShader{
		Tags{ "Queue" = "Background"  "IgnoreProjector" = "True" }
		LOD 100

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass{
		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag
#include "UnityCG.cginc"

		fixed4 _Color;
	fixed4 _Color2;

	struct v2f {
		float4 pos : SV_POSITION;
		fixed4 col : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	v2f vert(appdata_full v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.col = v.color;
		o.texcoord = v.texcoord;
		//            o.col = half4( v.vertex.y, 0, 0, 1);
		return o;
	}

	uniform float _TransitionAmount;
	sampler2D _TransitionTex;

	float4 frag(v2f i) : COLOR{
		float4 c = i.col;

		float2 p = i.texcoord;


		p.x *= 10.;
		p.y *= 10.;

		
		c = tex2D(_TransitionTex, p - float2(_TransitionAmount, 0.));

		if (p.x < _TransitionAmount || c.a > 0.01) c = float4(0., 0., 0., 1.0);
		if (p.x > _TransitionAmount + .98) c = float4(0., 0., 0., 0.0);
		//if (i.texcoord.x > _TransitionAmount) c = float4(0., 0., 0., 0.);


		return c;
	}
		ENDCG
	}
	}
}
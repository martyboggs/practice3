Shader "Tutorial/032_ImprovedToon" {
	//show values to edit in inspector
	Properties{
		[Header(Base Parameters)]

	_MossColor("Moss", Color) = (1,1,1,1)
		_MossColor2("Moss 2", Color) = (1,1,1,1)
		_Color("Tint", Color) = (1, 1, 1, 1)
		_GroundColor("Ground tint", Color) = (1,1,1,1)
		
		_MainTex("Texture", 2D) = "white" {}
		_Specular("Specular Color", Color) = (1,1,1,1)
		[HDR] _Emission("Emission", color) = (0 ,0 ,0 , 1)

		_Color2("Tint2", Color) = (1,1,1,1)
		_GroundColor2("Ground tint 2", Color) = (1,1,1,1)
		_ColorLerp("Color lerp", Float) = 0


		[Header(Lighting Parameters)]
		_ShadowTint("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
		[IntRange]_StepAmount("Shadow Steps", Range(1, 32)) = 2
		_StepWidth("Step Size", Range(0, 1)) = 0.25
		_SpecularSize("Specular Size", Range(0, 1)) = 0.1
		_SpecularFalloff("Specular Falloff", Range(0, 2)) = 1

		
		_AboveYColor("Above y color", Color) = (1,1,1,1)
	}
		SubShader{
			//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			CGPROGRAM
			#include "AutoLight.cginc"
			#include "Random.cginc"

			
			#pragma surface surf Stepped fullforwardshadows vertex:vert
			#pragma target 3.0


			float easeIn(float interpolator) {
				return interpolator * interpolator;
			}

			float easeOut(float interpolator) {
				return 1 - easeIn(1 - interpolator);
			}

			float easeInOut(float interpolator) {
				float easeInValue = easeIn(interpolator);
				float easeOutValue = easeOut(interpolator);
				return lerp(easeInValue, easeOutValue, interpolator);
			}

			float perlinNoise(float3 value) {
				float3 fraction = frac(value);

				float interpolatorX = easeInOut(fraction.x);
				float interpolatorY = easeInOut(fraction.y);
				float interpolatorZ = easeInOut(fraction.z);

				float3 cellNoiseZ[2];
				[unroll]
				for (int z = 0;z <= 1;z++) {
					float3 cellNoiseY[2];
					[unroll]
					for (int y = 0;y <= 1;y++) {
						float3 cellNoiseX[2];
						[unroll]
						for (int x = 0;x <= 1;x++) {
							float3 cell = floor(value) + float3(x, y, z);
							float3 cellDirection = rand3dTo3d(cell) * 2 - 1;
							float3 compareVector = fraction - float3(x, y, z);
							cellNoiseX[x] = dot(cellDirection, compareVector);
						}
						cellNoiseY[y] = lerp(cellNoiseX[0], cellNoiseX[1], interpolatorX);
					}
					cellNoiseZ[z] = lerp(cellNoiseY[0], cellNoiseY[1], interpolatorY);
				}
				float3 noise = lerp(cellNoiseZ[0], cellNoiseZ[1], interpolatorZ);
				return noise;
			}

			

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _GroundColor;
			fixed4 _MossColor;
			fixed4 _MossColor2;
			fixed4 _Color2;
			fixed4 _GroundColor2;
			float _ColorLerp;
			float _MinY;

			half3 _Emission;
			fixed4 _Specular;

			float3 _ShadowTint;
			float _StepWidth;
			float _StepAmount;
			float _SpecularSize;
			float _SpecularFalloff;
			float _AboveYThreshold;
			fixed4 _AboveYColor;

			struct ToonSurfaceOutput {
				fixed3 Albedo;
				half3 Emission;
				fixed3 Specular;
				fixed Alpha;
				fixed3 Normal;
				fixed3 Position;
				float Moss;
			};

			//our lighting function. Will be called once per light
			float4 LightingStepped(ToonSurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation) {
				//how much does the normal point towards the light?
				float towardsLight = dot(s.Normal, lightDir);

				//stretch values so each whole value is one step
				towardsLight = towardsLight / _StepWidth;
				//make steps harder
				float lightIntensity = floor(towardsLight);

				// calculate smoothing in first pixels of the steps and add smoothing to step, raising it by one step
				// (that's fine because we used floor previously and we want everything to be the value above the floor value, 
				// for example 0 to 1 should be 1, 1 to 2 should be 2 etc...)
				float change = fwidth(towardsLight);
				float smoothing = smoothstep(0, change, frac(towardsLight));
				lightIntensity = lightIntensity + smoothing;

				// bring the light intensity back into a range where we can use it for color
				// and clamp it so it doesn't do weird stuff below 0 / above one

				_StepAmount = (1. - s.Moss) * _StepAmount;

				lightIntensity = lightIntensity / _StepAmount;
				lightIntensity = saturate(lightIntensity);

			#ifdef USING_DIRECTIONAL_LIGHT
				//for directional lights, get a hard vut in the middle of the shadow attenuation
				//float attenuationChange = fwidth(shadowAttenuation) * 0.5;
				//float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
				
				float shadow = max(0.0, shadowAttenuation - (_AboveYThreshold - s.Position.y) * 0.03);
				//float shadow = shadowAttenuation;
			#else
				//for other light types (point, spot), put the cutoff near black, so the falloff doesn't affect the range

				//UNITY_LIGHT_ATTENUATION(attenuation, 0, s.Position);
				//float attenuationChange = fwidth(shadowAttenuation);
				float shadow = shadowAttenuation;//floor(shadowAttenuation * 5.) / 5.;//smoothstep(0, attenuationChange, shadowAttenuation);
				//float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
			#endif

				lightIntensity = lightIntensity * shadow;

				//calculate how much the surface points points towards the reflection direction
				float3 reflectionDirection = reflect(lightDir, s.Normal);
				float towardsReflection = dot(viewDir, -reflectionDirection);

				//make specular highlight all off towards outside of model
				float specularFalloff = dot(viewDir, s.Normal);
				specularFalloff = pow(specularFalloff, _SpecularFalloff);
				towardsReflection = towardsReflection * specularFalloff;

				//make specular intensity with a hard corner
				float specularChange = fwidth(towardsReflection);
				float specularIntensity = smoothstep(1 - _SpecularSize, 1 - _SpecularSize + specularChange, towardsReflection);
				//factor inshadows
				specularIntensity = specularIntensity * shadow;

				float4 color;
				//calculate final color
				color.rgb = s.Albedo * lightIntensity * _LightColor0.rgb;
				color.rgb = lerp(color.rgb, s.Specular * _LightColor0.rgb, saturate(specularIntensity));

				color.a = s.Alpha;
				return color;
			}


			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
			};

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);


				if (o.worldPos.y > _AboveYThreshold)
				{
					v.vertex.x += 1000.;
				}
			}
			
			void surf(Input i, inout ToonSurfaceOutput o) {
				fixed4 col = tex2D(_MainTex, i.uv_MainTex);

				float l = 1. - (i.worldPos.y - _MinY) / (_AboveYThreshold - _MinY);


				col *= lerp(_Color, _Color2, l);


				float m = .1;
				float noise = perlinNoise(i.worldPos.xyz * m);
				noise *= noise;
				float noise2 = perlinNoise(i.worldPos.xyz / 100.);
				noise2 *= noise2;

				//noise = noise2;
				//noise *= noise2;

				o.Albedo = col.rgb;
				o.Position = i.worldPos;


				float d = dot(o.Normal, float3(0, 1., 0));
				if (d > .02)
					o.Albedo = lerp(_GroundColor, _GroundColor2, l);



				o.Moss = 0;
				if (noise2 > 0.04 && noise > 0.1)
				{
					o.Albedo = lerp(_MossColor, _MossColor2, l);
					//o.Moss = 1;
				}

				if (i.worldPos.y > _AboveYThreshold)
				{
					o.Albedo = _AboveYColor;
					o.Moss = 0;
				}

				float3 wp = float3(i.worldPos.x, 0, i.worldPos.z);

				if (distance(wp, float3(0, 0, 0)) > 40.)
				{
					clip(-1);
				}

				o.Specular = _Specular;

				float3 shadowColor = col.rgb * _ShadowTint;
				shadowColor *= max(1. - (_AboveYThreshold - i.worldPos.y) * 0.03, 0.0);
				o.Emission = shadowColor;
			}
			ENDCG
		}
			FallBack "Standard"
}
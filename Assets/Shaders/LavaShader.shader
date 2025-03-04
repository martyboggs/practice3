﻿Shader "Custom/LavaShader" {
	Properties{
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_CellSize("Cell Size", Range(0, 2)) = 1
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			#include "Random.cginc"

			float _CellSize;
			fixed4 _Color1;
			fixed4 _Color2;

			struct Input {
				float3 worldPos;
			};

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

			float _LavaRising;
			void surf(Input i, inout SurfaceOutputStandard o) {
				float2 value = i.worldPos.xz / _CellSize;
				//get noise and adjust it to be ~0-1 range

				float noise = perlinNoise(float3(value.x, value.y, _Time.y)) + 0.5;
				noise = round(noise);	


				o.Albedo = lerp(_Color1, _Color2, noise);

				if (_LavaRising < 0.5) o.Albedo = float4(0, 0, 0, 0);
				o.Emission = o.Albedo;
			}
			ENDCG
	}
		FallBack "Standard"
}
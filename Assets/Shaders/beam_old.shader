// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/beam_old" {
	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		Cull Off
		Blend SrcAlpha One 				// alpha additive
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 			
 			#include "UnityCG.cginc"

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD1;
			};

 			struct v2f {
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
 			};
 			
            v2f vert(appdata_custom v)
            {
				float4 tv0 = v.vertex;
				float4 opponent = float4(v.texcoord.y, v.texcoord2.x, v.texcoord2.y, 1);

				float3 diff = normalize(opponent - v.vertex).xyz;
				float3 eyeVector = ObjSpaceViewDir(tv0);
				float3 sideVector = normalize(cross(eyeVector, diff));
				tv0.xyz += (v.texcoord.x-0.5f)*sideVector*0.5;
				float4 finalposition = tv0;

            	v2f o;
			    o.pos = UnityObjectToClipPos( finalposition);
            	o.color = v.color;
            	return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				return i.color;
            }

            ENDCG
        }
    }
}

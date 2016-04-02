Shader "Custom/spark" {
	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		// Blend SrcAlpha OneMinusSrcAlpha // alpha blending
		Blend SrcAlpha One 				// alpha additive
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 
 			#include "UnityCG.cginc"

			uniform fixed4 _Colors[8];

 			struct appdata_custom {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				// fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				float4 texcoord2 : TEXCOORD1;
			};

 			struct v2f
 			{
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
 			};
 			
 			float4x4 _PrevInvMatrix;
			float    _CurrentTime;
			float    _PreviousTime;
   
            v2f vert(appdata_custom v)
            {
				float elapsed = (_CurrentTime - v.texcoord2.x);
            	float4 tv0 = v.vertex;
				float alpha = clamp(1 - elapsed*2, 0, 1); // life time:0.5sec
				float scale = 16;
				float flow_z = -30;

				float size = elapsed * scale;
				tv0.xyz *= size;
				tv0.xyz += v.normal.xyz;
				tv0.z += elapsed * flow_z;
				tv0 *= v.texcoord.x;
            	tv0 = mul(UNITY_MATRIX_MVP, tv0);
            	
				float prev_elapsed = (_PreviousTime - v.texcoord2.x);
				float prev_size = prev_elapsed * scale;
            	float4 tv1 = v.vertex;
				tv1.xyz *= prev_size;
				tv1.xyz += v.normal.xyz;
				tv1.z += prev_elapsed * flow_z;
				tv1 *= v.texcoord.y;
				tv1 = mul(UNITY_MATRIX_MV, tv1);
            	tv1 = mul(_PrevInvMatrix, tv1);
            	tv1 = mul(UNITY_MATRIX_P, tv1);
            	
            	v2f o;
            	o.pos = tv0 + tv1;
            	// o.color = v.color;
				int color_index = (int)v.texcoord2.y;
				o.color = _Colors[color_index];
				o.color.a *= alpha;
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

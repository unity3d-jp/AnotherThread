// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/debris" {
	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha // alpha blending
//		Blend SrcAlpha One 				// alpha additive
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 
 			
 			#include "UnityCG.cginc"

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
			};

 			struct v2f
 			{
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
 			};
 			
 			float4x4 _PrevInvMatrix;
			float3   _TargetPosition;
			float    _Range;
			float    _RangeR;
			float    _MoveTotal;
			float    _Move;
   
            v2f vert(appdata_custom v)
            {
				v.vertex.z -= _MoveTotal;
				float3 target = _TargetPosition;
				float3 trip;
				trip = floor( ((target - v.vertex.xyz)*_RangeR + 1) * 0.5 );
				trip *= (_Range * 2);
				v.vertex.xyz += trip;

            	float4 tv0 = v.vertex * v.texcoord.x;
            	tv0 = UnityObjectToClipPos (tv0);
            	
				v.vertex.z += _Move;
            	float4 tv1 = v.vertex * v.texcoord.y;
				tv1.xyz = UnityObjectToViewPos(tv1);
            	tv1 = mul (_PrevInvMatrix, tv1);
            	tv1 = mul (UNITY_MATRIX_P, tv1);
            	
            	v2f o;
            	o.pos = tv0 + tv1;
            	float depth = o.pos.z * 0.02;
            	float normalized_depth = (1 - depth);
            	o.color = v.color;
            	o.color.a *= (normalized_depth);
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

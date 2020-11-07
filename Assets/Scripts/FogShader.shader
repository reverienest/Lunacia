Shader "Hidden/FogShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_bwBlend ("Black & White blend", Range (0, 1)) = 0
        _FogTex ("Fog", 2D) = "white" {}
        _GradTex ("Grad", 2D) = "white" {}
		_SpreadTex("Spread", 2D) = "white" {}
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _bwBlend;
            uniform sampler2D _FogTex;
            uniform sampler2D _GradTex;
			uniform sampler2D _SpreadTex;

			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);
				
				// float lum = c.r*.3 + c.g*.59 + c.b*.11;
				// float3 bw = float3( lum, lum, lum ); 
                float4 fog = tex2D(_FogTex, i.uv);
                float4 grad = tex2D(_GradTex, i.uv);
				float4 spread = tex2D(_SpreadTex, i.uv);
				
				float4 result = c;
				// result.rgb = lerp(c.rgb, bw, _bwBlend);
                result.rgb = c.rgb + fog.rgb * grad.rgb * spread.rgb * _bwBlend;
				return result;
			}
			ENDCG
		}
	}
}
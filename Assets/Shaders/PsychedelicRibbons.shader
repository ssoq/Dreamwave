Shader "Custom/LiquidifyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Texture Scale", Float) = 1.0
        _WarpScale ("Warp Scale", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Scale;
            float _WarpScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Scale; // Apply texture scale
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 distortion = float2(
                    sin(i.uv.y * 10.0) * 0.02 * _WarpScale,
                    cos(i.uv.x * 10.0) * 0.02 * _WarpScale
                );
                float2 uv = i.uv + distortion;
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}

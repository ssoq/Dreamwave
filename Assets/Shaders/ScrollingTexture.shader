Shader "Custom/ScrollingTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeedX ("Scroll Speed X", Float) = 0.1
        _ScrollSpeedY ("Scroll Speed Y", Float) = 0.0
        _ScaleX ("Scale X", Float) = 1.0
        _ScaleY ("Scale Y", Float) = 1.0
        _RealTime ("Real Time", Float) = 0.0
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
            float4 _MainTex_ST;
            float _ScrollSpeedX;
            float _ScrollSpeedY;
            float _ScaleX;
            float _ScaleY;
            float _RealTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply separate scaling
                float2 scaledUV = v.uv * float2(_ScaleX, _ScaleY);

                // Scroll texture coordinates over time
                float2 scrolledUV = scaledUV + float2(_RealTime * _ScrollSpeedX, _RealTime * _ScrollSpeedY);
                o.uv = TRANSFORM_TEX(scrolledUV, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture at the scrolled and scaled UV coordinates
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
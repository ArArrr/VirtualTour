Shader "Custom/AlwaysOnTopUI"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _AlphaClipThreshold ("Alpha Clip Threshold", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _AlphaClipThreshold;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR; // Adds support for UI color tint
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR; // Pass color to fragment
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                // Transform vertex position with stereo support
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color; // Pass the vertex color for alpha blending
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
                
                // Multiply by vertex color alpha (supports CanvasGroup and UI Color modifiers)
                texColor *= i.color;

                // Alpha clipping
                if (texColor.a < _AlphaClipThreshold)
                    discard;

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}

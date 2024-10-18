Shader "Custom/AlwaysOnTopUI"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)           // Base Color
        _MainTex ("Base (RGB)", 2D) = "white" {}       // Texture Map
        _AlphaClipThreshold ("Alpha Clip Threshold", Range(0,1)) = 0.5   // Threshold for alpha clipping
    }
    SubShader
    {
        Tags {"Queue" = "Overlay"}                     // Renders in the overlay queue
        Pass
        {
            Cull Off                                   // Render both sides of the UI element
            ZWrite Off                                 // Do not write to the depth buffer
            ZTest Always                               // Always render this pass
            Blend SrcAlpha OneMinusSrcAlpha            // Enable standard alpha blending

            CGPROGRAM
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Shader properties
            sampler2D _MainTex;                        // Texture sampler for the main texture
            float4 _Color;                             // Color property for tinting
            float _AlphaClipThreshold;                 // Alpha clip threshold property

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

            v2f vert (appdata v)
            {
                v2f o;

                // Handle VR stereo rendering using UnityObjectToClipPos and UNITY_MATRIX_MVP
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture at the UV coordinates
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Apply the base color as a tint
                texColor *= _Color;

                // Alpha clipping: Discard pixels below the threshold
                if (texColor.a < _AlphaClipThreshold)
                    discard;

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}

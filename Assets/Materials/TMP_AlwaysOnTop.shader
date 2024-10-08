Shader "Custom/TMP_AlwaysOnTop"
{
    Properties
    {
        _FaceColor ("Face Color", Color) = (1,1,1,1)      // Color of the text face
        _OutlineColor ("Outline Color", Color) = (0,0,0,1) // Outline color
        _OutlineWidth ("Outline Width", Float) = 0.2       // Outline width
        _MainTex ("Font Atlas", 2D) = "white" {}           // Font texture
        _ClipThreshold ("Clip Threshold", Range(0, 1)) = 0.5 // Alpha threshold for clipping
    }

    SubShader
    {
        Tags {"Queue" = "Overlay" }
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _FaceColor;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _ClipThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _FaceColor;  // Multiply by the text color
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.a = texColor.a * i.color.a;

                // Alpha Clipping
                if (texColor.a < _ClipThreshold)
                    discard;

                return texColor * _FaceColor;
            }
            ENDCG
        }
    }
}

Shader "Custom/GradientShader"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (1,1,1,1)
        _ColorBottom ("Bottom Color", Color) = (1,1,1,0)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Cull Back
            ZWrite Off
            ZTest Less
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _ColorTop;
            fixed4 _ColorBottom;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.yz; // Use the y component for vertical gradient
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate the gradient color based on the y position
                float gradientFactor = i.uv.x; // Y position for gradient
                fixed4 gradientColor = lerp(_ColorBottom, _ColorTop, gradientFactor);
                return gradientColor;
            }
            ENDCG
        }
    }
}

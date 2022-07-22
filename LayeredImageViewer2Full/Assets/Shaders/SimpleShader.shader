Shader "LayeredImageView/SimpleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaMult ("Alpha Multiplier", Float) = 1.0
        _Cutoff("Cutoff", Float) = 0.0
        _MinColorScale("Min Color Scale", Float) = 0.25
        _MaxColorScale("Max Color Scale", Float) = 0.75
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Cull off
        Blend SrcAlpha OneMinusSrcAlpha

        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
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
            float _AlphaMult;
            float _Cutoff;
            float _MinColorScale;
            float _MaxColorScale;

            fixed4 frag (v2f_img i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 rgb = col.rgb;
                if (dot(rgb,rgb) > _Cutoff*_Cutoff) {
                    col.a *= _AlphaMult;
                    //fixed3 blue = fixed3(0, 0, 1);
                    //fixed3 green = fixed3(0, 1, 0);
                    //fixed3 yellow = fixed3(1, 1, 0);
                    //float delta = smoothstep(_MinColorScale, _MaxColorScale, rgb.r);
                    //if (delta < 0.5) {
                    //    col.rgb = lerp(blue, green, delta*2.0);
                    //}
                    //else {
                    //    col.rgb = lerp(green,yellow,(delta-0.5)*2.0);
                    //}
                    //col.rgb = lerp(rgb, col.rgb, (sin(30*_Time.y) + 1) / 2);
                }
                else {
                    col = fixed4(0, 0, 0, 0);
                }
                return col;
            }
            ENDCG
        }
    }
}

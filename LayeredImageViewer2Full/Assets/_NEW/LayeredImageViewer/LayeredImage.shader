// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/LayeredImage"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Cutoff("Cutoff", float) = 0.2
        _Alpha("Alpha", float) = 0.4
        _Saturation("Satruation", float) = 0.5
        _NormalCutoff("NormalCutoff", float) = 0.01
        _ClipBase("ClipBase", Vector) = (0,0,0,0)
        _ClipNormal("ClipNormal", Vector) = (1,1,1,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;
            float _NormalCutoff;
            float _Alpha;
            float _Saturation;
            float4 _ClipBase;
            float4 _ClipNormal;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex = v.vertex;
                o.worldPos =
                    mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                clip(-dot((i.worldPos.xyz - _ClipBase.xyz),_ClipNormal)); 
                half camera_normal = abs(dot(normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz), i.normal));
                if (camera_normal < _NormalCutoff) return fixed4(0, 0, 0, 0);
                fixed4 col = tex2D(_MainTex, i.uv);
                // Compute the maximum color component for grayscale
                float maxColor = max(max(col.r, col.g), col.b);

                // Create the grayscale color
                float4 color_gray = float4(maxColor, maxColor, maxColor, col.a);

                // Lerp between the original color and the grayscale color
                col = lerp(col,color_gray, _Saturation);

                half value_squared = dot(col.rgb, col.rgb);
                half modifier = step(_Cutoff*0.5, maxColor);
          
                col.a *= clamp(_Alpha*value_squared,0,1);
                //col.a *= _Alpha;

                col *= modifier;
                return col;
            }
            ENDCG
        }
    }
}

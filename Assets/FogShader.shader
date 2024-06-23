Shader "Custom/MergeParticlesShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _MergeRadius ("Merge Radius", Float) = 1.0
        _BlendFactor ("Blend Factor", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _MergeRadius;
            float _BlendFactor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, i.uv) * _Color;

                // Initialize merged color and count
                fixed3 mergeColor = baseColor.rgb;
                float mergeCount = 1.0;

                // Sample nearby particles within merge radius
                for (float y = -_MergeRadius; y <= _MergeRadius; y += 1.0)
                {
                    for (float x = -_MergeRadius; x <= _MergeRadius; x += 1.0)
                    {
                        float2 offset = float2(x, y);
                        float2 uvOffset = i.uv + offset / _MainTex_ST.xy;

                        fixed4 neighborColor = tex2D(_MainTex, uvOffset) * _Color;
                        float distance = length(offset);

                        // Blend colors based on proximity
                        if (distance <= _MergeRadius)
                        {
                            mergeColor += neighborColor.rgb;
                            mergeCount += 1.0;
                        }
                    }
                }

                // Calculate final merged color
                fixed3 finalColor = mergeColor / mergeCount;
                return fixed4(finalColor, baseColor.a);
            }
            ENDCG
        }
    }
}

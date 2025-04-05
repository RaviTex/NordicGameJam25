Shader "Unlit/WorldBending"
{
Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BendAmount ("Bend Amount", Float) = 0.2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "Unlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST; // x = tiling.x, y = tiling.y, z = offset.x, w = offset.y
            float _BendAmount;

            float3 BendWorldPos(float3 worldPos, float bendAmount)
            {
                float radius = 1.0 / max(bendAmount, 0.0001);
                float angle = worldPos.z * bendAmount;

                float y = worldPos.y;
                float z = radius * sin(angle);
                y -= radius * (1 - cos(angle));

                return float3(worldPos.x, y, z);
            }

            Varyings vert(Attributes input)
            {
                Varyings output;

                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                worldPos = BendWorldPos(worldPos, _BendAmount);
                output.positionHCS = TransformWorldToHClip(worldPos);

                // Apply Tiling and Offset to UV
                output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
Shader "Custom/URP_Bend_With_Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BendAmount ("Bend Amount", Float) = 0.2
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.02
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" }

        // --------- OUTLINE PASS ----------
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="UniversalForward" }
            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float _BendAmount;
            float _OutlineThickness;
            float4 _OutlineColor;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float3 BendWorld(float3 pos)
            {
                float r = 1.0 / max(_BendAmount, 0.0001);
                float angle = pos.z * _BendAmount;
                float y = pos.y - r * (1 - cos(angle));
                float z = r * sin(angle);
                return float3(pos.x, y, z);
            }

            Varyings vert(Attributes input)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                float3 normal = TransformObjectToWorldNormal(input.normalOS);
                worldPos += normal * _OutlineThickness;
                worldPos = BendWorld(worldPos);
                o.positionHCS = TransformWorldToHClip(worldPos);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }

        // --------- MAIN TEXTURED PASS ----------
        Pass
        {
            Name "Main"
            Tags { "LightMode"="UniversalForward" }
            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float _BendAmount;

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

            float3 BendWorld(float3 pos)
            {
                float r = 1.0 / max(_BendAmount, 0.0001);
                float angle = pos.z * _BendAmount;
                float y = pos.y - r * (1 - cos(angle));
                float z = r * sin(angle);
                return float3(pos.x, y, z);
            }

            Varyings vert(Attributes input)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
                worldPos = BendWorld(worldPos);
                o.positionHCS = TransformWorldToHClip(worldPos);
                o.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}

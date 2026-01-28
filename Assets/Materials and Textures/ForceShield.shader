Shader "Custom/URP_SimpleBubble"
{
    Properties
    {
        _Color("Bubble Color", Color) = (0.2, 1, 0.2, 1)
        _Opacity("Base Opacity", Range(0,1)) = 0.15

        _RimPower("Rim Power", Range(0.2, 10)) = 4
        _RimStrength("Rim Strength", Range(0, 1)) = 0.8
        _RimColor("Rim Color (optional)", Color) = (1, 1, 1, 1)
        _RimColorStrength("Rim Color Strength", Range(0, 1)) = 0.25
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" "RenderType"="Transparent" }

        // Bubble usually looks better double-sided; change to Back if you want.
        Cull Off

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float  _Opacity;

                float  _RimPower;
                float  _RimStrength;
                float4 _RimColor;
                float  _RimColorStrength;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                half   fogFactor  : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                VertexPositionInputs pos = GetVertexPositionInputs(v.positionOS.xyz);
                VertexNormalInputs nrm   = GetVertexNormalInputs(v.normalOS);

                o.positionCS = pos.positionCS;
                o.positionWS = pos.positionWS;
                o.normalWS   = normalize(nrm.normalWS);
                o.fogFactor  = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                float3 N = normalize(i.normalWS);
                float3 V = normalize(GetWorldSpaceViewDir(i.positionWS));

                // Fresnel term: 0 at center-facing, 1 at grazing angles (rim)
                float ndv = saturate(dot(N, V));
                float rim = pow(1.0 - ndv, _RimPower);        // your "Shield Rim Power" equivalent

                // Alpha increases towards rim => edges less transparent
                float alpha = saturate(_Opacity + rim * _RimStrength * (1.0 - _Opacity));

                // Optional rim tint
                float3 rgb = _Color.rgb;
                rgb = lerp(rgb, _RimColor.rgb, rim * _RimColorStrength);

                rgb = MixFog(rgb, i.fogFactor);
                return half4(rgb, alpha);
            }
            ENDHLSL
        }
    }
}

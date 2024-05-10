Shader "Hidden/Assignment5/VignetteMaskedOverlayTexture"
{
    Properties
    { 
        _MainTex("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "Universal Forward"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct Attributes
            {
                float4 positionOS   : POSITION;    
                float2 uv : TEXCOORD0;             
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
            };            
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _VignetteIntensity;
            float _VignettePower;

            TEXTURE2D(_OverlayTexture);
            SAMPLER(sampler_OverlayTexture);

            float4 _OverlayTint;
            float2 _OverlayVelocity;

            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
     
            half4 frag(Varyings IN) : SV_Target
            {
                float2 prePolarUvs = IN.uv * 2 - 1;
                float2 polarCoords = float2(length(prePolarUvs), (atan2(prePolarUvs.y, prePolarUvs.x) / PI) * 0.5 + 0.5);
                float2 vigUv = IN.uv * (1-IN.uv.yx);
                float vignette = 1 - pow(vigUv.x * vigUv.y * _VignetteIntensity, _VignettePower);
                
                float4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float4 vignetteMask = SAMPLE_TEXTURE2D(_OverlayTexture, sampler_OverlayTexture, polarCoords + _OverlayVelocity * _Time.y) * _OverlayTint;

                return lerp(mainColor, vignetteMask, saturate(vignette) * vignetteMask.a * vignetteMask.r);
            }
            ENDHLSL
        }
    }
}
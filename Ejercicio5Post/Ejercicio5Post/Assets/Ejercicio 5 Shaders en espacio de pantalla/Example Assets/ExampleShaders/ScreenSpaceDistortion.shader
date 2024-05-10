Shader "Hidden/Assignment5/ScreenSpaceDistortion"
{
    Properties
    { 
        _MainTex("Main Texture", 2D) = "white" {}
        _DistortionMap("Distortion Map", 2D) = "bump" {}
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
            SamplerState my_linear_clamp_sampler;

            TEXTURE2D(_DistortionMap);
            SAMPLER(sampler_DistortionMap);

            float _DistortionAmount;
            float2 _DistortionVelocity;
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
     
            half4 frag(Varyings IN) : SV_Target
            {
                float2 distortionVector = UnpackNormal(SAMPLE_TEXTURE2D(_DistortionMap, sampler_DistortionMap, IN.uv + _DistortionVelocity * _Time.y)).xy;
                
                float4 distortedScreen = SAMPLE_TEXTURE2D(_MainTex, my_linear_clamp_sampler, IN.uv + distortionVector * _DistortionAmount);

                return distortedScreen;
            }
            ENDHLSL
        }
    }
}
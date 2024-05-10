Shader "Hidden/Assignment5/ChromaticAberration"
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
            SamplerState my_linear_clamp_sampler;

            float _RedShift;
            float _GreenShift;
            float _BlueShift;
            
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
     
            half4 frag(Varyings IN) : SV_Target
            {
                float2 offsetDir = IN.uv * 2-1;
                float2 centerForce = length(offsetDir);
                float2 vigUv = IN.uv * (1-IN.uv.yx);
                float vignette = 1 - pow(vigUv.x * vigUv.y * 15, 2);
                
                float2 r = SAMPLE_TEXTURE2D(_MainTex, my_linear_clamp_sampler, IN.uv + offsetDir * centerForce * vignette * _RedShift).ra;
                float g = SAMPLE_TEXTURE2D(_MainTex, my_linear_clamp_sampler, IN.uv + offsetDir * centerForce * vignette * _GreenShift).g;
                float b = SAMPLE_TEXTURE2D(_MainTex, my_linear_clamp_sampler, IN.uv + offsetDir * centerForce * vignette * _BlueShift).b;

                return float4(r.r, g,b,r.g);
            }
            ENDHLSL
        }
    }
}
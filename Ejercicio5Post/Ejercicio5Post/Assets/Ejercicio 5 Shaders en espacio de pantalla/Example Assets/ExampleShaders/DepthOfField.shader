Shader "Hidden/Assignment5/DepthOfField"
{
    Properties{
        _MainTex("Main Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
    
        Pass
        {
            ZWrite Off
            Name "Universal Forward"
            
            HLSLPROGRAM
            
            #pragma vertex vertex
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;

            float _BlurAmount;
            float _BlurDistance;
            float _FocalPoint;
            float _Aperture;
            
            struct VertexAttributes{
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings{
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 BoxBlur3x3(Texture2D tex, SamplerState ss, float2 uv, float2 texelSize, float2 texelOffset)
            {
                float4 res = 0;
                [unroll(9)]
                for(int y = 1; y > -2; y--)
                {
                    for(int x = -1; x < 2; x++)
                    {
                        res += SAMPLE_TEXTURE2D(tex, ss, uv + texelSize * texelOffset * float2(x,y));
                    }
                }

                res /= 9;
                return res;
            }
            
            Varyings vertex(VertexAttributes IN){
                Varyings OUT = (Varyings)0;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            
            float4 frag(Varyings IN) : SV_Target{
                float4 screenColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float eyeDepth  = LinearEyeDepth(SampleSceneDepth(IN.uv), _ZBufferParams);
                float depthMask =  smoothstep(0, _Aperture,abs(eyeDepth - _FocalPoint));
                float4 blurredScreenCOlor = BoxBlur3x3(_MainTex, sampler_MainTex, IN.uv, _MainTex_TexelSize.xy, _BlurDistance);
                return lerp(screenColor, blurredScreenCOlor, depthMask);
            }
            
            ENDHLSL
        }
    }
}

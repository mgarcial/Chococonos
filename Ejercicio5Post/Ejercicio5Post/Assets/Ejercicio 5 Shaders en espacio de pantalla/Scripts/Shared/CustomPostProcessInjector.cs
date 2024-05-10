using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomPostProcessInjector : ScriptableRendererFeature
{
    [SerializeField] private SingleBlitPostProcessRenderPassConfig chromaticAberration;
    [SerializeField] private SingleBlitPostProcessRenderPassConfig screenSpaceDistortion;
    [SerializeField] private SingleBlitPostProcessRenderPassConfig depthOfField; 
    [SerializeField] private SingleBlitPostProcessRenderPassConfig vignetteMaskedTexture;
    
    private ChromaticAberrationPass chromaticAberrationPass;
    private ScreenSpaceDistortionPass screenSpaceDistortionPass;
    private CustomDepthOfFieldPass depthOfFieldPass;
    private RadialTextureVignetteMaskedPass radialTexturePass;
    
    public override void Create()
    {
        chromaticAberrationPass = new ChromaticAberrationPass(chromaticAberration);
        screenSpaceDistortionPass = new ScreenSpaceDistortionPass(screenSpaceDistortion);
        depthOfFieldPass = new CustomDepthOfFieldPass(depthOfField);
        radialTexturePass = new RadialTextureVignetteMaskedPass(vignetteMaskedTexture);
        
        
        chromaticAberrationPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        screenSpaceDistortionPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        depthOfFieldPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        radialTexturePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(chromaticAberrationPass);
        renderer.EnqueuePass(screenSpaceDistortionPass);
        renderer.EnqueuePass(depthOfFieldPass);
        renderer.EnqueuePass(radialTexturePass);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDepthOfFieldPass : MultipleBlitPostProcessRenderPass<CustomDepthOfField>
{
    public CustomDepthOfFieldPass(SingleBlitPostProcessRenderPassConfig config) : base(config)
    {
    }

    protected override void UpdateMaterialValues(CustomDepthOfField volumeComponent)
    {
        renderingMaterial.SetFloat("_BlurDistance", volumeComponent.blurDistance.value);
        renderingMaterial.SetFloat("_FocalPoint", volumeComponent.focalPoint.value);
        renderingMaterial.SetFloat("_Aperture", volumeComponent.aperture.value);
    }

    protected override int GetIterationCount(CustomDepthOfField volumeComponent)
    {
        return volumeComponent.blurAmount.value;
    }
}

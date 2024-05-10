using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceDistortionPass : SingleBlitPostProcessRenderPass<ScreenSpaceDistortion>
{
    public ScreenSpaceDistortionPass(SingleBlitPostProcessRenderPassConfig config) : base(config)
    {
    }

    protected override void UpdateMaterialValues(ScreenSpaceDistortion volumeComponent)
    {
        renderingMaterial.SetTexture("_DistortionMap", volumeComponent.distortionNormalMap.value);
        renderingMaterial.SetFloat("_DistortionAmount", volumeComponent.distortionAmount.value);
        renderingMaterial.SetVector("_DistortionVelocity", volumeComponent.distortionVelocity.value);
    }
}

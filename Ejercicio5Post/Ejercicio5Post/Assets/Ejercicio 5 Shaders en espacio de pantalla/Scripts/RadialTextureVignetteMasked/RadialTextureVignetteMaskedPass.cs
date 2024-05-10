using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTextureVignetteMaskedPass : SingleBlitPostProcessRenderPass<RadialTextureVignetteMasked>
{
    public RadialTextureVignetteMaskedPass(SingleBlitPostProcessRenderPassConfig config) : base(config)
    {
    }

    protected override void UpdateMaterialValues(RadialTextureVignetteMasked volumeComponent)
    {
        renderingMaterial.SetFloat("_VignetteIntensity", volumeComponent.vignetteMaskIntensity.value);
        renderingMaterial.SetFloat("_VignettePower", volumeComponent.vignettePower.value);
        renderingMaterial.SetTexture("_OverlayTexture", volumeComponent.overlayTexture.value);
        renderingMaterial.SetColor("_OverlayTint", volumeComponent.overlayTextureTint.value);
        renderingMaterial.SetVector("_OverlayVelocity", volumeComponent.overlayTextureVelocity.value);
    }
}

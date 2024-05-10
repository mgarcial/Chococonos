public class ChromaticAberrationPass : SingleBlitPostProcessRenderPass<CustomChromaticAberration>
{
    public ChromaticAberrationPass(SingleBlitPostProcessRenderPassConfig config) : base(config)
    {
    }

    protected override void UpdateMaterialValues(CustomChromaticAberration volumeComponent)
    { 
        renderingMaterial.SetFloat("_RedShift", volumeComponent.redShift.value);
        renderingMaterial.SetFloat("_GreenShift", volumeComponent.greenShift.value);
        renderingMaterial.SetFloat("_BlueShift", volumeComponent.blueShift.value);
    }
}

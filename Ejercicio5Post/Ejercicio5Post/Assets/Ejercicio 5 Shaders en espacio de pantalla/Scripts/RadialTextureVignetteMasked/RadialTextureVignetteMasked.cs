using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Ejercicio #5/Textura Radial con Máscara Vignette")]
public class RadialTextureVignetteMasked : VolumeComponent, IPostProcessComponent
{
    public FloatParameter vignetteMaskIntensity = new FloatParameter(10);
    public FloatParameter vignettePower = new FloatParameter(2);
    public TextureParameter overlayTexture = new TextureParameter(null);
    public ColorParameter overlayTextureTint = new ColorParameter(Color.white);
    public Vector2Parameter overlayTextureVelocity = new Vector2Parameter(new Vector2(0,0));

    public bool IsActive() => overlayTextureTint.value.a > 0 && overlayTexture.value != null;

    public bool IsTileCompatible() => true;
}

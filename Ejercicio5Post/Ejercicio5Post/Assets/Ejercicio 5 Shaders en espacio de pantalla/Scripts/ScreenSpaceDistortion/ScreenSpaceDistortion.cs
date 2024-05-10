using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Ejercicio #5/Distorsión en Espacio de Pantalla")]
public class ScreenSpaceDistortion : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter distortionAmount = new ClampedFloatParameter(1, 0, 1);
    public TextureParameter distortionNormalMap = new TextureParameter(null);
    public Vector2Parameter distortionVelocity = new Vector2Parameter(new Vector2(0, 0));

    public bool IsActive() =>distortionAmount.value > 0;

    public bool IsTileCompatible() => true;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Ejercicio #5/Profundidad de Campo")]
public class CustomDepthOfField : VolumeComponent, IPostProcessComponent
{
    public ClampedIntParameter blurAmount = new ClampedIntParameter(0, 0, 10);
    public IntParameter blurDistance = new IntParameter(1);
    public FloatParameter focalPoint = new FloatParameter(5);
    public FloatParameter aperture = new FloatParameter(10);

    public bool IsActive() => blurAmount.value > 0 && active;

    public bool IsTileCompatible() => true;
}
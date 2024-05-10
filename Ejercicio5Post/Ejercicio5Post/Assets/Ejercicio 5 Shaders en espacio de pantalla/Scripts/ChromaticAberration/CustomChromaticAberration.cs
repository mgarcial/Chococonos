using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Ejercicio #5/Aberración Cromática")]
public class CustomChromaticAberration : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter redShift = new ClampedFloatParameter(0, 0, 1);
    public ClampedFloatParameter greenShift = new ClampedFloatParameter(0, 0, 1);
    public ClampedFloatParameter blueShift = new ClampedFloatParameter(0, 0, 1);
    
    public bool IsActive() => redShift.value > 0 || greenShift.value > 0 || blueShift.value > 0;
    public bool IsTileCompatible() => true;
}

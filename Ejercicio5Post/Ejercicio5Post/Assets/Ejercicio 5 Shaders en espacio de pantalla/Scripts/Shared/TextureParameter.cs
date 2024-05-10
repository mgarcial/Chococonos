using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class TextureParameter : VolumeParameter<Texture2D>
{
    public TextureParameter(Texture2D value, bool overrideState = false) : base(value, overrideState)
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class lensDistortion : MonoBehaviour
{

    public Material material;
    [Range(0.0f, 5.0f)]
    public float Power = 1.0f;
    [Range(0.0f, 3.0f)]
    public float fade = 1.0f;
    [Range(0.1f, 4.0f)]
    public float zoom = 1.0f;
    [Range(0, 1)]
    public int EffectSelect = 1;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_Power", Power);
            material.SetFloat("_Fade", fade);
            material.SetFloat("_Zoom", zoom);
            material.SetInt("_EffectSelect", EffectSelect);

            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}

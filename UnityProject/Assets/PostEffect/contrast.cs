using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class contrast : MonoBehaviour
{
    
    public Material material;
    [Range(0.0f, 5.0f)]
    public float Brightness=1.0f;
    [Range(0.0f, 5.0f)]
    public float Saturation=1.0f;
    [Range(0.0f, 1.5f)]
    public float Contrast=1.0f;
    [Range(0.0f, 1.0f)]
    public float Hue = 0.0f;


    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_Brightness", Brightness);
            material.SetFloat("_Saturation", Saturation);
            material.SetFloat("_Contrast", Contrast);
            material.SetFloat("_Hue", Hue);


            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Bloom : MonoBehaviour
{

    public Material material;
    [Range(0.0f, 5.0f)]
    public float bloomPower = 1.0f;
    [Range(0, 6)]
    public int iterations = 1;
    [Range(0.0f, 3.0f)]
    public float blurSpread = 0.0f;
    [Range(1, 8)]
    public int SampleDown = 1;
    [Range(0.0f, 1.0f)]
    public float blend = 1.0f;
    [Range(0.0f, 3.0f)]
    public float threshold = 1.0f;


    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            int rtw = src.width / SampleDown;
            int rth = src.height / SampleDown;
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtw, rth, 0);
            buffer0.filterMode = FilterMode.Bilinear;
            Graphics.Blit(src, buffer0,material,0);
            for (int i = 0; i < iterations; i++)
            {
                material.SetFloat("_BlurSize", 1.0f + i * blurSpread);
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtw, rth, 0);
                Graphics.Blit(buffer0, buffer1, material, 1);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtw, rth, 0);
                Graphics.Blit(buffer0, buffer1, material, 2);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }
            material.SetFloat("_BloomPower", bloomPower);
            material.SetTexture("_Bloom", buffer0);
            material.SetFloat("_Blend", blend);
            material.SetFloat("_Threshold", threshold);
            Graphics.Blit(src, dest, material, 3);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}

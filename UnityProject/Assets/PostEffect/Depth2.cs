using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth2 : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;

    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
    public Material material;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

}

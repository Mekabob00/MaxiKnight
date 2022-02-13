using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthOn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

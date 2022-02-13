using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class fog : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera myCamera;
    public Camera camera
    {
        get
        {
            if (myCamera == null)
            {
                myCamera = GetComponent<Camera>();
            }
            return myCamera;
        }
    }



    private Transform myCameraTansform;
    public Transform cameraTransform
    {
        get
        {
            if (myCameraTansform == null)
            {
                myCameraTansform = camera.transform;
            }
            return myCameraTansform;
        }
    }

    [Range(-500.0f, 1000.0f)]
    public float High = 0.0f;
    [Range(0.0f, 1.0f)]
    public float Blend = 1.0f;
    [Range(0, 1)]
    public int DirChange = 0;
    [Range(0.0f, 3.0f)]
    public float Fade = 1.0f;
    [Range(0.0f, 1000.0f)]
    public float FadeDistance = 1.0f;
    public Color Color=Color.white;
    [Range(0, 1)]
    public int HighFogOn = 1;

    [Range(0.0f, 1.0f)]
    public float DepthBlend = 1.0f;
    [Range(0.0f, 3.0f)]
    public float DepthFade = 1.0f;
    [Range(-500.0f, 1000.0f)]
    public float Depth = 0.0f;
    [Range(0.0f, 1000.0f)]
    public float DepthFadeDistance = 1.0f;
    [Range(0, 1)]
    public int DepthFogOn = 1;


    public Material material;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            Matrix4x4 frustunCorners = Matrix4x4.identity;

            float fov = camera.fieldOfView;
            float near = camera.nearClipPlane;
            float far = camera.farClipPlane;
            float aspect = camera.aspect;

            float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            Vector3 toRight = cameraTransform.right * halfHeight * aspect;
            Vector3 toTop = cameraTransform.up * halfHeight;
            Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
            float scale = topLeft.magnitude / near;

            topLeft.Normalize();
            topLeft *= scale;

            Vector3 topRight = cameraTransform.forward * near + toRight + toTop;
            topRight.Normalize();
            topRight *= scale;

            Vector3 bottomLeft = cameraTransform.forward * near - toRight - toTop;
            bottomLeft.Normalize();
            bottomLeft *= scale;

            Vector3 bottomRight = cameraTransform.forward * near + toRight - toTop;
            bottomRight.Normalize();
            bottomRight *= scale;

            frustunCorners.SetRow(0, bottomLeft);
            frustunCorners.SetRow(1, bottomRight);
            frustunCorners.SetRow(2, topRight);
            frustunCorners.SetRow(3, topLeft);
            material.SetMatrix("_FrustunCorners", frustunCorners);
            material.SetFloat("_High", High);
            material.SetFloat("_Blend", Blend);
            material.SetInt("_DirChange", DirChange);
            material.SetFloat("_Fade", Fade);
            material.SetColor("_Color", Color);
            material.SetFloat("_FadeDistance", FadeDistance);
            material.SetInt("_HighFogOn", HighFogOn);


            material.SetFloat("_DepthBlend", DepthBlend);
            material.SetFloat("_DepthFade", DepthFade);
            material.SetFloat("_Depth", Depth);
            material.SetFloat("_DepthFadeDistance", DepthFadeDistance);
            material.SetInt("_DepthFogOn", DepthFogOn);

            Graphics.Blit(src, dest, material);

        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}












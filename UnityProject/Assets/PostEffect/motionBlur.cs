using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class motionBlur : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera myCamera;
    public Camera camera
    {
        get
        {
            if(myCamera == null)
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

    [Range(0.0f, 1.0f)]
    public float Blend = 1.0f;
    [Range(0.0f, 2.0f)]
    public float BlurScale = 1.0f;



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
           
            Graphics.Blit(src, dest, material);

        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }















}

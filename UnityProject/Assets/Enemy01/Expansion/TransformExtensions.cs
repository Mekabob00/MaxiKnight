using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    //¡‚ÌXˆÊ’u‚©‚çˆø”‚ÌX•ª‚¾‚¯’Ç‰Á‚µ‚Ä‚¢‚­
    public static void AddPosX(this Transform transform,float x)
    {
        var pos = transform.position;

        pos.x += x;
        transform.position = pos;


    }





}

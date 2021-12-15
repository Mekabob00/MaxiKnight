using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    //今のX位置から引数のX分だけ追加していく
    public static void AddPosX(this Transform transform,float x)
    {
        var pos = transform.position;

        pos.x += x;
        transform.position = pos;


    }





}

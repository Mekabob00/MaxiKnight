using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    //����X�ʒu���������X�������ǉ����Ă���
    public static void AddPosX(this Transform transform,float x)
    {
        var pos = transform.position;

        pos.x += x;
        transform.position = pos;


    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RigidBodyExtensions
{
    //Player�ړ�����
    //�J�����̌������O����ɓ���
    public static void PlayerControll(this Rigidbody rb,float horizontal,float vertical,float Speed)
    {
        var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        Vector3 move = cameraForward*vertical+Camera.main.transform.right*horizontal;
        move.Normalize();

        rb.velocity = move * Speed;
    } 


    //�O�������ɐi�߂�
    public static void ForontMove(this Rigidbody rb,Transform transform,float Speed)
    {

        rb.velocity = new Vector3(-Speed, 0, 0);

    }

    //����ɐi��
    public static void BackMove(this Rigidbody rb,float Speed)
    {
        rb.velocity = new Vector3(0, 0, Speed);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��莞�ԂŖC�e���������A�܂��͎蓮����
/// </summary>
public class CanonBallGenerator : MonoBehaviour
{
    public GameObject canonBall;

    public float createCT; //�C�e�����N�[���^�C��

    private float timer; //�v�Z�p�����N�[���^�C��

    void Update()
    {
        if (GlobalData.Instance.isGameOver) { return; }

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = createCT;
            CreateCanonBall();
        }
    }

    //�C�e����
    public void CreateCanonBall()
    {
        Instantiate(canonBall);
    }
}

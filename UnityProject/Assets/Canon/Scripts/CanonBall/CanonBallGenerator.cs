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

    private float remainCreateCT; //�v�Z�p�����N�[���^�C��

    void Update()
    {
        remainCreateCT -= Time.deltaTime;
        if(remainCreateCT < 0)
        {
            remainCreateCT = createCT;
            CreateCanonBall();
        }
    }

    //�C�e����
    public void CreateCanonBall()
    {
        Instantiate(canonBall);
    }
}

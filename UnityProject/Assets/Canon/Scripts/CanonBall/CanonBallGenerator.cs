using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一定時間で砲弾自動生成、または手動生成
/// </summary>
public class CanonBallGenerator : MonoBehaviour
{
    public GameObject canonBall;

    public float createCT; //砲弾生成クールタイム

    private float timer; //計算用生成クールタイム

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

    //砲弾生成
    public void CreateCanonBall()
    {
        Instantiate(canonBall);
    }
}

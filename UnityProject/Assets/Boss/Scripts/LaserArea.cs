using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserArea : MonoBehaviour
{
    [Header("テスト レーザーダメージ")]
    public int _Damage;
    [Header("攻撃間隔")]
    public float _IntervalTIme;

    private float timeCount;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //タイマー
            timeCount += Time.deltaTime;
            if(timeCount >= _IntervalTIme)
            {
                timeCount = 0;
                other.gameObject.GetComponent<Player_Controll>()._AddDamege(_Damage);
            }
        }
    }
}

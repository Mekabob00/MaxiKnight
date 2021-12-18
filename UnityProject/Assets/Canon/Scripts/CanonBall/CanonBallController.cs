using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBallController : MonoBehaviour
{
    [Header("ランダム生成位置範囲")]
    public float randomStartPosition;
    [Header("通常速度")]
    public int normalSpeed;
    [Header("スローモーション速度")]
    public int slowSpeed;

    private int speed; //現在速度
    private bool isHit; //プレイヤーの攻撃を受ける
   
    void Start()
    {
        //ランダム初期位置
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Random.Range(-randomStartPosition, randomStartPosition));
    }

    void Update()
    {
        //カメラ範囲に入ったとき減速
        {
            if (GlobalData.Instance.isCanonAppear)
                speed = slowSpeed;
            else
                speed = normalSpeed;
        }
    }

    private void FixedUpdate()
    {
        //プレイヤーの攻撃を受けたら、反対側移動
        {
            if (isHit)
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            else
                transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //プレイヤーの攻撃を受けた時
        if (other.gameObject.CompareTag("Player Attack Area"))
        {
            isHit = true;
            GlobalData.Instance.isCanonAppear = false;
        }

        //反撃状態で敵と当たった時、敵を破壊する
        if (other.gameObject.CompareTag("Enemy") && isHit)
        {
            Destroy(other.gameObject);
        }

        //最終防衛ラインとの接触、スローモーション解除
        if (other.gameObject.CompareTag("First Defense"))
        {
            GlobalData.Instance.isCanonAppear = false;
        }


        //TODO: 拠点にダメージを与える
        if (other.gameObject.CompareTag("Castle"))
        {
            other.GetComponent<CastleBehavior>()._AddDamage(1);
            Destroy(gameObject, 0.5f);
        }
    }

    //カメラ範囲に入り、スローモーション起動
    private void OnBecameVisible()
    {
        GlobalData.Instance.isCanonAppear = true;
    }

    //カメラ範囲外、通常速度
    private void OnBecameInvisible()
    {
        GlobalData.Instance.isCanonAppear = false;
        //プレイヤーの攻撃を受けて、カメラ範囲外出た時消滅
        if (isHit)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}

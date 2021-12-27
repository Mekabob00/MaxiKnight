//更新日:12月9日 担当:下川和馬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwordControll : MonoBehaviour
{

    public bool AttakUPFlag; //防衛ライン判定




    void Start()
    {
        AttakUPFlag = false;

    }
    void Update()
    {

    }
    public void _AddDamege(int _Damage)
    {

    }
    public void OnTriggerStay(Collider other) //現在かなり強引な当たり判定の取り方です。ほかにやり方考えたい所です。
    {
        /*if (other.gameObject.tag == "2st defense")
        {
            AttakUPFlag = true;
        }
        else
        {
            AttakUPFlag = false;
        }*/
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (AttakUPFlag)
        {
            Debug.Log("現在第二防衛ラインにいます 攻撃力アップ");
            if (collision.tag == "Enemy") //現在仮タグでEnemyと付けています。随時変更していただけると助かります
            {
                Debug.Log("攻撃");
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(3); //強攻撃
            }
        }
        else
        {
            if (collision.tag == "Enemy")　//現在仮タグでEnemyと付けています。随時変更していただけると助かります
            {
                Debug.Log("弱攻撃");
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(1);//弱攻撃
            }
        }
    }

    #region コルーチン


    #endregion

}

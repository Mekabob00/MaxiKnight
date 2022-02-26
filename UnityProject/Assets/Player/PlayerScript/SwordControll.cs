//更新日:12月9日 担当:下川和馬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwordControll : MonoBehaviour
{
    [SerializeField, Tooltip("剣の攻撃力")]
    private List<float> _SwordAttackList;

    [SerializeField, Tooltip("Player")]
    private GameObject _Player;

    public bool AttakUPFlag; //防衛ライン判定


    void Start()
    {
        AttakUPFlag = false;

    }
    void Update()
    {

        //デバック用
        //Debug.Log(AttakUPFlag);


        //入力イベント
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //ColliderをONにする
            //StartCoroutine(IEColliderActive());
        }

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
        if (collision.tag == "Enemy") //現在仮タグでEnemyと付けています。随時変更していただけると助かります
        {
            int you=collision.GetComponent<JudgLaneMovement>().GetNowLane();
            int my = _Player.GetComponent<Player_Controll>().GetNowLane();

            if (you == my)
            {
                //攻撃のデータ処理
                var PlayerPower = Player_Controll.AttackBuff;
                int SwordNum = DataManager.Instance._WeaponNumberSword;
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(PlayerPower * _SwordAttackList[SwordNum]);//弱攻撃

                Debug.Log(collision.name + ".HP=>-" + PlayerPower * _SwordAttackList[SwordNum]);
            }
            

        }
    }

    #region コルーチン
    //IEnumerator IEColliderActive()
    //{
    //    //剣を振り上げてる時間
    //    yield return new WaitForSeconds(0.2f);

    //    //当たり判定をONにする
    //    _Collider.enabled = true;
    //    _Collider2.enabled = true;

    //    //剣を振り下ろす時間
    //    yield return new WaitForSeconds(0.2f);

    //    //当たり判定をOFFにする
    //    _Collider.enabled = false;
    //    _Collider2.enabled = false;

    //    yield break;
    //}


    #endregion

}

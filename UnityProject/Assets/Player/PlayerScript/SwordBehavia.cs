using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavia : MonoBehaviour
{
    [SerializeField, Tooltip("攻撃力")]
    private float _AttackPower;

    [SerializeField, Tooltip("Player")]
    private GameObject _Player = null;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Enemy") //現在仮タグでEnemyと付けています。随時変更していただけると助かります
        {
            int you = collision.GetComponent<JudgLaneMovement>().GetNowLane();
            int my = _Player.GetComponent<Player_Controll>().GetNowLane();

            if (you == my)
            {
                //攻撃のデータ処理
                var PlayerPower = Player_Controll.AttackBuff;
                int SwordNum = DataManager.Instance._WeaponNumberSword;
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(PlayerPower * _AttackPower);

                Debug.Log(collision.name + ".HP=>-" + PlayerPower * _AttackPower);
            }

        }


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavia : MonoBehaviour
{
    [SerializeField, Tooltip("�U����")]
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
        if (collision.tag == "Enemy") //���݉��^�O��Enemy�ƕt���Ă��܂��B�����ύX���Ă���������Ə�����܂�
        {
            int you = collision.GetComponent<JudgLaneMovement>().GetNowLane();
            int my = _Player.GetComponent<Player_Controll>().GetNowLane();

            if (you == my)
            {
                //�U���̃f�[�^����
                var PlayerPower = Player_Controll.AttackBuff;
                int SwordNum = DataManager.Instance._WeaponNumberSword;
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(PlayerPower * _AttackPower);

                Debug.Log(collision.name + ".HP=>-" + PlayerPower * _AttackPower);
            }

        }


    }

}

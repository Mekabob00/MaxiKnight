//�X�V��:12��9�� �S��:����a�n
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwordControll : MonoBehaviour
{
    [SerializeField, Tooltip("���̍U����")]
    private List<float> _SwordAttackList;

    [SerializeField, Tooltip("Player")]
    private GameObject _Player;

    public bool AttakUPFlag; //�h�q���C������


    void Start()
    {
        AttakUPFlag = false;

    }
    void Update()
    {

        //�f�o�b�N�p
        //Debug.Log(AttakUPFlag);


        //���̓C�x���g
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Collider��ON�ɂ���
            //StartCoroutine(IEColliderActive());
        }

    }
    public void _AddDamege(int _Damage)
    {

    }
    public void OnTriggerStay(Collider other) //���݂��Ȃ苭���ȓ����蔻��̎����ł��B�ق��ɂ����l���������ł��B
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
        if (collision.tag == "Enemy") //���݉��^�O��Enemy�ƕt���Ă��܂��B�����ύX���Ă���������Ə�����܂�
        {
            int you=collision.GetComponent<JudgLaneMovement>().GetNowLane();
            int my = _Player.GetComponent<Player_Controll>().GetNowLane();

            if (you == my)
            {
                //�U���̃f�[�^����
                var PlayerPower = Player_Controll.AttackBuff;
                int SwordNum = DataManager.Instance._WeaponNumberSword;
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(PlayerPower * _SwordAttackList[SwordNum]);//��U��

                Debug.Log(collision.name + ".HP=>-" + PlayerPower * _SwordAttackList[SwordNum]);
            }
            

        }
    }

    #region �R���[�`��
    //IEnumerator IEColliderActive()
    //{
    //    //����U��グ�Ă鎞��
    //    yield return new WaitForSeconds(0.2f);

    //    //�����蔻���ON�ɂ���
    //    _Collider.enabled = true;
    //    _Collider2.enabled = true;

    //    //����U�艺�낷����
    //    yield return new WaitForSeconds(0.2f);

    //    //�����蔻���OFF�ɂ���
    //    _Collider.enabled = false;
    //    _Collider2.enabled = false;

    //    yield break;
    //}


    #endregion

}

//�X�V��:12��9�� �S��:����a�n
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwordControll : MonoBehaviour
{

    public bool AttakUPFlag; //�h�q���C������




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
        if (AttakUPFlag)
        {
            Debug.Log("���ݑ��h�q���C���ɂ��܂� �U���̓A�b�v");
            if (collision.tag == "Enemy") //���݉��^�O��Enemy�ƕt���Ă��܂��B�����ύX���Ă���������Ə�����܂�
            {
                Debug.Log("�U��");
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(3); //���U��
            }
        }
        else
        {
            if (collision.tag == "Enemy")�@//���݉��^�O��Enemy�ƕt���Ă��܂��B�����ύX���Ă���������Ə�����܂�
            {
                Debug.Log("��U��");
                var SecondDamege = collision.gameObject.GetComponent<IPlayerDamege>();
                SecondDamege._AddDamege(1);//��U��
            }
        }
    }

    #region �R���[�`��


    #endregion

}

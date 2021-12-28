using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behaviour : MonoBehaviour, IPlayerDamege
{
    #region SeliarizFild
    [SerializeField, Tooltip("�ʏ�̈ړ����x")]
    private float _MoveSpeed = 10.0f;

    [SerializeField, Tooltip("�X���[���[�V�������̈ړ����x")]
    private float _SlowMoveSpeed = 1.0f;

    [SerializeField, Tooltip("�̗�,int�^")]
    private int _HP;

    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField, Tooltip("Renderer")]
    private Renderer _Renderer = null;

    [SerializeField, Tooltip("�O���[�o���f�[�^")]
    private GameObject _GlobalDataObject = null;

    #endregion

    #region Defalut


    private GlobalData _GlobalData = null;

    private float _HighPos = 1.0f;

    //Flag
    private bool _IsAddDamageEffect = false;
    private bool _IsMoveActive = false;

    #endregion


    #region Unity function
    void Start()
    {
        _IsAddDamageEffect = false;
        _IsMoveActive = true;

        //�O���[�o���f�[�^�̃R���|�[�l���g���擾
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();
    }

    void Update()
    {
        if (_IsAddDamageEffect)//�_���[�W��H��������Effect�Œ�
        {
            if (this.transform.position.y < _HighPos)
            {
                //���̍����̂ɂȂ�����Effect���I��������
                _IsAddDamageEffect = false;
                _RigidBody.useGravity= false;

                //�ʒu��␳
                Vector3 pos = this.transform.position;
                pos.y = _HighPos;
                this.transform.position = pos;
            }
            return;
        }


        if (_IsMoveActive)
        {

            //�O���i��
            if (!_GlobalData.isCanonAppear)//�ʏ�
            {
                _RigidBody.ForontMove(this.transform, _MoveSpeed);
            }
            else
            {
                //�X���[�ړ�
                _RigidBody.ForontMove(this.transform, _SlowMoveSpeed);
            }

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //------------------------------------------------------
        if (other.CompareTag("Castle"))
        {
            other.GetComponent<CastleBehavior>()._AddDamage(1);
            Destroy(gameObject);
        }
        //------------------------------------------------------

        
    }

    private void OnTriggerExit(Collider other)
    {

        //�ŏI�h�q���C���𒴂����瓮�����~�߂�Stop
        if (other.tag == "Stage")
        {
            _IsMoveActive = false;
        }
        


    }


    #endregion

    #region public function

    /// <summary>
    /// �_���[�W�̉��Z
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void _AddDamege(int _Damege)
    {
        if (_IsAddDamageEffect || !_IsMoveActive)//�_���[�W��^���Ȃ�
        {
            return;
        }

        var after = _HP - _Damege;

        //�̗͂�0�Ȃ�
        if (after <= 0)
        {
            //��
            Destroy(this.gameObject);
            return;
        }
        else if (after != _HP)//�_���[�W���󂯂���
        {

            //�_���[�W���󂯂����̏���
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }

    #endregion

    #region private function


    #endregion

    #region Coroutine
    IEnumerator AddDamageMove()
    {
        //�d�͂�ON�ɂ���
        _RigidBody.useGravity= true;
        //��яオ��
        _RigidBody.AddForce(new Vector3(0,300.0f,0));

        //[TODO]
        //�����ɑ΂��ĐF��ύX����

        _IsAddDamageEffect = true;

        yield break;
    }

    




    #endregion


}

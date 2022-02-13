using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Behaviour : MonoBehaviour, IPlayerDamege
{
    #region SeliarizFild
    [SerializeField, Tooltip("�ʏ�̈ړ����x")]
    private float _MoveSpeed = 10.0f;

    [SerializeField, Tooltip("�X���[���[�V�������̈ړ����x")]
    private float _SlowMoveSpeed = 1.0f;

    [SerializeField, Tooltip("�̗�,float�^")]
    private float _HP;

    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField, Tooltip("Renderer")]
    private Renderer _Renderer = null;

    [SerializeField, Tooltip("�O���[�o���f�[�^")]
    private GameObject _GlobalDataObject = null;
    [SerializeField, Tooltip("�G�t�F�N�g")]
    private GameObject Effct;

    #endregion

    #region Defalut


    private GlobalData _GlobalData = null;

    private float _HighPos = 1.0f;

    public float span = 3f;
    private float currentTime = 0f;

    //Flag
    private bool _IsAddDamageEffect = false;
    public bool _IsMoveActive = false;
    private bool _IsAttackFlag = false;

    #endregion

    //��
    [SerializeField]
    private GameObject castle;
    [SerializeField]
    private GameObject Enemy;
    public Vector3 PlayerPosition;
    private Vector3 EnemyPosition;

    private float dis;
    [SerializeField]
    private float FocusSpeed;


    #region Unity function
    private void Start()
    {
        _IsAddDamageEffect = false;
        _IsMoveActive = true;
        _IsAttackFlag = false;
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();
    }
    void Update()
    {
        PlayerFocus();
        Debug.Log(dis);
      //  IsAttackFlag();
        if (_IsAddDamageEffect)//�_���[�W��H��������Effect�Œ�
        {
            if (this.transform.position.y < _HighPos)
            {
                //���̍����̂ɂȂ�����Effect���I��������
                _IsAddDamageEffect = false;
                _RigidBody.useGravity = false;

                //�ʒu��␳
                Vector3 pos = this.transform.position;
                pos.y = _HighPos;
                this.transform.position = pos;
            }
            return;
        }
        if (_IsMoveActive && !_IsAttackFlag)
        { 
              Enemy.transform.position= Vector3.MoveTowards(transform.position, castle.transform.position,2*Time.deltaTime);
        }
        else
        {
            _RigidBody.ForontMove(this.transform, 0.0f);
            IsAttack();
        }
    }
    #endregion

    #region public function

    /// <summary>
    /// �_���[�W�̉��Z
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    public void _AddDamege(float _Damege)
    {
        if (_IsAddDamageEffect)//�_���[�W��^���Ȃ�
        {
            return;
        }

        var after = _HP - _Damege;

        //�̗͂�0�Ȃ�
        if (after<= 0)
        {
            //��
            Instantiate(Effct, transform.position, transform.rotation);
            Destroy(this.gameObject);
            return;
        }
        else if (after != _HP)//�_���[�W���󂯂���
        {
            Instantiate(Effct, transform.position, transform.rotation);
            //�_���[�W���󂯂����̏���
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }
    public void OnCollisionEnter(Collision collision)
    {
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //���݉��^�O��Enemy�ƕt���Ă��܂��B�����ύX���Ă���������Ə�����܂�
        {
            Debug.Log("�U��");
            var Enemy4Damege = other.gameObject.GetComponent<IPlayerDamege>();
            Enemy4Damege._AddDamege(3); //���U��

        }
    }

    #endregion

    #region private function

    private void PlayerFocus()
    {
        // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o����Quaternion(��]�l)���擾
        Vector3 vector3 = castle.transform.position - this.transform.position;
      
        Quaternion quaternion = Quaternion.LookRotation(vector3);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, quaternion, Time.deltaTime * FocusSpeed);
    }
    /*  private void IsAttackFlag()
      {
          PlayerPosition = castle.transform.position;
          EnemyPosition = Enemy.transform.position;
          dis = Vector3.Distance(PlayerPosition, EnemyPosition);
          if (dis < 5.0f)
          {
              _IsAttackFlag = true;
              _IsMoveActive = false;
          }
          else
          {
              _IsAttackFlag = false;
              _IsMoveActive = true;
          }
          Debug.Log("����" + dis);
      }*/
    private void IsAttack()
    {
        currentTime += Time.deltaTime;

        if (currentTime > span)
        {
            Debug.LogFormat("{0}�b�o��", span);
            //Player_Controll.instance.HP--;
            Debug.Log("��ɍU��");
            currentTime = 0f;
        }
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

    #region �R���[�`��
    IEnumerator AddDamageMove()
    {
        //�d�͂�ON�ɂ���
      //  _RigidBody.useGravity = true;
        //��яオ��
     //   _RigidBody.AddForce(new Vector3(0, 300.0f, 0));

        //[TODO]
        //�����ɑ΂��ĐF��ύX����

        _IsAddDamageEffect = true;

        yield break;
    }
    #endregion
}

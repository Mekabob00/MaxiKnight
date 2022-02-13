using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Behaviour : MonoBehaviour, IPlayerDamege
{
    #region SeliarizFild
    [SerializeField, Tooltip("�ʏ�̈ړ����x")]
    private float _MoveSpeed = 10.0f;

    [SerializeField, Tooltip("�X���[���[�V�������̈ړ����x")]
    private float _SlowMoveSpeed = 1.0f;

    [SerializeField, Tooltip("�̗�,int�^")]
    private float _HP;

    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField, Tooltip("Renderer")]
    private Renderer _Renderer = null;

    [SerializeField, Tooltip("�O���[�o���f�[�^")]
    private GameObject _GlobalDataObject = null;
    [SerializeField, Tooltip("Enemy�I�u�W�F�N�g�擾")]
    private GameObject Enemy3;
    [SerializeField, Tooltip("Enemy3�U����")]
    private int _Damage;

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
    public Vector3 castlePosition;
    private Vector3 EnemyPosition;

   
    [SerializeField, Tooltip("�_���[�W���ʉ�")]
    private AudioClip DamegeSE;

    [SerializeField, Tooltip("�U��SE")]
    private AudioClip AttackSE;

    [SerializeField, Tooltip("���SSE")]
    private AudioClip DieSE;
    [SerializeField, Tooltip("�G�t�F�N�g")]
    private GameObject Effect;
    [SerializeField,Tooltip("�A�j���[�V����")]
    private Animator EnemyAnimator;

    private float dis;

    #region Unity function
    private void Start()
    {
        _Damage = 1;
        _IsAddDamageEffect = false;
        _IsMoveActive = true;
        _IsAttackFlag = false;
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
        if (after<=0)
        {
            //��
            EnemyAnimator.SetTrigger("Damege");
            Instantiate(Effect, transform.position, transform.rotation);
            Destroy(Enemy3);
            return;
        }
        else if (after != _HP)//�_���[�W���󂯂���
        {
            EnemyAnimator.SetTrigger("Damege");
            Instantiate(Effect, transform.position, transform.rotation);
            EnemyAttackManeger.instance.PlaySE(DamegeSE);
            //�_���[�W���󂯂����̏���
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }
    public void AnimationStop()
    {
        _RigidBody.constraints = RigidbodyConstraints.FreezePositionX;

    }
    public void AnimationStart()
    {
        _RigidBody.constraints = RigidbodyConstraints.None;
        _RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    #endregion

    #region private function
    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag== "Castle")
        {
            castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
            Destroy(Enemy3);
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
    /*    //�d�͂�ON�ɂ���
        _RigidBody.useGravity = true;
        //��яオ��
        _RigidBody.AddForce(new Vector3(0, 300.0f, 0));

        //[TODO]
        //�����ɑ΂��ĐF��ύX����

        _IsAddDamageEffect = true;
    */
        yield break;
    }






    #endregion
}

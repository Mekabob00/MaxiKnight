using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Behaviour : MonoBehaviour, IPlayerDamege
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

    [SerializeField, Tooltip("�_���[�W���ʉ�")]
    private AudioClip DamegeSE;

    [SerializeField, Tooltip("�U��SE")]
    private AudioClip AttackSE;

    [SerializeField, Tooltip("���SSE")]
    private AudioClip DieSE;
    [SerializeField, Tooltip("Enemy2�U����")]
    private int _Damage;
    [SerializeField, Tooltip("�G�t�F�N�g")]
    private GameObject Effect;
    [SerializeField, Tooltip("���S���̃G�t�F�N�g")]
    private GameObject DidEffect;
    [SerializeField, Tooltip("�A�C�e��")]
    private GameObject Item;
    [SerializeField, Tooltip("�e���˃|�C���g")]
    private Transform ShotPoint;
    [SerializeField, Tooltip("�e")]
    private GameObject Shot;
    #endregion

    #region Defalut

    private Animator EnemyAnimator;

    private GlobalData _GlobalData = null;

    private float _HighPos = 1.0f;

    public float span = 3f;
    private float currentTime = 0f;

    //Flag
    public bool _IsAddDamageEffect = false;
    public bool _IsMoveActive = false;
    public bool _IsAttackFlag = false;

    #endregion

    //��
    [SerializeField]
    private GameObject castle;
    [SerializeField]
    private GameObject Enemy;
    public Vector3 castlePosition;
    private Vector3 EnemyPosition;

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
        EnemyAnimator = GetComponent<Animator>();
        
        
    }
    void Update()
    {
        Enemy.transform.localScale = new Vector3(2, 2, 2);
        IsAttackFlag();
       /*if (_IsAddDamageEffect)//�_���[�W��H��������Effect�Œ�
        {
            if (this.transform.position.y < _HighPos)
            {
                //���̍����̂ɂȂ�����Effect���I��������
                _IsAddDamageEffect = false;
                _RigidBody.useGravity = false;

                //�ʒu��␳
                Vector3 pos = this.transform.position;
               // pos.y = _HighPos;
                this.transform.position = pos;
            }
            return;
        }*/
        if (_IsMoveActive && !_IsAttackFlag)
        {
            EnemyAnimator.SetBool("Walk", true);
            //�O���i��
            if (!_GlobalData.isCanonAppear)//�ʏ�
            {
                _RigidBody.ForontMove(this.transform, _MoveSpeed);
            }
        }
        else
        {
            EnemyAnimator.SetBool("Walk", false);
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
    

        var after =_HP - _Damege;

        //�̗͂�0�Ȃ�
        if (after<=0)
        {
            //��
            EnemyAttackManeger.instance.PlaySE(DieSE);
            Destroy(this.gameObject);
            EnemyAnimator.SetTrigger("Damege");
            Instantiate(DidEffect, transform.position, transform.rotation);
            Instantiate(Item, transform.position, transform.rotation);
            return;
        }
        else if (after != _HP)//�_���[�W���󂯂���
        {
            EnemyAttackManeger.instance.PlaySE(DamegeSE);
            EnemyAnimator.SetTrigger("Damege");
            Instantiate(Effect, transform.position, transform.rotation);
            //�_���[�W���󂯂����̏���
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }

    #endregion

    #region private function

    private void IsAttackFlag()
    {
        castlePosition = castle.transform.position;
        EnemyPosition = Enemy.transform.position;
        dis = Vector3.Distance(castlePosition, EnemyPosition);
        if (dis < 35.0f)
        {
            _IsAttackFlag = true;
            _IsMoveActive = false;
        }
        Debug.Log("����" + dis);
    }
    private void IsAttack()
    {
        if (DataManager.Instance._CastleHP >= 1)
        {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {
                EnemyAnimator.SetTrigger("Attack");
                Instantiate(Shot, ShotPoint.position, ShotPoint.rotation);
                currentTime = 0f;
            }
        }
        else if(DataManager.Instance._CastleHP<=0)
        {
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

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
        /*   //�d�͂�ON�ɂ���
           _RigidBody.useGravity = true;
           //��яオ��
           _RigidBody.AddForce(new Vector3(0, 300.0f, 0));
        */
        //EnemyAnimator.SetTrigger("Damege");

        //[TODO]
        //�����ɑ΂��ĐF��ύX����

        _IsAddDamageEffect = true;

        yield break;
    }





}
#endregion

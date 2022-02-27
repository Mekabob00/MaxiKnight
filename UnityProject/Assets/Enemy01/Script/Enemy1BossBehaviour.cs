using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1BossBehaviour : MonoBehaviour, IPlayerDamege
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

    [SerializeField, Tooltip("�_���[�W���ʉ�")]
    private AudioClip DamegeSE;

    [SerializeField, Tooltip("�U��SE")]
    private AudioClip AttackSE;

    [SerializeField, Tooltip("���SSE")]
    private AudioClip DieSE;
    [SerializeField, Tooltip("Enemy1�U����")]
    private int _Damage;
    [SerializeField, Tooltip("�G�t�F�N�g")]
    private GameObject Effect;
    [SerializeField, Tooltip("���S���̃G�t�F�N�g")]
    private GameObject DidEffect;
    [SerializeField, Tooltip("�A�C�e������")]
    private GameObject Item;
    [SerializeField, Tooltip("�N���A�E�C���h�E")]
    private GameObject panel;
    [SerializeField, Tooltip("�I�[��")]
    private GameObject Aura;
    #endregion

    #region Defalut

    private Animator EnemyAnimator;

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

    private float dis;

 

    #region Unity function
    private void Start()
    {
        panel.SetActive(false);
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
        IsAttackFlag();
      
        if (_IsMoveActive && !_IsAttackFlag)
        {
            EnemyAnimator.SetBool("Walk", true);
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
    

        var after = _HP - _Damege;

        //�̗͂�0�Ȃ�
        if (after <= 0)
        {
            //��
            EnemyAttackManeger.instance.PlaySE(DieSE);
            Destroy(this.gameObject);
            EnemyAnimator.SetTrigger("Damege");
            EnemyDestroy("Enemy");
            panel.SetActive(true);
            Instantiate(DidEffect, transform.position, transform.rotation);
            Instantiate(Item, transform.position, transform.rotation);
            return;
        }
        else if (after != _HP)//�_���[�W���󂯂���
        {
            EnemyAnimator.SetTrigger("Damege");
            EnemyAttackManeger.instance.PlaySE(DamegeSE);
            Instantiate(Effect, transform.position, transform.rotation);
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
    public void EnemyDestroy(string tag_Enemyname)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag_Enemyname);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
    #endregion

    #region private function

    private void IsAttackFlag()
    {
        castlePosition = castle.transform.position;
        EnemyPosition = Enemy.transform.position;
        dis = Vector3.Distance(castlePosition, EnemyPosition);
        if (dis < 10.0f)
        {
            _IsAttackFlag = true;
            _IsMoveActive = false;
        }
        //  Debug.Log("����" + dis);
    }
    private void IsAttack()
    {
        if (DataManager.Instance._CastleHP >= 1)
        {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {
                EnemyAttackManeger.instance.PlaySE(AttackSE);
                EnemyAnimator.SetTrigger("Attack");
                castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
                Debug.Log("��ɍU��");
                currentTime = 0f;
            }
        }
        else if (DataManager.Instance._CastleHP <= 0)
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
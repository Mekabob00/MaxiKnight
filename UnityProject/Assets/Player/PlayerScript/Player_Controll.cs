//�X�V�� 12��10�� �S��:����a�n
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controll : MonoBehaviour,IPlayerDamege
{
    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField]
    float PlayerWlakSpeed;
    [SerializeField]
    private float applySpeed = 0.2f;       // ��]�̓K�p���x

    [SerializeField]
    public int PlayerAttackPoint;

    [SerializeField, Tooltip("�����蔻��")]
    private Collider _HitBase;

    [SerializeField, Tooltip("����̃I�u�W�F�N�g")]
    private GameObject _SwordObject = null;

    [SerializeField, Tooltip("�U�����x")]
    private float _AttackSpeed = 1.0f;

    [SerializeField, Tooltip("�A���U���̗P�\����")]
    private float _CombAttackGraceTime = 1.0f;

    [SerializeField, Tooltip("�U���p�^�[���̎��")]
    private int _AttackPatternNum = 3;



    [SerializeField, Tooltip("����̑傫��")]
    private float _AvoidanceValue = 50.0f;

    [SerializeField, Tooltip("�ő僉�C�t")]
    private float _MAXHP = 100;

    private Vector3 latestPosition;
    private Animator PlayerAttackAnimator;
    public Collider _SwordCollider;
    private GunControll _GunControll;
    
    private int _AttackType = 0;
    private float _NextActionTime;


    private bool _IsGaraceTime = false;
    private bool _IsAvoid = false;
    private bool _IsAttack = false;

    public float SmoothTime = 2f;
    public float Speed = 1f;
    public float JourneyLength = 10f;
    private float _StartTime = 0;
    private float _NowHP;


    private Vector3 AvoidPos_Start = new Vector3();
    private Vector3 AvoidPos_End = new Vector3();


    public static float AttackBuff = 1;


    private void Awake()
    {//�X�^�[�g�֐��O�ɉ������������鎞�p
    }
    void Start()
    {
        //animtion
        PlayerAttackAnimator = GetComponent<Animator>();
        PlayerAttackAnimator.SetFloat("AttackSpeed", 1);

        _GunControll = GetComponent<GunControll>();

        latestPosition = transform.position;
        PlayerAttackPoint = 10;
        _AttackType = 0;
        _StartTime = Time.time;
        _NowHP = _MAXHP;
        AttackBuff = DataManager.Instance._PlayerAttackBuff;


        //����̓����蔻��̐ݒ�
        _SwordCollider = _SwordObject.GetComponent<BoxCollider>();
        _SwordCollider.enabled = false;//�R���C�_�[��OFF

        _NextActionTime = _CombAttackGraceTime;

        _IsGaraceTime = false;
        _IsAvoid = false;
        _IsAttack = false;
    }
    void Update()
    {
        

        if (_IsAvoid)
        {
            Avoidance();
            return;
        }

        PlayerWalk();//�v���C���[�ړ��֐��Ăяo��
        PlayerAttackAnimation();//Z�L�[�����������ɃA�j���[�V������������
        PlayerAvoidance();
        PlayercolliderONOFF();

        //Player�̍U���͔��f
        AttackBuff = DataManager.Instance._PlayerAttackBuff;
    }


    float CalcMoveRatio()
    {
        var distCovered = (Time.deltaTime - _StartTime) * 5;
        return distCovered / JourneyLength;
    }

    private void PlayercolliderONOFF()
    {
        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("OnDamage")||!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _HitBase.enabled = true;
        }


    }

    private void PlayerWalk()
    {//�v���C���[�̈ړ��֐�
        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerWlakSpeed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * PlayerWlakSpeed;

        bool IsAttack = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

        //�U�����ɓ������Ȃ�
        if (!IsAttack)
        {
            transform.position = new Vector3(
            transform.position.x + dx, 0.5f, transform.position.z + dz
        );
        }
        

        Vector3 diff = transform.position - latestPosition;   //���݋���ʒu����ǂ̕����ɐi�񂾂�
        latestPosition = transform.position;
        diff.y = 0;//y�����͖�������
        if (diff.magnitude > 0.01f)
        {
            if (!_IsAvoid)
            {
                transform.rotation = Quaternion.LookRotation(diff); //�v���C���[�̌����ύX
                PlayerAttackAnimator.SetBool("Run", true);
            }
            
        }
        else
        {
            PlayerAttackAnimator.SetBool("Run", false);
        }





    }

    /// <summary>
    /// ���
    /// </summary>
    private bool PlayerAvoidance()
    {
        //�U�����[�V������
        if (!_IsGaraceTime && PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return false;
        }

        if (Input.GetKeyDown(KeyCode.C) && !_IsAvoid)
        {

            PlayerAttackAnimator.SetTrigger("Avoid");

            _IsAvoid = true;
            AvoidPos_Start = transform.position;
            AvoidPos_End = transform.position + transform.forward * _AvoidanceValue;
            return true;
        }

        return false;
    }

    private void Avoidance()
    {
        transform.position = Vector3.Lerp(transform.position, AvoidPos_End, CalcMoveRatio());

        if (PlayerAttackAnimator.IsInTransition(0))//�J�ڒ�
        {
            return;
        }

        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _IsAvoid = false;
        }

    }

    public void PlayerAttackAnimation()
    {

        
        if (PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("run") || PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            //���̍U��
            PlayerAttackMove();

            //�e�ł̍U��
            _GunControll.GunAttack();
        }
       


    }
    public void _AddDamege(float _Damege)
    {
        _NowHP -= _Damege;
        PlayerAttackAnimator.SetTrigger("OnDamage");
        _HitBase.enabled = false;
    }

    public void AttackAnimEnd()
    {
        _IsAttack = false;
        PlayerAttackAnimator.SetFloat("AttackSpeed", 0);
        //0.5�b��Collider��OFF
        _SwordCollider.enabled = false;

        //�P�\���Ԃ̃��Z�b�g
        _NextActionTime = 0;

        //�P�\���Ԃ̎n�܂�
        _IsGaraceTime = true;
        StartCoroutine(AttackColliderTime());

        PlayerAttackAnimator.SetBool("IsAttack", false);
    }

    //���ł̍U��
    bool PlayerAttackMove()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_SwordCollider.enabled)
        {
            _SwordCollider.enabled = true;
          
            PlayerAttackAnimator.SetTrigger("Attack");
            
            PlayerAttackAnimator.Play("Attack", 0, 0);
            return true;
        }
        return false;
    }

    IEnumerator AttackColliderTime()
    {
        float time = 0;

        while (time <= 0.5f)//�P�\����
        {
            //�U���C�x���g������������
            if (PlayerAttackMove())
            {
                //�A���U���̃J�E���g
                _AttackType++;
                if (_AttackType >= 3)
                {
                    _AttackType = 0;
                }
                PlayerAttackAnimator.SetFloat("AttackType", _AttackType);
                PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed);

                yield break;
            }
            else if (PlayerAvoidance())//���
            {
                break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed);

        _AttackType = 0;
        PlayerAttackAnimator.SetFloat("AttackType", _AttackType);
        yield break;
    }

}


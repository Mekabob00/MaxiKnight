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

    [SerializeField, Tooltip("����̃I�u�W�F�N�g")]
    private GameObject _SwordObject = null;

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
    private Collider _SwordCollider;
    private GunControll _GunControll;
    
    private int _AttackType = 0;
    private float _NextActionTime;


    private bool _IsGaraceTime = false;
    private bool _IsAvoid = false;

    public float SmoothTime = 2f;
    public float Speed = 1f;
    public float JourneyLength = 10f;
    private float _StartTime = 0;
    private float _NowHP;


    private Vector3 AvoidPos_Start = new Vector3();
    private Vector3 AvoidPos_End = new Vector3();

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


        //����̓����蔻��̐ݒ�
        _SwordCollider = _SwordObject.GetComponent<BoxCollider>();
        _SwordCollider.enabled = false;//�R���C�_�[��OFF

        _NextActionTime = _CombAttackGraceTime;

        _IsGaraceTime = false;
        _IsAvoid = false;
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
    }


    float CalcMoveRatio()
    {
        var distCovered = (Time.deltaTime - _StartTime) * 5;
        return distCovered / JourneyLength;
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
    private void PlayerAvoidance()
    {
        //�U�����[�V������
        if (!_IsGaraceTime && PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.C) && !_IsAvoid)
        {
            
            PlayerAttackAnimator.SetTrigger("Avoid");
            
            _IsAvoid = true;
            AvoidPos_Start = transform.position;
            AvoidPos_End =transform.position + transform.forward * _AvoidanceValue;
        }

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

        //���Ԃ��v�Z
        _NextActionTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Z)&&!_SwordCollider.enabled)
        {

            //�P�\���Ԃ̏I���
            _IsGaraceTime = false;

            //�A���U��
            if (_NextActionTime <= _CombAttackGraceTime)//�P�\���ԓ��ɃC�x���g��������
            {
                PlayerAttackAnimator.SetFloat("AttackSpeed", 1);

                ++_AttackType;

                if (_AttackType >= 3)
                {
                    _AttackType = 0;
                }
            }
            else
            {
                //�P�\���Ԃ𒴂����Type�P�̃A�j���[�V�����ɂ���
                _AttackType = 0;

            }

            //AnimetorParameter�ɔ��f
            PlayerAttackAnimator.SetFloat("AttackType", _AttackType);

            //�A�j���[�V�������ŏ�����Đ�
            PlayerAttackAnimator.Play("Attack",0,0);

            //�U���A�j���[�V�����Đ�
            PlayerAttackAnimator.SetBool("IsAttack",true);

            StartCoroutine(AttackColliderTime());
        }


        if(_NextActionTime >= _CombAttackGraceTime)//�P�\���Ԃ𒴂�����
        {
            //�A�j���[�V�����̍Đ�
            PlayerAttackAnimator.SetFloat("AttackSpeed", 1);

            //�A�j���[�V�����̑J��
            PlayerAttackAnimator.SetBool("IsAttack", false);

            //�P�\���Ԃ̏I���
            _IsGaraceTime = false;
        }

        if(_IsGaraceTime)//�P�\���Ԃł���Ƃ�
        {
            //�A�j���[�V�����̒�~
            PlayerAttackAnimator.SetFloat("AttackSpeed", 0);
        }

        _GunControll.GunAttack();


    }
    public void _AddDamege(int _Damege)
    {


    }

    IEnumerator AttackColliderTime()
    {
        yield return new WaitForSeconds(0.3f);

        //�����蔻���ON�ɂ���
        _SwordCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);//�U���I��

        //0.5�b��Collider��OFF
        _SwordCollider.enabled = false;

        //�P�\���Ԃ̃��Z�b�g
        _NextActionTime = 0;

        //�P�\���Ԃ̎n�܂�
        _IsGaraceTime = true;

        yield break;
    }

}


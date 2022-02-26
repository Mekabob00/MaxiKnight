//�X�V�� 12��10�� �S��:����a�n
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controll : MonoBehaviour, IPlayerDamege
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

    [SerializeField, Tooltip("����̃����[�h����")]
    private float AvoidReLoadTime = 3.0f;

    [SerializeField, Tooltip("���[���̍��W")]
    private List<float> _LanePosList;

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
    private bool _IsLaneChamge = false;

    public float SmoothTime = 2f;
    public float Speed = 1f;
    public float JourneyLength = 10f;
    private float _StartTime = 0;
    private float _NowHP;
    private int _NowLane;
    private float Input_tmp = 0;
    private float InputZ_tmp = 0;
    private Vector3 AvoidPos_Start = new Vector3();
    private Vector3 AvoidPos_End = new Vector3();
    private float AvoidAngle = 0; 

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

        _NowLane = 0;//����

        //����̓����蔻��̐ݒ�
        _SwordCollider = _SwordObject.GetComponent<BoxCollider>();
        _SwordCollider.enabled = false;//�R���C�_�[��OFF

        _NextActionTime = _CombAttackGraceTime;

        Input_tmp = 1;
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
        ChangeLaneMove();


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
        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("OnDamage") || !PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _HitBase.enabled = true;
        }


    }

    private void PlayerWalk()
    {//�v���C���[�̈ړ��֐�
        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerWlakSpeed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * PlayerWlakSpeed;
        
        bool IsAttack = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        bool IschangeLane = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChangeLane");


        //�U�����ɓ������Ȃ�
        if (!IsAttack&&!IschangeLane)
        {
            transform.position = new Vector3(
            transform.position.x + dx, 0.5f, _LanePosList[_NowLane]) ;
        }

        //�����f�[�^��ۑ�
        if (dx != 0)
        {
            Input_tmp = dx;
        }


        Vector3 diff = transform.position - latestPosition;   //���݋���ʒu����ǂ̕����ɐi�񂾂�
        latestPosition = transform.position;
        diff.y = 0;//y�����͖�������
        diff.z = 0;
        if (diff.magnitude > 0.01f)
        {
            if (!_IsAvoid||!_IsLaneChamge)
            {
                transform.rotation = Quaternion.LookRotation(diff); //�v���C���[�̌����ύX
                PlayerAttackAnimator.SetBool("Run", true);
            }

        }
        else
        {
            PlayerAttackAnimator.SetBool("Run", false);
        }

        //���[���ړ�
        if (!_IsLaneChamge)
        {
            ChangeLaneFun(dz);
        }


    }

    /// <summary>
    /// ���[���ړ�
    /// </summary>
    /// <param name="inp">���͏��</param>
    void ChangeLaneFun(float inp)
    {
        if (inp != 0)
        {
            if (_NowLane == 0)
            {
                if (inp < 0)
                {
                    return;
                }
            }
            else
            {
                if (inp > 0)
                {
                    return;
                }
            }
            //�A�j���[�V�����Đ�
            PlayerAttackAnimator.SetTrigger("ChangeLane");

            InputZ_tmp = inp;
        }


       

    }

    /// <summary>
    /// ���[���ړ�����Ƃ��̓���
    /// </summary>
    void ChangeLaneMove()
    {
        //2�_�Ԃ̋�����������
        float distance = _LanePosList[1] - _LanePosList[0];

        //���݂̈ʒu
        float present_Location = (Time.time*0.01f) / distance;

        if (_IsLaneChamge)
        {
            //�I�u�W�F�N�g�̈ړ�
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, _LanePosList[_NowLane]), 0.08f);

            //���[���ړ��̏I��
            if (present_Location >= 1)
            {
                //_IsLaneChamge = false;
            }

        }



    }

    public void ChangeLaneAnime_Start()
    {
        _IsLaneChamge = true;

        if (InputZ_tmp > 0)//���Z
        {
            _NowLane++;
            //����ی�
            if (_NowLane >= _LanePosList.Count)
            {
                _NowLane = _LanePosList.Count - 1;
                return;
            }
        }
        else if (InputZ_tmp < 0)//���Z
        {
            _NowLane--;
            //�����ی�
            if (_NowLane < 0.00f)
            {
                _NowLane = 0;
                return;
            }
        }


    }
    public void ChangeLaneAnime_End()
    {
        _IsLaneChamge = false;
        PlayerAttackAnimator.ResetTrigger("ChangeLane");

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
            //�A�j���[�V�����Đ�
            PlayerAttackAnimator.SetTrigger("Avoid");

            _IsAvoid = true;
            AvoidPos_Start = transform.position;

            if (Input_tmp > 0)
            {
                AvoidPos_End = transform.position + new Vector3(_AvoidanceValue, 0, 0);//���ʂ̉�
            }
            else
            {
                AvoidPos_End = transform.position + new Vector3(-_AvoidanceValue, 0, 0);//���ʂ̉�
            }
            StartCoroutine(AvoidReLoad_IE());

            return true;
        }

        return false;
    }

    private void Avoidance()
    {


        if (PlayerAttackAnimator.IsInTransition(0))//�J�ڒ�
        {
            return;
        }

        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _IsAvoid = false;
        }

        if (_IsAvoid)
        {
            transform.position = Vector3.Lerp(transform.position, AvoidPos_End, CalcMoveRatio());
        }
        else
        {
            _IsLaneChamge = false;
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

        PlayerAttackAnimator.ResetTrigger("Attack");
    }

    //���ł̍U��
    bool PlayerAttackMove()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_SwordCollider.enabled)
        {
            _SwordCollider.enabled = true;

            PlayerAttackAnimator.SetTrigger("Attack");

            PlayerAttackAnimator.Play("Attack", 0, 0);
            Debug.Log("SwordAttack!!");
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

    IEnumerator AvoidReLoad_IE()
    {
        yield return new WaitForSeconds(AvoidReLoadTime);
        _IsAvoid = false;

        yield break;


    }

}


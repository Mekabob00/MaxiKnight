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
    private List<float> PlayerWlakSpeed;
    [SerializeField]
    private float applySpeed = 0.2f;       // ��]�̓K�p���x

    [SerializeField]
    public int PlayerAttackPoint;

    [SerializeField, Tooltip("�����蔻��")]
    private Collider _HitBase;

    [SerializeField, Tooltip("����̓����蔻��")]
    private List<Collider> _SwordColliderList;

    [SerializeField, Tooltip("����̃I�u�W�F�N�g")]
    private SwordControll _SwordBeh = null;

    [SerializeField, Tooltip("�U�����x")]
    private List<float> _AttackSpeed;

    [SerializeField, Tooltip("�A���U���̗P�\����")]
    private List<float> _AttackGraceTime;

    [SerializeField, Tooltip("����̑傫��")]
    private float _AvoidanceValue = 50.0f;

    [SerializeField, Tooltip("����̃����[�h����")]
    private float AvoidReLoadTime = 3.0f;

    [SerializeField, Tooltip("���[�����WY")]
    private List<float> _LanePosYList;

    [SerializeField, Tooltip("���[���̍��WZ")]
    private List<float> _LanePosZList;

    [SerializeField, Tooltip("�傫��")]
    private List<float> _ScaleList;

    [SerializeField, Tooltip("�ő僉�C�t")]
    private float _MAXHP = 100;

    private Vector3 latestPosition;
    private Animator PlayerAttackAnimator;
    public Collider _SwordCollider;
    private GunControll _GunControll;

    //Effect
    [SerializeField]
    private ParticleSystem RunSmork_P;
    [SerializeField]
    private ParticleSystem JunpDownSmork_P;

    private int _AttackType = 0;
    private float _NextActionTime;


    private bool _IsGaraceTime = false;
    public bool _IsAvoid = false;
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


    //�O������̃f�[�^���󂯎��
    private int DataSowrdNum;
    public static float AttackBuff = 1;


    private void Awake()
    {//�X�^�[�g�֐��O�ɉ������������鎞�p
    }
    void Start()
    {
        //�O������f�[�^���󂯎��
        DataSowrdNum = DataManager.Instance._WeaponNumberSword;

        //animtion
        PlayerAttackAnimator = GetComponent<Animator>();
        PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed[DataSowrdNum]);
        PlayerAttackAnimator.SetFloat("RunSpeed", PlayerWlakSpeed[DataSowrdNum] * 0.7f);

        _GunControll = GetComponent<GunControll>();

        latestPosition = transform.position;
        PlayerAttackPoint = 10;
        _AttackType = 0;
        _StartTime = Time.time;
        _NowHP = _MAXHP;
        AttackBuff = DataManager.Instance._PlayerAttackBuff;

        _NowLane = 0;//����

        //����̓����蔻��̐ݒ�
        _SwordCollider = _SwordColliderList[DataSowrdNum];
        _SwordCollider.enabled = false;//�R���C�_�[��OFF

        Input_tmp = 1;
        _IsGaraceTime = false;
        _IsAvoid = false;
        _IsAttack = false;

    }
    void Update()
    {

        bool isAvoid = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance");
        if (isAvoid)
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
        float dx = Input.GetAxisRaw("Horizontal") * Time.deltaTime * PlayerWlakSpeed[DataSowrdNum]*7;
        float dz = Input.GetAxisRaw("Vertical") * Time.deltaTime * 1;

        bool IsAttack = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        bool IschangeLane = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChangeLane");
        bool IsAvoid = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance");

        //���[�J���ݒ�
        const float PositionX_Left = 30;
        const float PositionX_Right = 0.3f;

        //�U�����ɓ������Ȃ�
        if (!IsAttack && !IschangeLane)
        {

            transform.position = new Vector3(
            transform.position.x + dx, _LanePosYList[_NowLane], _LanePosZList[_NowLane]);

            //�o�O�C��
            _SwordCollider.enabled = false;

            //�A�j���[�V�����Đ�
            if (dx != 0.0f)
            {
                PlayerAttackAnimator.SetBool("Run", true);

                //Effect�̍Đ�
                RunSmork_P.Play();
            }
            else
            {
                PlayerAttackAnimator.SetBool("Run", false);
                RunSmork_P.Stop();
            }

            //X�����̌��E�ݒ�
            if (transform.position.x >= PositionX_Left)
            {
                transform.position = new Vector3(PositionX_Left, _LanePosYList[_NowLane], _LanePosZList[_NowLane]);
            }
            else if(transform.position.x<=PositionX_Right)
            {
                transform.position = new Vector3(PositionX_Right, _LanePosYList[_NowLane], _LanePosZList[_NowLane]);
            }
            else
            {
                
            }
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
            if (!IsAvoid||!_IsLaneChamge)
            {
                transform.rotation = Quaternion.LookRotation(diff); //�v���C���[�̌����ύX
                
            }

        }
        else
        {
            
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
        float distance = _LanePosZList[1] - _LanePosZList[0];

        //���݂̈ʒu
        float present_Location = (Time.time*0.01f) / distance;

        if (_IsLaneChamge)
        {
            //�I�u�W�F�N�g�̈ړ�
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, _LanePosYList[_NowLane], _LanePosZList[_NowLane]), 0.08f);

            //���[���ړ��̏I��
            if (present_Location >= 1)
            {
                //_IsLaneChamge = false;
            }

        }



    }

    void ChnageLanetoScale()
    {
        this.transform.localScale = new Vector3(_ScaleList[_NowLane], _ScaleList[_NowLane], _ScaleList[_NowLane]);
    }

    public void ChangeLaneAnime_Start()
    {
        _IsLaneChamge = true;

        if (InputZ_tmp > 0)//���Z
        {
            _NowLane++;
            //����ی�
            if (_NowLane >= _LanePosZList.Count)
            {
                _NowLane = _LanePosZList.Count - 1;
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

        ChnageLanetoScale();
    }

    public void ChangeLaneAnime_End()
    {
        _IsLaneChamge = false;
        PlayerAttackAnimator.ResetTrigger("ChangeLane");

        JunpDownSmork_P.Stop();
        if (JunpDownSmork_P.isStopped)
        {
            JunpDownSmork_P.Play();
        }
        

    }

    public int GetNowLane()
    {
        return _NowLane;
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

            //�o�O�̕ی�
            _SwordCollider.enabled = false;

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
        else if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            //_IsAvoid = false;
        }
        else if (PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            transform.position = Vector3.Lerp(transform.position, AvoidPos_End, CalcMoveRatio());
        }

    }

    public void PlayerAttackAnimation()
    {


        if (PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("run") || PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            //���̍U��
            PlayerAttackMove();

            //�e�ł̍U��
            //_GunControll.GunAttack();
        }



    }
    public void _AddDamege(float _Damege)
    {
        _NowHP -= _Damege;
        PlayerAttackAnimator.SetTrigger("OnDamage");
        _HitBase.enabled = false;
    }

    public void AttackAnim_ColldierON()
    {
        _SwordCollider.enabled = true;
    }

    public void AttackAnimEnd()
    {
        _IsAttack = false;
        PlayerAttackAnimator.SetFloat("AttackSpeed", 0);
        //0.5�b��Collider��OFF
        _SwordCollider.enabled = false;

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
        int AttackNum = DataManager.Instance._WeaponNumberSword;
        while (time <= _AttackGraceTime[AttackNum])//�P�\����
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
                PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed[AttackNum]);

                yield break;
            }
            else if (PlayerAvoidance())//���
            {
                break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed[AttackNum]);

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


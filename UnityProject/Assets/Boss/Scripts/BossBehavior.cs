using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Move, Enter, StandBy, AttackPlayer, Charge, AttackCastle, Return };

public class BossBehavior : MonoBehaviour, IPlayerDamege
{
    [Header("�����l")]
    public int _Life;
    [Header("�ړ����x")]
    public float _MoveSpeed;
    [Header("���f��ړ����x")]
    public float _BreakSpeed;
    [Header("�_���[�W")]
    public int _Damage;
    [Header("�U���͈�")]
    public float _Area;
    [Header("�`���[�W")]
    public float _ChargeTime;
    [Header("�`���[�W���f��")]
    public int _StopChargeCount;
    [Header("��")]
    public GameObject _Castle;
    [Header("�v���C���[")]
    public GameObject _Player;
    [Header("�U���G���A")]
    public GameObject _AttackArea;
    [Header("���[�U�[")]
    public GameObject _Laser;

    Animator anim;
    [SerializeField, Header("���")]
    State state;            //�X�e�[�^�X
    Vector3 startPoint;     //�����ʒu
    [SerializeField, Header("�`���[�W��Ԃ̉����񐔌v�Z")]
    int hitCount;           //�`���[�W��Ԃ̉����񐔌v�Z
    float castleDistance;   //��Ƃ̋���
    float chargeTimeCount;  //�`���[�W���Ԍv�Z
    bool isWait;            //��ԑJ�ڑ҂�
    bool isEnter;           //�\����ԋL�^
    bool isAttackedPlayer;  //�v���C���[�ɍU���L�^
    bool isMove;            //�ړ�(Anim�p)
    bool isStandBy;         //�U���\��(Anim�p)
    bool isCharge;          //�`���[�W���(Anim�p)

    void Start()
    {
        state = State.Move;
        startPoint = transform.position;
        anim = GetComponent<Animator>();
        isMove = false;
        isWait = false;
        isEnter = false;
        isStandBy = false;
        isAttackedPlayer = false;
        isCharge = false;
    }

    void Update()
    {
        castleDistance = Vector3.Distance(transform.position, _Castle.transform.position);

        SetState();
        SetAnim();
    }

    void SetState()
    {
        switch (state)
        {
            case State.Move:
                StateUpDate_Move();
                break;
            case State.Enter:
                StateUpDate_Enter();
                break;
            case State.StandBy:
                StateUpDate_StandBy();
                break;
            case State.AttackPlayer:
                StateUpDate_AttackPlayer();
                break;
            case State.Charge:
                StateUpDate_Charge();
                break;
            case State.AttackCastle:
                StateUpDate_AttackCastle();
                break;
            case State.Return:
                StateUpDate_Return();
                break;
        }
    }

    void StateUpDate_Move()
    {
        isMove = true;
        if (!isWait)
            transform.position += new Vector3(-_MoveSpeed * Time.deltaTime, 0, 0);

        //��ԑJ��
        if (castleDistance <= 52 && !isEnter)
        {
            state = State.Enter;
            anim.SetTrigger("Enter");
            StartCoroutine(WaitTime(5.0f));
        }
        else if (castleDistance <= 40 && !isAttackedPlayer)
        {
            //�U���\�����
            state = State.StandBy;
            //�U���͈͐���
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Instantiate(_AttackArea, new Vector3(_Player.transform.position.x, _AttackArea.transform.position.y, _Player.transform.position.z + (-_Area + (_Area + 1) * i)), Quaternion.identity);
                temp.GetComponent<SearchArea>()._Area = _Area;
                temp.GetComponent<SearchArea>()._Damage = _Damage;
                temp.GetComponent<SearchArea>()._SetTime = 3.0f;
            }
            StartCoroutine(WaitTime(3.0f));
        }
        else if (castleDistance <= 25)
        {
            state = State.Charge;
        }
    }
    void StateUpDate_Enter()
    {
        isMove = false;
        isEnter = true;

        if (!isWait)
        {
            state = State.Move;
        }
    }
    void StateUpDate_StandBy()
    {
        isMove = false;
        isStandBy = true;

        //��ԑJ��
        if (!isWait)
        {
            anim.SetTrigger("Attack");
            state = State.AttackPlayer;
            isAttackedPlayer = true;
            isStandBy = false;
            StartCoroutine(WaitTime(4.0f));
        }
    }
    void StateUpDate_AttackPlayer()
    {
        if (!isWait)
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                state = State.Move;
    }
    void StateUpDate_Charge()
    {
        isMove = false;
        isCharge = true;
        chargeTimeCount += Time.deltaTime;

        //�`���[�W����
        if (chargeTimeCount >= _ChargeTime)
        {
            state = State.AttackCastle;
            _Castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
            isCharge = false;
            anim.SetTrigger("Attack");
            _Laser.SetActive(true);
            StartCoroutine(WaitTime(0.5f));
        }
        //�`���[�W���f
        if (hitCount >= _StopChargeCount)
        {
            state = State.Return;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            isCharge = false;
            StartCoroutine(WaitTime(2.0f));
        }
    }
    void StateUpDate_AttackCastle()
    {
        //�J�n�n�_�ɖ߂�
        if (!isWait)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), 20 * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
        {
            //��ԃ��Z�b�g
            StateReset();
            StartCoroutine(WaitTime(4.0f));
        }
    }
    void StateUpDate_Return()
    {
        //�J�n�n�_�ɖ߂�
        if (!isWait)
        {
            isMove = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
        {
            //��ԃ��Z�b�g
            StateReset();
            StartCoroutine(WaitTime(4.0f));
            transform.rotation = Quaternion.AngleAxis(270, Vector3.up);
        }
    }

    void SetAnim()
    {
        anim.SetBool("Move", isMove);
        anim.SetBool("StandBy", isStandBy);
        anim.SetBool("Charge", isCharge);
    }

    void StateReset()
    {
        _Laser.SetActive(false);
        state = State.Move;
        hitCount = 0;
        chargeTimeCount = 0;
        isAttackedPlayer = false;
        isWait = false;
        isCharge = false;
        isEnter = false;
    }

    //���̏�Ԃ��ڍs�܂ł̑҂�����
    IEnumerator WaitTime(float time)
    {
        isWait = true;
        yield return new WaitForSeconds(time);
        isWait = false;
    }

    //�_���[�W���󂯂�
    public void _AddDamege(int _Damage)
    {
        _Life--;
        hitCount++;
        chargeTimeCount -= 1.0f;
        if (_Life <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player Attack Area")
        //{
        //    _AddDamege(1);
        //}
    }
}

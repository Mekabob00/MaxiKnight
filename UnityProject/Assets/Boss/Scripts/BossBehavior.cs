using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { MOVE, ENTER, STANDBY, ATTACKPLAYER, CHARGE, ATTACKCASTLE, RETURN, DAMAGE, DOWN, STANDUP };

public class BossBehavior : MonoBehaviour, IPlayerDamege
{
    [Header("�����l")]
    public int _Health;
    [Header("�ړ����x")]
    public float _MoveSpeed;
    [Header("���f��ړ����x")]
    public float _BreakSpeed;
    [Header("�_���[�W")]
    public int _Damage;
    [Header("�U���͈�")]
    public float _Area;
    [Header("�`���[�W")]
    [Tooltip("����")]public float _ChargeTime;
    [Tooltip("���f��")]public int _StopChargeCount;
    [Header("��")]
    public GameObject _Castle;
    [Header("�v���C���[")]
    public GameObject _Player;
    [Header("�U���G���A")]
    public GameObject _AttackArea;
    public GameObject _Laser;
    [Header("�����ݒ�")]
    [Tooltip("�\��")]public float _EnterDistance;
    [Tooltip("�U��")] public float _StandByDistance;
    [Tooltip("�`���[�W")] public float _ChargeDistance;

    Animator anim;
    [SerializeField, Header("���")]
    STATE state;            //�X�e�[�^�X
    Vector3 breakChargePos;
    [SerializeField, Tooltip("�����ʒu")]Vector3 startPoint;     //�����ʒu
    [SerializeField, Header("�`���[�W��Ԃ̉����񐔌v�Z")]
    int currentHitCount;           //�`���[�W��Ԃ̉����񐔌v�Z
    float castleDistance;   //��Ƃ̋���
    float chargeTimeCount;  //�`���[�W���Ԍv�Z
    bool isWait;            //��ԑJ�ڑ҂�
    bool isEnter;           //�\����ԋL�^
    bool isAttackedPlayer;  //�v���C���[�ɍU���L�^
    bool isMove;            //�ړ�(Anim�p)
    bool isStandBy;         //�U���\��(Anim�p)
    bool isCharge;          //�`���[�W���(Anim�p)
    bool isReturn;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = startPoint;
        StateReset();
    }

    void Update()
    {
        if (GlobalData.Instance.isGameOver) return;

        //Debug.Log(state);
        castleDistance = Vector3.Distance(transform.position, _Castle.transform.position);

        SetState();
        SetAnim();
    }

    void SetState()
    {
        switch (state)
        {
            case STATE.MOVE:
                StateUpDate_Move();
                break;
            case STATE.ENTER:
                StateUpDate_Enter();
                break;
            case STATE.STANDBY:
                StateUpDate_StandBy();
                break;
            case STATE.ATTACKPLAYER:
                StateUpDate_AttackPlayer();
                break;
            case STATE.CHARGE:
                StateUpDate_Charge();
                break;
            case STATE.ATTACKCASTLE:
                StateUpDate_AttackCastle();
                break;
            case STATE.RETURN:
                StateUpDate_Return();
                break;
            case STATE.DAMAGE:
                StateUpDate_Damage();
                break;
            case STATE.DOWN:
                StateUpDate_Down();
                break;
            case STATE.STANDUP:
                StateUpDate_StandUp();
                break;
        }
    }

    void StateUpDate_Move()
    {
        isMove = true;
        if (!isWait)
            transform.position += new Vector3(-_MoveSpeed * Time.deltaTime, 0, 0);

        //��ԑJ��
        if (castleDistance <= _EnterDistance && !isEnter)
        {
            state = STATE.ENTER;
            anim.SetTrigger("Enter");
            StartCoroutine(WaitTime(5.0f));
        }
        else if (castleDistance <= _StandByDistance && !isAttackedPlayer)
        {
            //�U���\�����
            state = STATE.STANDBY;
            //�U���͈͐���
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Instantiate(_AttackArea, new Vector3(_Player.transform.position.x + ((-_Area - 2) + (_Area + 2) * i), _AttackArea.transform.position.y, _Player.transform.position.z), Quaternion.identity);
                temp.GetComponent<SearchArea>()._Area = _Area;
                temp.GetComponent<SearchArea>()._Damage = _Damage;
                temp.GetComponent<SearchArea>()._SetTime = 3.0f;
            }
            StartCoroutine(WaitTime(3.0f));
        }
        else if (castleDistance <= _ChargeDistance)
        {
            state = STATE.CHARGE;
        }
    }
    void StateUpDate_Enter()
    {
        isMove = false;
        isEnter = true;

        if (!isWait)
        {
            state = STATE.MOVE;
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
            state = STATE.ATTACKPLAYER;
            isAttackedPlayer = true;
            isStandBy = false;
            StartCoroutine(WaitTime(4.0f));
        }
    }
    void StateUpDate_AttackPlayer()
    {
        if (!isWait)
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                state = STATE.MOVE;
    }
    void StateUpDate_Charge()
    {
        isMove = false;
        isCharge = true;
        chargeTimeCount += Time.deltaTime;

        //�`���[�W����
        if (chargeTimeCount >= _ChargeTime)
        {
            state = STATE.ATTACKCASTLE;
            float percent = (_StopChargeCount - currentHitCount) * 1.0f / _StopChargeCount;
            Debug.Log(_Damage * percent);
            _Castle.GetComponent<CastleBehavior>()._AddDamage((int)(_Damage * percent));
            isCharge = false;
            anim.SetTrigger("Attack");
            _Laser.GetComponent<LaserArea>()._Damage = (int)(_Damage * percent);
            _Laser.SetActive(true);
            StartCoroutine(WaitTime(0.5f));
        }
        //�`���[�W���f
        if (currentHitCount >= _StopChargeCount)
        {
            state = STATE.DOWN;
            breakChargePos = transform.position - new Vector3(0, 10, 0);
            anim.SetTrigger("Down");
            StartCoroutine(WaitTime(1.0f));
            isCharge = false;
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
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //�J�n�n�_�ɖ߂�
        if (!isWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
        {
            //��ԃ��Z�b�g
            StateReset();
            StartCoroutine(WaitTime(4.0f));
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    void StateUpDate_Damage()
    {
        isMove = false;
        if (!isWait)
            state = STATE.MOVE;
    }
    void StateUpDate_Down()
    {
        if (!isWait)
        {
            state = STATE.STANDUP;
            StartCoroutine(WaitTime(1.0f));
        }
    }
    void StateUpDate_StandUp()
    {
        if(!isWait)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y-10, transform.position.z), 10 * Time.deltaTime);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("StandUp"))
        {
            state = STATE.RETURN;
            isReturn = true;
        }
    }

    void SetAnim()
    {
        anim.SetBool("Move", isMove);
        anim.SetBool("StandBy", isStandBy);
        anim.SetBool("Charge", isCharge);
        anim.SetBool("Return", isReturn);
    }

    void StateReset()
    {
        _Laser.SetActive(false);
        state = STATE.MOVE;
        currentHitCount = 0;
        chargeTimeCount = 0;
        isMove = false;
        isWait = false;
        isEnter = false;
        isStandBy = false;
        isAttackedPlayer = false;
        isCharge = false;
        isReturn = false;
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
        Debug.Log("hit!");
        _Health--;
        if (_Health <= 0) Destroy(gameObject);

        if (state == STATE.MOVE)
        {
            StartCoroutine(WaitTime(0.3f));
            anim.SetTrigger("Damage");
            state = STATE.DAMAGE;
        }else if(state == STATE.CHARGE)
        {
            currentHitCount++;
            //chargeTimeCount -= 1.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Attack Area")
        {
            _AddDamege(1);
        }
    }
}

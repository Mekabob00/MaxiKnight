using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { MOVE, ENTER, STANDBY, ATTACKPLAYER, CHARGE, ATTACKCASTLE, RETURN, DAMAGE, DOWN, STANDUP, DEAD };

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
    [Tooltip("����")] public float _ChargeTime;
    [Tooltip("���f��")] public int _StopChargeCount;

    [Header("�I�u�W�F�N�g")]
    public GameObject _Castle;
    public GameObject _Player;
    public GameObject _AttackArea;
    public GameObject _NapalmBomb;
    public GameObject _Laser;
    public GameObject _Explosion;
    public Transform[] _ExplosionPos;
    public GameObject _StageClear;
    public GameObject _DamageEffect;
    public GameObject _AttackFireEffect;

    [Header("�����ݒ�")]
    [Tooltip("�\��")] public float _EnterDistance;
    [Tooltip("�U��")] public float _StandByDistance;
    [Tooltip("�`���[�W")] public float _ChargeDistance;

    [Header("���̏�Ԉڍs�܂ł̑҂�����")]
    [Tooltip("�\���Ɉڍs�O")] public float _Move_To_Enter;
    [Tooltip("�v���C���[�֍U�������Ɉڍs�O")] public float _Move_To_StandBy;
    [Tooltip("�v���C���[�֍U���Ɉڍs�O")] public float _StandBy_To_AttackPlayer;
    [Tooltip("���_�֍U���Ɉڍs�O")] public float _Charge_To_AttackCastle;
    [Tooltip("�`���[�W���f�Ɉڍs�O")]public float _Charge_To_StopCharge;
    [Tooltip("�_�E�������O")]public float _Down_To_StandUp;
    [Tooltip("���̍U�����[�v�Ɉڍs�O")] public float _NextAttackLoop;

    [Header("�f�o�b�O�p")]
    Animator m_anim;
    Vector3 m_breakChargePos;
    [SerializeField, Tooltip("���")] STATE m_state;            //�X�e�[�^�X
    [SerializeField, Tooltip("�����ʒu")] Vector3 m_startPoint;     //�����ʒu
    [SerializeField, Tooltip("�`���[�W��Ԃ̉����񐔌v�Z")] int m_currentHitCount;           //�`���[�W��Ԃ̉����񐔌v�Z
    float m_castleDistance;   //��Ƃ̋���
    float m_chargeTimeCount;  //�`���[�W���Ԍv�Z
    float m_explosionCT = 0.6f;
    float m_timer;            //�^�C�}�[
    bool m_isWait;            //��ԑJ�ڑ҂�
    bool m_isEnter;           //�\����ԋL�^
    bool m_isAttackedPlayer;  //�v���C���[�ɍU���L�^
    bool m_isMove;            //�ړ�(Anim�p)
    bool m_isStandBy;         //�U���\��(Anim�p)
    bool m_isCharge;          //�`���[�W���(Anim�p)
    bool m_isReturn;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        transform.position = m_startPoint;
        StateReset();
    }

    void Update()
    {
        if (GlobalData.Instance.isGameOver)
        {
            StateReset();
            return;
        }

        //Debug.Log(state);
        //���_�̋���
        m_castleDistance = Vector3.Distance(transform.position, _Castle.transform.position);

        SetState();
        SetAnim();
    }

    void SetState()
    {
        switch (m_state)
        {
            case STATE.MOVE:
                StateUpdate_Move();
                break;
            case STATE.ENTER:
                StateUpdate_Enter();
                break;
            case STATE.STANDBY:
                StateUpdate_StandBy();
                break;
            case STATE.ATTACKPLAYER:
                StateUpdate_AttackPlayer();
                break;
            case STATE.CHARGE:
                StateUpdate_Charge();
                break;
            case STATE.ATTACKCASTLE:
                StateUpdate_AttackCastle();
                break;
            case STATE.RETURN:
                StateUpdate_Return();
                break;
            case STATE.DAMAGE:
                StateUpdate_Damage();
                break;
            case STATE.DOWN:
                StateUpdate_Down();
                break;
            case STATE.STANDUP:
                StateUpdate_StandUp();
                break;
            case STATE.DEAD:
                StateUpdate_Dead();
                break;
        }
    }

    void StateUpdate_Move()
    {
        m_isMove = true;
        //���_�ֈړ�����
        if (!m_isWait)
            transform.position += new Vector3(-_MoveSpeed * Time.deltaTime, 0, 0);

        //��ԑJ��
        //���_�̋����ɂ���ԑJ��
        if (m_castleDistance <= _EnterDistance && !m_isEnter)
        {
            m_state = STATE.ENTER;
            m_anim.SetTrigger("Enter");
            StartCoroutine(WaitTime(_Move_To_Enter));
        }
        else if (m_castleDistance <= _StandByDistance && !m_isAttackedPlayer)
        {
            //�U���\�����
            m_state = STATE.STANDBY;
            StartCoroutine(WaitTime(_Move_To_StandBy));
        }
        else if (m_castleDistance <= _ChargeDistance)
        {
            m_state = STATE.CHARGE;
        }
    }
    void StateUpdate_Enter()
    {
        m_isMove = false;
        m_isEnter = true;

        if (!m_isWait)
        {
            m_state = STATE.MOVE;
        }
    }
    void StateUpdate_StandBy()
    {
        m_isMove = false;
        m_isStandBy = true;

        //��ԑJ��
        if (!m_isWait)
        {
            m_anim.SetTrigger("Attack");
            m_state = STATE.ATTACKPLAYER;
            //�U������
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Instantiate(_AttackArea, new Vector3(_Player.transform.position.x + ((-_Area - 2) + (_Area + 2) * i), transform.position.y + 0.15f, _Player.transform.position.z), Quaternion.identity);
                temp.GetComponent<SearchArea>()._NapalmBomb = Instantiate(_NapalmBomb, temp.transform.position + new Vector3(0, 80 - 20 * i, 0), Quaternion.Euler(90, 0, 0));
                temp.GetComponent<SearchArea>()._Area = _Area;
                temp.GetComponent<SearchArea>()._Damage = _Damage;
            }
            m_isAttackedPlayer = true;
            m_isStandBy = false;
            StartCoroutine(WaitTime(_StandBy_To_AttackPlayer));
        }
    }
    void StateUpdate_AttackPlayer()
    {
        if (!m_isWait)
            if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                m_state = STATE.MOVE;
    }
    void StateUpdate_Charge()
    {
        m_isMove = false;
        m_isCharge = true;
        m_chargeTimeCount += Time.deltaTime;

        //�`���[�W����
        if (m_chargeTimeCount >= _ChargeTime)
        {
            m_state = STATE.ATTACKCASTLE;
            float percent = (_StopChargeCount - m_currentHitCount) * 1.0f / _StopChargeCount;
            _Castle.GetComponent<CastleBehavior>()._AddDamage((int)(_Damage * percent));
            m_isCharge = false;
            m_anim.SetTrigger("Attack");

            GameObject temp = Instantiate(_AttackFireEffect, new Vector3(0, 3, transform.position.z), _AttackFireEffect.transform.rotation);
            temp.SetActive(true);
            temp.transform.localScale = new Vector3(25, 10, 10);

            _Laser.GetComponent<LaserArea>()._Damage = (int)(_Damage * percent);
            _Laser.SetActive(true);

            StartCoroutine(WaitTime(_Charge_To_AttackCastle));
        }
        //�`���[�W���f
        if (m_currentHitCount >= _StopChargeCount)
        {
            m_state = STATE.DOWN;
            m_breakChargePos = transform.position - new Vector3(0, 10, 0);
            m_anim.SetTrigger("Down");
            StartCoroutine(WaitTime(_Charge_To_StopCharge));
            m_isCharge = false;
        }
    }
    void StateUpdate_AttackCastle()
    {
        //�J�n�n�_�ɖ߂�
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_startPoint.x, transform.position.y, transform.position.z), 100 * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - m_startPoint.x) <= 0.5)
        {
            //��ԃ��Z�b�g
            StateReset();
            StartCoroutine(WaitTime(_NextAttackLoop));
        }
    }
    void StateUpdate_Return()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //�J�n�n�_�ɖ߂�
        if (!m_isWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x - m_startPoint.x) <= 0.5)
        {
            //��ԃ��Z�b�g
            StateReset();
            StartCoroutine(WaitTime(_NextAttackLoop));
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
    void StateUpdate_Damage()
    {
        m_isMove = false;
        if (!m_isWait)
            m_state = STATE.MOVE;
    }
    void StateUpdate_Down()
    {
        if (!m_isWait)
        {
            m_state = STATE.STANDUP;
            StartCoroutine(WaitTime(_Down_To_StandUp));
        }
    }
    void StateUpdate_StandUp()
    {
        if (!m_isWait)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_startPoint.x, transform.position.y - 7, transform.position.z), 10 * Time.deltaTime);

        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("StandUp"))
        {
            m_state = STATE.RETURN;
            m_isReturn = true;
        }
    }
    void StateUpdate_Dead()
    {
        if (m_isWait) return;

        m_timer -= Time.deltaTime;
        if(m_timer <= 0)
        {
            m_timer = m_explosionCT;
            //�����G�t�F�N�g����
            int num = Random.Range(1, _ExplosionPos.Length / 2);
            while(num > 0)
            {
                GameObject obj = Instantiate(_Explosion, _ExplosionPos[Random.Range(0, _ExplosionPos.Length)]);
                obj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                num--;
            }
        }
    }

    void SetAnim()
    {
        m_anim.SetBool("Move", m_isMove);
        m_anim.SetBool("StandBy", m_isStandBy);
        m_anim.SetBool("Charge", m_isCharge);
        m_anim.SetBool("Return", m_isReturn);
    }

    void StateReset()
    {
        _Laser.SetActive(false);
        m_state = STATE.MOVE;
        m_currentHitCount = 0;
        m_chargeTimeCount = 0;
        m_isMove = false;
        m_isEnter = false;
        m_isStandBy = false;
        m_isAttackedPlayer = false;
        m_isCharge = false;
        m_isReturn = false;
    }

    //�X�e�[�W�N���A��̃V�[������
    IEnumerator StageClear()
    {
        yield return new WaitForSeconds(4.5f);
        _StageClear.GetComponent<Animator>().SetTrigger("StageClear");
        yield return new WaitForSeconds(2.5f);
        DataManager.Instance._Stage++;
        FadeManager.Instance.LoadScene("Shop", 1.0f);
    }

    //���̏�Ԃ��ڍs�܂ł̑҂�����
    IEnumerator WaitTime(float time)
    {
        m_isWait = true;
        yield return new WaitForSeconds(time);
        m_isWait = false;
    }

    //�_���[�W���󂯂�
    public void _AddDamege(float _Damage)
    {
        if (_Health <= 0) return;

        Debug.Log("hit!");
        _Health--;
        Instantiate(_DamageEffect, transform.position + new Vector3(0, 2, 0), transform.rotation);
        if (_Health <= 0)
        {
            GlobalData.Instance.isStageClear = true;
            m_state = STATE.DEAD;
            m_anim.SetTrigger("Dead");
            StartCoroutine(WaitTime(1.0f));
            StartCoroutine(StageClear());
        }

        if (m_state == STATE.MOVE)
        {
            StartCoroutine(WaitTime(0.3f));
            m_anim.SetTrigger("Damage");
            m_state = STATE.DAMAGE;
        }else if(m_state == STATE.CHARGE)
        {
            m_currentHitCount++;
            //chargeTimeCount -= 1.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Attack Area")
        {
            _AddDamege(Player_Controll.AttackBuff);
        }
    }
}

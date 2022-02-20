using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { MOVE, ENTER, STANDBY, ATTACKPLAYER, CHARGE, ATTACKCASTLE, RETURN, DAMAGE, DOWN, STANDUP, DEAD };

public class BossBehavior : MonoBehaviour, IPlayerDamege
{
    [Header("生命値")]
    public int _Health;
    [Header("移動速度")]
    public float _MoveSpeed;
    [Header("中断後移動速度")]
    public float _BreakSpeed;
    [Header("ダメージ")]
    public int _Damage;
    [Header("攻撃範囲")]
    public float _Area;
    [Header("チャージ")]
    [Tooltip("時間")]public float _ChargeTime;
    [Tooltip("中断回数")]public int _StopChargeCount;
    [Header("オブジェクト")]
    public GameObject _Castle;
    public GameObject _Player;
    public GameObject _AttackArea;
    public GameObject _Laser;
    public GameObject _Explosion;
    public Transform[] _ExplosionPos;
    [Header("距離設定")]
    [Tooltip("構え")]public float _EnterDistance;
    [Tooltip("攻撃")] public float _StandByDistance;
    [Tooltip("チャージ")] public float _ChargeDistance;

    Animator m_anim;
    Vector3 m_breakChargePos;
    [SerializeField, Header("状態")] STATE m_state;            //ステータス
    [SerializeField, Tooltip("初期位置")] Vector3 m_startPoint;     //初期位置
    [SerializeField, Header("チャージ状態の解除回数計算")]
    int m_currentHitCount;           //チャージ状態の解除回数計算
    float m_castleDistance;   //城との距離
    float m_chargeTimeCount;  //チャージ時間計算
    float m_explosionCT = 0.4f;
    float m_timer;            //タイマー
    bool m_isWait;            //状態遷移待ち
    bool m_isEnter;           //構え状態記録
    bool m_isAttackedPlayer;  //プレイヤーに攻撃記録
    bool m_isMove;            //移動(Anim用)
    bool m_isStandBy;         //攻撃予告(Anim用)
    bool m_isCharge;          //チャージ状態(Anim用)
    bool m_isReturn;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        transform.position = m_startPoint;
        StateReset();
    }

    void Update()
    {
        if (GlobalData.Instance.isGameOver) return;

        //Debug.Log(state);
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
        if (!m_isWait)
            transform.position += new Vector3(-_MoveSpeed * Time.deltaTime, 0, 0);

        //状態遷移
        if (m_castleDistance <= _EnterDistance && !m_isEnter)
        {
            m_state = STATE.ENTER;
            m_anim.SetTrigger("Enter");
            StartCoroutine(WaitTime(5.0f));
        }
        else if (m_castleDistance <= _StandByDistance && !m_isAttackedPlayer)
        {
            //攻撃予告状態
            m_state = STATE.STANDBY;
            //攻撃範囲生成
            for (int i = 0; i < 3; ++i)
            {
                GameObject temp = Instantiate(_AttackArea, new Vector3(_Player.transform.position.x + ((-_Area - 2) + (_Area + 2) * i), _AttackArea.transform.position.y, _Player.transform.position.z), Quaternion.identity);
                temp.GetComponent<SearchArea>()._Area = _Area;
                temp.GetComponent<SearchArea>()._Damage = _Damage;
                temp.GetComponent<SearchArea>()._SetTime = 3.0f;
            }
            StartCoroutine(WaitTime(3.0f));
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

        //状態遷移
        if (!m_isWait)
        {
            m_anim.SetTrigger("Attack");
            m_state = STATE.ATTACKPLAYER;
            m_isAttackedPlayer = true;
            m_isStandBy = false;
            StartCoroutine(WaitTime(4.0f));
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

        //チャージ完成
        if (m_chargeTimeCount >= _ChargeTime)
        {
            m_state = STATE.ATTACKCASTLE;
            float percent = (_StopChargeCount - m_currentHitCount) * 1.0f / _StopChargeCount;
            Debug.Log(_Damage * percent);
            _Castle.GetComponent<CastleBehavior>()._AddDamage((int)(_Damage * percent));
            m_isCharge = false;
            m_anim.SetTrigger("Attack");
            _Laser.GetComponent<LaserArea>()._Damage = (int)(_Damage * percent);
            _Laser.SetActive(true);
            StartCoroutine(WaitTime(0.5f));
        }
        //チャージ中断
        if (m_currentHitCount >= _StopChargeCount)
        {
            m_state = STATE.DOWN;
            m_breakChargePos = transform.position - new Vector3(0, 10, 0);
            m_anim.SetTrigger("Down");
            StartCoroutine(WaitTime(1.0f));
            m_isCharge = false;
        }
    }
    void StateUpdate_AttackCastle()
    {
        //開始地点に戻る
        if (!m_isWait)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_startPoint.x, transform.position.y, transform.position.z), 20 * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - m_startPoint.x) <= 0.5)
        {
            //状態リセット
            StateReset();
            StartCoroutine(WaitTime(4.0f));
        }
    }
    void StateUpdate_Return()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //開始地点に戻る
        if (!m_isWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x - m_startPoint.x) <= 0.5)
        {
            //状態リセット
            StateReset();
            StartCoroutine(WaitTime(4.0f));
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
            StartCoroutine(WaitTime(1.0f));
        }
    }
    void StateUpdate_StandUp()
    {
        
        if (!m_isWait)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(m_startPoint.x, transform.position.y-10, transform.position.z), 10 * Time.deltaTime);

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
        m_isWait = false;
        m_isEnter = false;
        m_isStandBy = false;
        m_isAttackedPlayer = false;
        m_isCharge = false;
        m_isReturn = false;
    }

    void StageClear()
    {
        SwitchScene.ChangeSceneToShop();
    }

    //次の状態を移行までの待ち時間
    IEnumerator WaitTime(float time)
    {
        m_isWait = true;
        yield return new WaitForSeconds(time);
        m_isWait = false;
    }

    //ダメージを受ける
    public void _AddDamege(float _Damage)
    {
        Debug.Log("hit!");
        _Health--;
        if (_Health <= 0)
        {
            GlobalData.Instance.isStageClear = true;
            m_state = STATE.DEAD;
            m_anim.SetTrigger("Dead");
            StartCoroutine(WaitTime(1.0f));
            Invoke("StageClear", 8f);
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
            _AddDamege(1);
        }
    }
}

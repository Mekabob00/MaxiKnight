using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE { MOVE, ENTER, STANDBY, ATTACKPLAYER, CHARGE, ATTACKCASTLE, RETURN, DAMAGE, DOWN, STANDUP };

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
    [Header("城")]
    public GameObject _Castle;
    [Header("プレイヤー")]
    public GameObject _Player;
    [Header("攻撃エリア")]
    public GameObject _AttackArea;
    public GameObject _Laser;
    [Header("距離設定")]
    [Tooltip("構え")]public float _EnterDistance;
    [Tooltip("攻撃")] public float _StandByDistance;
    [Tooltip("チャージ")] public float _ChargeDistance;

    Animator anim;
    [SerializeField, Header("状態")]
    STATE state;            //ステータス
    Vector3 breakChargePos;
    [SerializeField, Tooltip("初期位置")]Vector3 startPoint;     //初期位置
    [SerializeField, Header("チャージ状態の解除回数計算")]
    int currentHitCount;           //チャージ状態の解除回数計算
    float castleDistance;   //城との距離
    float chargeTimeCount;  //チャージ時間計算
    bool isWait;            //状態遷移待ち
    bool isEnter;           //構え状態記録
    bool isAttackedPlayer;  //プレイヤーに攻撃記録
    bool isMove;            //移動(Anim用)
    bool isStandBy;         //攻撃予告(Anim用)
    bool isCharge;          //チャージ状態(Anim用)
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

        //状態遷移
        if (castleDistance <= _EnterDistance && !isEnter)
        {
            state = STATE.ENTER;
            anim.SetTrigger("Enter");
            StartCoroutine(WaitTime(5.0f));
        }
        else if (castleDistance <= _StandByDistance && !isAttackedPlayer)
        {
            //攻撃予告状態
            state = STATE.STANDBY;
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

        //状態遷移
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

        //チャージ完成
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
        //チャージ中断
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
        //開始地点に戻る
        if (!isWait)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), 20 * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
        {
            //状態リセット
            StateReset();
            StartCoroutine(WaitTime(4.0f));
        }
    }
    void StateUpDate_Return()
    {
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //開始地点に戻る
        if (!isWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
        {
            //状態リセット
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

    //次の状態を移行までの待ち時間
    IEnumerator WaitTime(float time)
    {
        isWait = true;
        yield return new WaitForSeconds(time);
        isWait = false;
    }

    //ダメージを受ける
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

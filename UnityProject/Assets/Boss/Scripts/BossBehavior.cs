using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Move, Enter, StandBy, AttackPlayer, Charge, AttackCastle, Return };

public class BossBehavior : MonoBehaviour, IPlayerDamege
{
    [Header("生命値")]
    public int _Life;
    [Header("移動速度")]
    public float _MoveSpeed;
    [Header("中断後移動速度")]
    public float _BreakSpeed;
    [Header("ダメージ")]
    public int _Damage;
    [Header("攻撃範囲")]
    public float _Area;
    [Header("チャージ")]
    public float _ChargeTime;
    [Header("チャージ中断回数")]
    public int _StopChargeCount;
    [Header("城")]
    public GameObject _Castle;
    [Header("プレイヤー")]
    public GameObject _Player;
    [Header("攻撃エリア")]
    public GameObject _AttackArea;
    [Header("レーザー")]
    public GameObject _Laser;

    Animator anim;
    [SerializeField, Header("状態")]
    State state;            //ステータス
    Vector3 startPoint;     //初期位置
    [SerializeField, Header("チャージ状態の解除回数計算")]
    int hitCount;           //チャージ状態の解除回数計算
    float castleDistance;   //城との距離
    float chargeTimeCount;  //チャージ時間計算
    bool isWait;            //状態遷移待ち
    bool isEnter;           //構え状態記録
    bool isAttackedPlayer;  //プレイヤーに攻撃記録
    bool isMove;            //移動(Anim用)
    bool isStandBy;         //攻撃予告(Anim用)
    bool isCharge;          //チャージ状態(Anim用)

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

        //状態遷移
        if (castleDistance <= 52 && !isEnter)
        {
            state = State.Enter;
            anim.SetTrigger("Enter");
            StartCoroutine(WaitTime(5.0f));
        }
        else if (castleDistance <= 40 && !isAttackedPlayer)
        {
            //攻撃予告状態
            state = State.StandBy;
            //攻撃範囲生成
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

        //状態遷移
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

        //チャージ完成
        if (chargeTimeCount >= _ChargeTime)
        {
            state = State.AttackCastle;
            _Castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
            isCharge = false;
            anim.SetTrigger("Attack");
            _Laser.SetActive(true);
            StartCoroutine(WaitTime(0.5f));
        }
        //チャージ中断
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
        //開始地点に戻る
        if (!isWait)
        {
            isMove = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
        {
            //状態リセット
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

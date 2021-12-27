using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Move, StandBy, AttackPlayer, Charge, AttackCastle, Return };

public class BossBehavior : MonoBehaviour, IPlayerDamege
{
    [Header("生命値")]
    public int _Life;
    [Header("移動速度")]
    public float _MoveSpeed;
    [Header("ダメージ")]
    public int _Damage;
    [Header("攻撃範囲")]
    public float _Area;
    [Header("城")]
    public GameObject _Castle;
    [Header("プレイヤー")]
    public GameObject _Player;
    [Header("攻撃エリア")]
    public GameObject _AttackArea;
    [Header("チャージ")]
    public float _ChargeTime;
    [Header("チャージ中断回数")]
    public int _StopChargeCount;

    Animator anim;
    [SerializeField]
    State state;            //ステータス
    Vector3 startPoint;     //初期位置
    int hitCount;           //チャージ状態の解除回数計算
    float castleDistance;   //城との距離
    float chargeTimeCount;  //チャージ時間計算
    bool isWait;            //状態遷移待ち
    bool isAttackPlayer;    //プレイヤーに攻撃
    bool isMove;            //移動(Anim)
    bool isCharge;          //チャージ状態（Anim）

    void Start()
    {
        state = State.Move;
        startPoint = transform.position;
        anim = GetComponent<Animator>();
        isMove = false;
        isWait = false;
        isAttackPlayer = false;
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
                isMove = true;
                if(!isWait)
                    transform.position += new Vector3(-_MoveSpeed * Time.deltaTime, 0, 0);

                //状態遷移
                if(castleDistance <= 40 && !isAttackPlayer)
                {
                    //攻撃予告状態
                    state = State.StandBy;
                    //攻撃範囲生成
                    for(int i = 0; i < 3; ++i)
                    {
                        GameObject temp = Instantiate(_AttackArea, new Vector3(_Player.transform.position.x, _AttackArea.transform.position.y, _Player.transform.position.z + (-_Area  + (_Area + 1) * i)), Quaternion.identity);
                        temp.GetComponent<SearchArea>()._Area = _Area;
                        temp.GetComponent<SearchArea>()._Damage = _Damage;
                        temp.GetComponent<SearchArea>()._SetTime = 1.5f;
                    }
                    StartCoroutine(WaitTime(1.5f));
                }
                else if(castleDistance <= 25)
                {
                    state = State.Charge;
                }
                break;
            case State.StandBy:
                isMove = false;

                //攻撃状態
                if (!isWait)
                {
                    anim.SetTrigger("Attack");
                    state = State.AttackPlayer;
                    isAttackPlayer = true;
                    StartCoroutine(WaitTime(4.0f));
                }
                break;
            case State.AttackPlayer:
                if (!isWait)
                  if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    state = State.Move;
                break;
            case State.Charge:
                isMove = false;
                isCharge = true;
                chargeTimeCount += Time.deltaTime;

                //チャージ完成
                if(chargeTimeCount >= _ChargeTime)
                {
                    state = State.AttackCastle;
                    _Castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
                    isCharge = false;
                    anim.SetTrigger("Attack");
                    StartCoroutine(WaitTime(0.5f));
                }
                //チャージ中断
                if(hitCount >= _StopChargeCount)
                {
                    state = State.Return;
                    transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    isCharge = false;
                    StartCoroutine(WaitTime(4.0f));
                }
                break;
            case State.AttackCastle:
                //開始地点に戻る
                if(!isWait)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), 20 * Time.deltaTime);
                
                if(Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
                {
                    //状態リセット
                    StateReset();
                    StartCoroutine(WaitTime(4.0f));
                }
                break;
            case State.Return:
                //開始地点に戻る
                if (!isWait)
                {
                    isMove = true;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), 5 * Time.deltaTime);
                }

                if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
                {
                    //状態リセット
                    StateReset();
                    StartCoroutine(WaitTime(4.0f));
                    transform.rotation = Quaternion.AngleAxis(270, Vector3.up);
                }
                break;
        }
    }

    void SetAnim()
    {
        anim.SetBool("Move", isMove);
        anim.SetBool("Charge", isCharge);
    }

    void StateReset()
    {
        state = State.Move;
        hitCount = 0;
        chargeTimeCount = 0;
        isAttackPlayer = false;
        isWait = false;
        isCharge = false;
    }

    //次の状態を移行までの待ち時間
    IEnumerator WaitTime(float time)
    {
        isWait = true;
        yield return new WaitForSeconds(time);
        isWait = false;
    }

    public void _AddDamege(int _Damage)
    {
        _Life--;
        hitCount++;
        chargeTimeCount -= 1.0f;
        if (_Life <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player Attack Area")
        {
            _AddDamege(1);
        }
    }
}

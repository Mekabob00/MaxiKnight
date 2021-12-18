using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastleState { MOVE, TAKEDAMAGE, ATTACK, PLAYERREVIVAL, DEAD}

public class CastleBehavior : MonoBehaviour
{
    //TODO
    //移動
    //被ダメージ --　未完成（エネミーからのダメージ）
    //プレイヤー生成 -- 未完成
    //援護射撃 -- 未完成（エネミーにダメージを与える）
    //ゲームオーバー

    #region 変数
    [Header("生命値")]
    public int _Life;

    [Header("援護射撃範囲")]
    public int _AttackRange;

    [Header("攻撃中心")]
    [SerializeField]private GameObject attackPoint;

    [Header("攻撃クールターム")]
    public float _AttackCT;

    private float timer;

    [Header("プレイヤー")]
    public GameObject _PlayerPrefab;

    [Header("弾")]
    public GameObject _BulletPrefab;

    private Animator anim;

    private bool isAttack;
    private bool isDead;

    [Header("城状態")]
    [SerializeField] private CastleState castleState;

    [Header("攻撃対象")]
    [SerializeField]private GameObject attackTarget; //援護射撃対象

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchState();
        SwitchAnim();
    }

    void SwitchState()
    {
        #region 状態変更

        if (isDead)
            castleState = CastleState.DEAD;
        else if (GlobalData.Instance.isPlayerDead)
        {
            castleState = CastleState.PLAYERREVIVAL;
            anim.SetTrigger("Revival");
            GlobalData.Instance.isPlayerDead = false;
        }
        else if (isAttack)
            castleState = CastleState.ATTACK;
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            castleState = CastleState.MOVE;

        #endregion

        //行動
        switch (castleState)
        {
            case CastleState.MOVE:
                if (attackTarget == null)
                    SearchEnemy();
                if (GlobalData.Instance.isPlayerInSecondLine && attackTarget)
                    isAttack = true;
                break;
            case CastleState.TAKEDAMAGE:
                break;
            case CastleState.ATTACK:
                if (attackTarget == null)
                    SearchEnemy();
                if (!GlobalData.Instance.isPlayerInSecondLine || attackTarget == null)
                    isAttack = false;

                //攻撃
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    timer = _AttackCT;
                    Attack();
                }
                break;
            case CastleState.PLAYERREVIVAL:
                break;
            case CastleState.DEAD:
                break;
        }
    }

    void SwitchAnim()
    {
        anim.SetBool("Attack", isAttack);
        anim.SetBool("Death", isDead);
    }

    //------------------------------------------------------------

    //敵捜索
    bool SearchEnemy()
    {
        //円型捜索
        var colliders = Physics.OverlapSphere(transform.position, _AttackRange);
        //矩形捜索
        //var colliders = Physics.OverlapBox(transform.position, new Vector3(_AttackRange, 20, 20));
        //敵記録
        foreach (var target in colliders)
        {
            if (target.CompareTag("Enemy"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    //範囲表示
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _AttackRange);
        //Gizmos.DrawWireCube(transform.position, new Vector3(_AttackRange, 20, 20));
    }

    //------------------------------------------------------------

    //エネミーの攻撃を受ける
    public void _AddDamage(int _damage)
    {
        _Life -= _damage;
        anim.SetTrigger("TakeDamage");
        if(_Life <= 0)
        {
            isDead = true;
        }
    }

    //------------------------------------------------------------
    //AnimationEvent用
    //城を破壊する
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        Destroy(gameObject, 5);
    }

    //ダメージを与える
    public void Attack()
    {
        if(attackTarget == null) { return; }
        //弾生成
        //var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        //bullet.GetComponent<CastleBullet>()._SetTarget(attackTarget.transform.position, 0.5f);
        //エネミー側の関数
        attackTarget.GetComponent<IPlayerDamege>()._AddDamege(1);
    }

    //動画と合わせる
    public void RevivalPlayer()
    {
        Instantiate(_PlayerPrefab);
        castleState = CastleState.MOVE;
    }
}

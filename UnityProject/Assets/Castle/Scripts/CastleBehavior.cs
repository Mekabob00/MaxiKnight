using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastleState { MOVE, TAKEDAMAGE, ATTACK, PLAYERREVIVAL, DEAD }

public class CastleBehavior : MonoBehaviour
{
    #region 変数
    [Header("生命値")]
    public int _Life;

    [Header("援護射撃範囲")]
    public int _AttackRange;

    [Header("攻撃中心")]
    [SerializeField] private GameObject attackPoint;

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
    [SerializeField] private bool skipDamageAnim;

    [Header("城状態")]
    [SerializeField] private CastleState castleState;

    [Header("攻撃対象")]
    [SerializeField] private GameObject attackTarget; //援護射撃対象

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SearchEnemy();
        SwitchState();
        SwitchAnim();
        //Debug.Log(castleState.ToString());
    }

    void SwitchState()
    {
        //攻撃モード移行可能
        if (GlobalData.Instance.isPlayerInSecondLine && SearchEnemy())
            isAttack = true;
        else
            isAttack = false;


        if (GlobalData.Instance.isPlayerDead || isDead)
            skipDamageAnim = true;

        //Debug.Log(SearchEnemy());
        //行動
        switch (castleState)
        {
            case CastleState.MOVE:
                {
                    #region 状態遷移
                    if (isDead)
                        castleState = CastleState.DEAD;
                    else if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
                        castleState = CastleState.TAKEDAMAGE;
                    else if (GlobalData.Instance.isPlayerDead)
                    {
                        castleState = CastleState.PLAYERREVIVAL;
                        anim.SetTrigger("Revival");
                        GlobalData.Instance.isPlayerDead = false;
                    }
                    else if (isAttack && SearchEnemy())
                        castleState = CastleState.ATTACK;
                    #endregion
                    break;
                }
            case CastleState.TAKEDAMAGE:
                {
                    #region 状態遷移
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") || skipDamageAnim)
                    {
                        skipDamageAnim = false;
                        if (isDead)
                            castleState = CastleState.DEAD;
                        else if (GlobalData.Instance.isPlayerDead)
                        {
                            castleState = CastleState.PLAYERREVIVAL;
                            anim.SetTrigger("Revival");
                            GlobalData.Instance.isPlayerDead = false;
                        }
                        else if (isAttack && SearchEnemy())
                            castleState = CastleState.ATTACK;
                        else
                            castleState = CastleState.MOVE;
                    }
                    #endregion
                    break;
                }
            case CastleState.ATTACK:
                {
                    #region 攻撃
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        timer = _AttackCT;
                        Attack();
                    }
                    #endregion

                    //敵がない場合攻撃モードに退出
                    //if (!SearchEnemy())
                    //    isAttack = false;

                    #region 状態遷移
                    if (isDead)
                        castleState = CastleState.DEAD;
                    else if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
                        castleState = CastleState.TAKEDAMAGE;
                    else if (GlobalData.Instance.isPlayerDead)
                    {
                        castleState = CastleState.PLAYERREVIVAL;
                        anim.SetTrigger("Revival");
                        GlobalData.Instance.isPlayerDead = false;
                    }
                    else if(!isAttack)
                        castleState = CastleState.MOVE;
                    #endregion

                    break;
                }
            case CastleState.PLAYERREVIVAL:
                {
                    #region 状態遷移
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRevival"))
                    {
                        if (isDead)
                            castleState = CastleState.DEAD;
                        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
                            castleState = CastleState.TAKEDAMAGE;
                        else if (isAttack && SearchEnemy())
                            castleState = CastleState.ATTACK;
                        else
                            castleState = CastleState.MOVE;
                    }
                    #endregion
                    break;
                }
            case CastleState.DEAD:
                isDead = false;
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
        #region 古いバージョン
        //円型捜索
        //var colliders = Physics.OverlapSphere(transform.position, _AttackRange);
        //矩形捜索
        //var colliders = Physics.OverlapBox(transform.position, new Vector3(_AttackRange, 20, 20));

        //敵記録
        //foreach (var target in colliders)
        //{
        //    if (target.CompareTag("Enemy"))
        //    {
        //        attackTarget = target.gameObject;
        //        return true;
        //    }
        //}
        //attackTarget = null;
        //return false;
        #endregion

        //円型捜索
        var colliders = Physics.OverlapSphere(transform.position, _AttackRange, LayerMask.GetMask("Enemy"));

        if (colliders.Length <= 0)
        {
            attackTarget = null;
            return false;
        }

        //昇順ソート
        if (colliders.Length > 1)
        {
            for (int i = colliders.Length - 1; i > 0; --i)
            {
                for (int j = 0; j <= i - 1; ++j)
                {
                    if (Vector3.Distance(transform.position, colliders[j].transform.position) > Vector3.Distance(transform.position, colliders[j + 1].transform.position))
                    {
                        var temp = colliders[j];
                        colliders[j] = colliders[j + 1];
                        colliders[j + 1] = temp;
                    }
                }
            }
        }
        attackTarget = colliders[0].gameObject;
        return true;
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
        skipDamageAnim = false;
        if (_Life <= 0)
            isDead = true;
    }

    //------------------------------------------------------------
    //AnimationEvent用
    //城を破壊する
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        Destroy(gameObject);
    }

    //ダメージを与える
    public void Attack()
    {
        if (attackTarget == null) { return; }
        //弾生成
        var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<CastleBullet>()._SetTarget(attackTarget.transform.position, 0.5f);
        //エネミー側の関数(弾発射なし)
        //attackTarget.GetComponent<IPlayerDamege>()._AddDamege(1);
    }

    //動画と合わせる
    public void RevivalPlayer()
    {
        Instantiate(_PlayerPrefab);
    }
}

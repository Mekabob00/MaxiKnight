using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CastleBehavior : MonoBehaviour
{
    #region 変数
    [Header("生命値")]
    public int _Health;
    [Header("援護射撃範囲")]
    public int _AttackRange;
    [Header("攻撃中心")]
    [SerializeField] private GameObject attackPoint;
    [Header("攻撃クールターム")]
    public float _AttackCT;
    [Header("プレイヤー")]
    public GameObject _PlayerPrefab;
    [Header("弾")]
    public GameObject _BulletPrefab;
    [Header("HPBar")]
    public CastleHPBar m_hpBar;

    //プライベート変数
    private Animator m_animator;
    [Header("攻撃対象")]
    [SerializeField] private GameObject m_attackTarget; //援護射撃対象
    enum CastleState { MOVE, TAKEDAMAGE, ATTACK, PLAYERREVIVAL, DEAD }
    [Header("城状態")]
    [SerializeField] private CastleState m_castleState;
    private float m_timer;    //タイム計算
    private bool m_isAttack;
    private bool m_isDead;
    [SerializeField] private bool m_skipDamageAnim;

    #endregion

    void Start()
    {
        //m_animator = GetComponent<Animator>();
        m_hpBar.SetMaxHealth(_Health);
    }

    void Update()
    {
        SwitchState();
        Attack();
        //SwitchAnim();
    }

    void SwitchState()
    {


        #region 過去バージョン
        ////攻撃モード移行可能
        //if (GlobalData.Instance.isPlayerInSecondLine && SearchEnemy())
        //    m_isAttack = true;
        //else
        //    m_isAttack = false;
        //
        //
        //if (GlobalData.Instance.isPlayerDead || m_isDead)
        //    m_skipDamageAnim = true;
        //
        ////Debug.Log(SearchEnemy());
        ////行動
        //switch (m_castleState)
        //{
        //    case CastleState.MOVE:
        //        {
        //            #region 状態遷移
        //            if (m_isDead)
        //                m_castleState = CastleState.DEAD;
        //            else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        //                m_castleState = CastleState.TAKEDAMAGE;
        //            else if (GlobalData.Instance.isPlayerDead)
        //            {
        //                m_castleState = CastleState.PLAYERREVIVAL;
        //                m_animator.SetTrigger("Revival");
        //                GlobalData.Instance.isPlayerDead = false;
        //            }
        //            else if (m_isAttack && SearchEnemy())
        //                m_castleState = CastleState.ATTACK;
        //            #endregion
        //            break;
        //        }
        //    case CastleState.TAKEDAMAGE:
        //        {
        //            #region 状態遷移
        //            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage") || m_skipDamageAnim)
        //            {
        //                m_skipDamageAnim = false;
        //                if (m_isDead)
        //                    m_castleState = CastleState.DEAD;
        //                else if (GlobalData.Instance.isPlayerDead)
        //                {
        //                    m_castleState = CastleState.PLAYERREVIVAL;
        //                    m_animator.SetTrigger("Revival");
        //                    GlobalData.Instance.isPlayerDead = false;
        //                }
        //                else if (m_isAttack && SearchEnemy())
        //                    m_castleState = CastleState.ATTACK;
        //                else
        //                    m_castleState = CastleState.MOVE;
        //            }
        //            #endregion
        //            break;
        //        }
        //    case CastleState.ATTACK:
        //        {
        //            #region 攻撃
        //            m_timer -= Time.deltaTime;
        //            if (m_timer <= 0)
        //            {
        //                m_timer = _AttackCT;
        //                Attack();
        //            }
        //            #endregion

        //            //敵がない場合攻撃モードに退出
        //            //if (!SearchEnemy())
        //            //    isAttack = false;

        //            #region 状態遷移
        //            if (m_isDead)
        //                m_castleState = CastleState.DEAD;
        //            else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        //                m_castleState = CastleState.TAKEDAMAGE;
        //            else if (GlobalData.Instance.isPlayerDead)
        //            {
        //                m_castleState = CastleState.PLAYERREVIVAL;
        //                m_animator.SetTrigger("Revival");
        //                GlobalData.Instance.isPlayerDead = false;
        //            }
        //            else if (!m_isAttack)
        //                m_castleState = CastleState.MOVE;
        //            #endregion

        //            break;
        //        }
        //    case CastleState.PLAYERREVIVAL:
        //        {
        //            #region 状態遷移
        //            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerRevival"))
        //            {
        //                if (m_isDead)
        //                    m_castleState = CastleState.DEAD;
        //                else if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
        //                    m_castleState = CastleState.TAKEDAMAGE;
        //                else if (m_isAttack && SearchEnemy())
        //                    m_castleState = CastleState.ATTACK;
        //                else
        //                    m_castleState = CastleState.MOVE;
        //            }
        //            #endregion
        //            break;
        //        }
        //    case CastleState.DEAD:
        //        m_isDead = false;
        //        break;
        //}
        #endregion
    }

    void Attack()
    {
        if (!SearchEnemy()) { return; }
        //弾生成
        var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<CastleBullet>()._SetTarget(m_attackTarget.transform.position, 0.5f);
        //エネミー側の関数(弾発射なし)
        //attackTarget.GetComponent<IPlayerDamege>()._AddDamege(1);
    }

    void SwitchAnim()
    {
        m_animator.SetBool("Attack", m_isAttack);
        m_animator.SetBool("Death", m_isDead);
    }

    //------------------------------------------------------------

    //敵捜索
    bool SearchEnemy()
    {
        //円型捜索
        var colliders = Physics.OverlapSphere(transform.position, _AttackRange, LayerMask.GetMask("Enemy"));

        if (colliders.Length <= 0)
        {
            m_attackTarget = null;
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
        m_attackTarget = colliders[0].gameObject;
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
        _Health -= _damage;
        //m_animator.SetTrigger("TakeDamage");
        m_hpBar.SetCurrentHealth(_Health);
        m_skipDamageAnim = false;
        if (_Health <= 0)
        {
            m_isDead = true;
            GlobalData.Instance.isGameOver = true;
            Destroy(gameObject);
        }
    }

    //------------------------------------------------------------
    //AnimationEvent用
    //城を破壊する
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        Destroy(gameObject);
    }

    //動画と合わせる
    public void RevivalPlayer()
    {
        Instantiate(_PlayerPrefab);
    }
}

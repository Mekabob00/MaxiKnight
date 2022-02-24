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
    [Header("オブジェクト")]
    public GameObject _AttackPoint;
    public GameObject _Explosion;
    public GameObject _PlayerPrefab;
    public GameObject _BulletPrefab;
    public Transform[] _ExplosionPos;
    public GameObject _DamageEffect;
    [Header("ターム")]
    public float _AttackCT;
    public float _ExplosionCT;
    [Header("HPBar")]
    public CastleHPBar m_hpBar;


    //プライベート変数
    Animator m_animator;
    [Header("攻撃対象")]
    [SerializeField] GameObject m_attackTarget; //援護射撃目標
    enum CastleState { MOVE, ATTACK, DEAD }
    [Header("城状態")]
    [SerializeField] CastleState m_castleState;
    float m_timer;    //タイム計算用タイマー
    bool m_canAttack;
    #endregion

    void Start()
    {
        _Health = DataManager.Instance._CastleHP;
        m_hpBar.SetMaxHealth(_Health);
    }

    void Update()
    {
        SwitchState();
    }

    void SwitchState()
    {
        //攻撃モード移行可能
        if (SearchEnemy())
            m_canAttack = true;
        else
            m_canAttack = false;

        //Debug.Log(SearchEnemy());
        //行動
        switch (m_castleState)
        {
            case CastleState.MOVE:
                {
                    #region 状態遷移
                    if (m_canAttack && SearchEnemy())
                        m_castleState = CastleState.ATTACK;
                    #endregion
                    break;
                }
            case CastleState.ATTACK:
                {
                    #region 攻撃
                    m_timer -= Time.deltaTime;
                    if (m_timer <= 0)
                    {
                        m_timer = _AttackCT;
                        Attack();
                    }
                    #endregion

                    //敵がない場合攻撃モードに退出
                    //if (!SearchEnemy())
                    //    isAttack = false;

                    #region 状態遷移
                    if (!m_canAttack)
                        m_castleState = CastleState.MOVE;
                    #endregion

                    break;
                }
            case CastleState.DEAD:
                m_timer -= Time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = _ExplosionCT;
                    CreateExplosion();
                }
                break;
        }
    }

    void CreateExplosion()
    {
        int num = Random.Range(1, _ExplosionPos.Length / 2);
        while(num > 0)
        {
            GameObject obj = Instantiate(_Explosion, _ExplosionPos[Random.Range(0, _ExplosionPos.Length)]);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            num--;
        }
    }

    void Attack()
    {
        if (!SearchEnemy()) { return; }
        //弾生成
        var bullet = Instantiate(_BulletPrefab, _AttackPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<CastleBullet>()._SetTarget(m_attackTarget, 0.5f);
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
        if (m_castleState == CastleState.DEAD) return;

        _Health -= _damage;
        DataManager.Instance._CastleHP = _Health;
        m_hpBar.SetCurrentHealth(_Health);
        if (_Health <= 0)
        {
            m_castleState = CastleState.DEAD;
            GlobalData.Instance.isGameOver = true;
        }
    }
}

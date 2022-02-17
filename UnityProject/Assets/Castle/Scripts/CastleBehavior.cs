using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CastleBehavior : MonoBehaviour
{
    #region �ϐ�
    [Header("�����l")]
    public int _Health;
    [Header("����ˌ��͈�")]
    public int _AttackRange;
    [Header("�U�����S")]
    [SerializeField] private GameObject attackPoint;
    [Header("�U���N�[���^�[��")]
    public float _AttackCT;
    [Header("�v���C���[")]
    public GameObject _PlayerPrefab;
    [Header("�e")]
    public GameObject _BulletPrefab;
    [Header("HPBar")]
    public CastleHPBar m_hpBar;

    //�v���C�x�[�g�ϐ�
    Animator m_animator;
    [Header("�U���Ώ�")]
    [SerializeField] GameObject m_attackTarget; //����ˌ��ڕW
    enum CastleState { MOVE, ATTACK, DEAD }
    [Header("����")]
    [SerializeField] CastleState m_castleState;
    float m_timer;    //�U���N�[���^�C���v�Z
    bool m_canAttack;
    bool m_isDead;
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
        //�U�����[�h�ڍs�\
        if (SearchEnemy())
            m_canAttack = true;
        else
            m_canAttack = false;

        //Debug.Log(SearchEnemy());
        //�s��
        switch (m_castleState)
        {
            case CastleState.MOVE:
                {
                    #region ��ԑJ��
                    if (m_isDead)
                        m_castleState = CastleState.DEAD;
                    else if (m_canAttack && SearchEnemy())
                        m_castleState = CastleState.ATTACK;
                    #endregion
                    break;
                }
            case CastleState.ATTACK:
                {
                    #region �U��
                    m_timer -= Time.deltaTime;
                    if (m_timer <= 0)
                    {
                        m_timer = _AttackCT;
                        Attack();
                    }
                    #endregion

                    //�G���Ȃ��ꍇ�U�����[�h�ɑޏo
                    //if (!SearchEnemy())
                    //    isAttack = false;

                    #region ��ԑJ��
                    if (m_isDead)
                        m_castleState = CastleState.DEAD;
                    else if (!m_canAttack)
                        m_castleState = CastleState.MOVE;
                    #endregion

                    break;
                }
            case CastleState.DEAD:
                m_isDead = false;
                break;
        }
    }

    void Attack()
    {
        if (!SearchEnemy()) { return; }
        //�e����
        var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<CastleBullet>()._SetTarget(m_attackTarget, 0.5f);
    }

    //------------------------------------------------------------

    //�G�{��
    bool SearchEnemy()
    {
        //�~�^�{��
        var colliders = Physics.OverlapSphere(transform.position, _AttackRange, LayerMask.GetMask("Enemy"));

        if (colliders.Length <= 0)
        {
            m_attackTarget = null;
            return false;
        }

        //�����\�[�g
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

    //�͈͕\��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _AttackRange);
        //Gizmos.DrawWireCube(transform.position, new Vector3(_AttackRange, 20, 20));
    }

    //------------------------------------------------------------

    //�G�l�~�[�̍U�����󂯂�
    public void _AddDamage(int _damage)
    {
        _Health -= _damage;
        DataManager.Instance._CastleHP = _Health;
        m_hpBar.SetCurrentHealth(_Health);
        if (_Health <= 0)
        {
            m_isDead = true;
            GlobalData.Instance.isGameOver = true;
        }
    }
}

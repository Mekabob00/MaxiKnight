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
    private Animator m_animator;
    [Header("�U���Ώ�")]
    [SerializeField] private GameObject m_attackTarget; //����ˌ��Ώ�
    enum CastleState { MOVE, TAKEDAMAGE, ATTACK, PLAYERREVIVAL, DEAD }
    [Header("����")]
    [SerializeField] private CastleState m_castleState;
    private float m_timer;    //�^�C���v�Z
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


        #region �ߋ��o�[�W����
        ////�U�����[�h�ڍs�\
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
        ////�s��
        //switch (m_castleState)
        //{
        //    case CastleState.MOVE:
        //        {
        //            #region ��ԑJ��
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
        //            #region ��ԑJ��
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
        //            #region �U��
        //            m_timer -= Time.deltaTime;
        //            if (m_timer <= 0)
        //            {
        //                m_timer = _AttackCT;
        //                Attack();
        //            }
        //            #endregion

        //            //�G���Ȃ��ꍇ�U�����[�h�ɑޏo
        //            //if (!SearchEnemy())
        //            //    isAttack = false;

        //            #region ��ԑJ��
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
        //            #region ��ԑJ��
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
        //�e����
        var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<CastleBullet>()._SetTarget(m_attackTarget.transform.position, 0.5f);
        //�G�l�~�[���̊֐�(�e���˂Ȃ�)
        //attackTarget.GetComponent<IPlayerDamege>()._AddDamege(1);
    }

    void SwitchAnim()
    {
        m_animator.SetBool("Attack", m_isAttack);
        m_animator.SetBool("Death", m_isDead);
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
    //AnimationEvent�p
    //���j�󂷂�
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        Destroy(gameObject);
    }

    //����ƍ��킹��
    public void RevivalPlayer()
    {
        Instantiate(_PlayerPrefab);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastleState { MOVE, TAKEDAMAGE, ATTACK, PLAYERREVIVAL, DEAD }

public class CastleBehavior : MonoBehaviour
{
    #region �ϐ�
    [Header("�����l")]
    public int _Life;

    [Header("����ˌ��͈�")]
    public int _AttackRange;

    [Header("�U�����S")]
    [SerializeField] private GameObject attackPoint;

    [Header("�U���N�[���^�[��")]
    public float _AttackCT;

    private float timer;

    [Header("�v���C���[")]
    public GameObject _PlayerPrefab;

    [Header("�e")]
    public GameObject _BulletPrefab;

    private Animator anim;

    private bool isAttack;
    private bool isDead;
    [SerializeField] private bool skipDamageAnim;

    [Header("����")]
    [SerializeField] private CastleState castleState;

    [Header("�U���Ώ�")]
    [SerializeField] private GameObject attackTarget; //����ˌ��Ώ�

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
        //Debug.Log(castleState.ToString());
    }

    void SwitchState()
    {
        //�U�����[�h�ڍs�\
        if (GlobalData.Instance.isPlayerInSecondLine && SearchEnemy())
            isAttack = true;
        else
            isAttack = false;


        if (GlobalData.Instance.isPlayerDead || isDead)
            skipDamageAnim = true;

        //Debug.Log(SearchEnemy());
        //�s��
        switch (castleState)
        {
            case CastleState.MOVE:
                {
                    #region ��ԑJ��
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
                    #region ��ԑJ��
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
                    #region �U��
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        timer = _AttackCT;
                        Attack();
                    }
                    #endregion

                    //�G���Ȃ��ꍇ�U�����[�h�ɑޏo
                    //if (!SearchEnemy())
                    //    isAttack = false;

                    #region ��ԑJ��
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
                    #region ��ԑJ��
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

    //�G�{��
    bool SearchEnemy()
    {
        #region �Â��o�[�W����
        //�~�^�{��
        //var colliders = Physics.OverlapSphere(transform.position, _AttackRange);
        //��`�{��
        //var colliders = Physics.OverlapBox(transform.position, new Vector3(_AttackRange, 20, 20));

        //�G�L�^
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

        //�~�^�{��
        var colliders = Physics.OverlapSphere(transform.position, _AttackRange, LayerMask.GetMask("Enemy"));

        if (colliders.Length <= 0)
        {
            attackTarget = null;
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
        attackTarget = colliders[0].gameObject;
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
        _Life -= _damage;
        anim.SetTrigger("TakeDamage");
        skipDamageAnim = false;
        if (_Life <= 0)
            isDead = true;
    }

    //------------------------------------------------------------
    //AnimationEvent�p
    //���j�󂷂�
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        Destroy(gameObject);
    }

    //�_���[�W��^����
    public void Attack()
    {
        if (attackTarget == null) { return; }
        //�e����
        var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<CastleBullet>()._SetTarget(attackTarget.transform.position, 0.5f);
        //�G�l�~�[���̊֐�(�e���˂Ȃ�)
        //attackTarget.GetComponent<IPlayerDamege>()._AddDamege(1);
    }

    //����ƍ��킹��
    public void RevivalPlayer()
    {
        Instantiate(_PlayerPrefab);
    }
}

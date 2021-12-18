using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastleState { MOVE, TAKEDAMAGE, ATTACK, PLAYERREVIVAL, DEAD}

public class CastleBehavior : MonoBehaviour
{
    //TODO
    //�ړ�
    //��_���[�W --�@�������i�G�l�~�[����̃_���[�W�j
    //�v���C���[���� -- ������
    //����ˌ� -- �������i�G�l�~�[�Ƀ_���[�W��^����j
    //�Q�[���I�[�o�[

    #region �ϐ�
    [Header("�����l")]
    public int _Life;

    [Header("����ˌ��͈�")]
    public int _AttackRange;

    [Header("�U�����S")]
    [SerializeField]private GameObject attackPoint;

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

    [Header("����")]
    [SerializeField] private CastleState castleState;

    [Header("�U���Ώ�")]
    [SerializeField]private GameObject attackTarget; //����ˌ��Ώ�

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
        #region ��ԕύX

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

        //�s��
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

                //�U��
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

    //�G�{��
    bool SearchEnemy()
    {
        //�~�^�{��
        var colliders = Physics.OverlapSphere(transform.position, _AttackRange);
        //��`�{��
        //var colliders = Physics.OverlapBox(transform.position, new Vector3(_AttackRange, 20, 20));
        //�G�L�^
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
        if(_Life <= 0)
        {
            isDead = true;
        }
    }

    //------------------------------------------------------------
    //AnimationEvent�p
    //���j�󂷂�
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        Destroy(gameObject, 5);
    }

    //�_���[�W��^����
    public void Attack()
    {
        if(attackTarget == null) { return; }
        //�e����
        //var bullet = Instantiate(_BulletPrefab, attackPoint.transform.position, Quaternion.identity);
        //bullet.GetComponent<CastleBullet>()._SetTarget(attackTarget.transform.position, 0.5f);
        //�G�l�~�[���̊֐�
        attackTarget.GetComponent<IPlayerDamege>()._AddDamege(1);
    }

    //����ƍ��킹��
    public void RevivalPlayer()
    {
        Instantiate(_PlayerPrefab);
        castleState = CastleState.MOVE;
    }
}

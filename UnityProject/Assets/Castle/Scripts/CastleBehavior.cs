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

    [Header("����")]
    public CastleState castleState;

    [Header("�����l")]
    public int life;

    [Header("����ˌ��͈�")]
    public int attackRange;

    [Header("�v���C���[�v���n�u")]
    public GameObject playerPrefab;

    private Animator anim;

    private bool isTakeDmage;
    private bool isAttack;
    private bool isDead;

    private GameObject attackTarget; //����ˌ��Ώ�

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
        if (isDead)
            castleState = CastleState.DEAD;

        if (GlobalData.Instance.isPlayerDead)
        {
            castleState = CastleState.PLAYERREVIVAL;
            Instantiate(playerPrefab);
        }
           
        switch (castleState)
        {
            case CastleState.MOVE:
                break;
            case CastleState.TAKEDAMAGE:
                
                break;
            case CastleState.ATTACK:
                if (attackTarget == null)
                    SearchEnemy();
                break;
            case CastleState.PLAYERREVIVAL:
                break;
            case CastleState.DEAD:
                break;
        }
    }

    void SwitchAnim()
    {
        anim.SetBool("TakeDamage", isTakeDmage);
        anim.SetBool("Attack", isAttack);
        anim.SetBool("Death", isDead);
    }

    bool SearchEnemy()
    {
        //�~�^�{��
        var colliders = Physics.OverlapSphere(transform.position, attackRange);
        //var colliders = Physics.OverlapBox(transform.position, new Vector3(5, 2, 2));
        foreach(var target in colliders)
        {
            if (target.CompareTag("Enemy"))
                attackTarget = target.gameObject;
            return true;
        }
        attackTarget = null;
        return false;
    }

    //�G�l�~�[�̍U�����󂯂�
    public void GetDamage()
    {
        life--;
        if(life <= 0)
        {
            isDead = true;
        }
    }

    //---------------------------------------------------------
    //AnimationEvent�p
    //���j�󂷂�
    public void CastleDestory()
    {
        GlobalData.Instance.isGameOver = true;
        //Destroy(gameObject);
    }
    //
    public void ReturnToMoveState()
    {
        castleState = CastleState.MOVE;
    }
}

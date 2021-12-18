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

    [Header("城状態")]
    public CastleState castleState;

    [Header("生命値")]
    public int life;

    [Header("援護射撃範囲")]
    public int attackRange;

    [Header("プレイヤープレハブ")]
    public GameObject playerPrefab;

    private Animator anim;

    private bool isTakeDmage;
    private bool isAttack;
    private bool isDead;

    private GameObject attackTarget; //援護射撃対象

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
        //円型捜索
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

    //エネミーの攻撃を受ける
    public void GetDamage()
    {
        life--;
        if(life <= 0)
        {
            isDead = true;
        }
    }

    //---------------------------------------------------------
    //AnimationEvent用
    //城を破壊する
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

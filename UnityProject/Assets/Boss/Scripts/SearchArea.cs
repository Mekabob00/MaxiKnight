using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [Header("çUåÇîÕàÕ")]
    public float _Area;
    [Header("óPó\éûä‘")]
    public float _SetTime;
    [Header("É_ÉÅÅ[ÉW")]
    public int _Damage;

    GameObject attackTarget;
    Animator anim;
    bool isAttack;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isAttack = false;
        transform.localScale = new Vector3(_Area, 0.1f, _Area);
    }

    private void Update()
    {
        _SetTime -= Time.deltaTime;
        if(_SetTime <= 0 && !isAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttack = true;
        anim.SetTrigger("Attack");
        if (SearchPlayer())
            attackTarget.GetComponent<Player_Controll>()._AddDamege(_Damage);
    }

    bool SearchPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, _Area / 2.0f);
        foreach(var target in colliders)
        {
            if (target.tag == "Player")
            {
                attackTarget = target.gameObject;
                Debug.Log("Attack Player");
                return true;
            }
        }
        return false;
    }

    //îÕàÕï\é¶
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _Area / 2.0f);
        //Gizmos.DrawWireCube(transform.position, new Vector3(_AttackRange, 20, 20));
    }

    public void DestoryThis()
    {
        Destroy(gameObject, 1.0f);
    }

}

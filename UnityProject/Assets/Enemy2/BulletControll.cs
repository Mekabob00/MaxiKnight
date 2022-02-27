using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour
{
    [SerializeField, Tooltip("リジットボディ")]
    private Rigidbody rigidbody;
    [SerializeField, Tooltip("玉のスピード")]
    private float Speed;
    [SerializeField, Tooltip("衝突時のエフェクト")]
    private GameObject Effect;
    [SerializeField, Tooltip("攻撃対象")]
    private GameObject Castle;
    [SerializeField, Tooltip("攻撃力")]
    private int _Damege;
    [SerializeField,Tooltip("攻撃SE")]
    private AudioClip AttackSE;
    void Start()
    {
        _Damege = 1;
        Castle = GameObject.FindGameObjectWithTag("Castle");
        rigidbody = GetComponent<Rigidbody>();
        Speed = 65.0f;
        rigidbody.velocity = transform.forward* Speed;
    }
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
      
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Castle")
        {
            EnemyAttackManeger.instance.PlaySE(AttackSE);
            Instantiate(Effect, transform.position, transform.rotation);
            Destroy(this.gameObject);
            Castle.GetComponent<CastleBehavior>()._AddDamage(_Damege);
        }
    }
}

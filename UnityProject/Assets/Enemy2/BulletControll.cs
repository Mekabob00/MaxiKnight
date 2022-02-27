using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControll : MonoBehaviour
{
    [SerializeField, Tooltip("���W�b�g�{�f�B")]
    private Rigidbody rigidbody;
    [SerializeField, Tooltip("�ʂ̃X�s�[�h")]
    private float Speed;
    [SerializeField, Tooltip("�Փˎ��̃G�t�F�N�g")]
    private GameObject Effect;
    [SerializeField, Tooltip("�U���Ώ�")]
    private GameObject Castle;
    [SerializeField, Tooltip("�U����")]
    private int _Damege;
    [SerializeField,Tooltip("�U��SE")]
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

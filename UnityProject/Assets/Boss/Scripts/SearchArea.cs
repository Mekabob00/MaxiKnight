using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    [Header("�U���͈�")]
    public float _Area;
    [Header("�_���[�W")]
    public int _Damage;
    [Header("�I�u�W�F�N�g")]
    public GameObject _Effect;
    public GameObject _NapalmBomb;

    GameObject m_attackTarget;
    Vector3 m_velocuty; //���x
    Vector3 m_accleration; //�����x
    bool m_canAttack;

    private void Start()
    {
        m_canAttack = false;
        m_velocuty = Vector3.zero;
        m_accleration = new Vector3(0, -30, 0); //3�{�d��
        transform.localScale = new Vector3(_Area, 0.1f, _Area);
    }

    private void Update()
    {
        if (_NapalmBomb != null)
        {
            //�����x�͎��Ԃ̕�����
            m_velocuty += m_accleration * Time.deltaTime;
            _NapalmBomb.transform.position += m_velocuty * Time.deltaTime;
        }

        if (m_canAttack && SearchPlayer())
        {
            Attack();
        }
    }

    void Attack()
    {
        m_canAttack = false;
        m_attackTarget.GetComponent<Player_Controll>()._AddDamege(_Damage);
    }

    bool SearchPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, _Area / 2.0f);
        foreach(var target in colliders)
        {
            if (target.tag == "Player")
            {
                m_attackTarget = target.gameObject;
                Debug.Log("Attack Player");
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NapalmBomb")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject, 1f);
            GameObject temp = Instantiate(_Effect, transform.position, _Effect.transform.rotation);
            temp.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
            //temp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            m_canAttack = true;
        }
    }

    //�͈͕\��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _Area / 2.0f);
        //Gizmos.DrawWireCube(transform.position, new Vector3(_AttackRange, 20, 20));
    }
}

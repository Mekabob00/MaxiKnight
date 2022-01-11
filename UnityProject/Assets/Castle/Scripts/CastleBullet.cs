using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBullet : MonoBehaviour
{
    [SerializeField] private Vector3 attackTarget;
    [SerializeField] private float moveTime;

    private void Awake()
    {
        attackTarget = Vector3.zero;
        moveTime = 0;
    }

    void Start()
    {
        if(attackTarget == null) { Destroy(gameObject); }
        StartCoroutine(MoveTo());
    }

    IEnumerator MoveTo()
    {
        float t = 0;
        //float dis = Time.deltaTime / moveTime;
        while (true)
        {
            t += Time.deltaTime;
            float a = t / moveTime;
            transform.position = Vector3.Lerp(transform.position, attackTarget, a);
            if (a >= 1.0f)
                break;
            yield return null;
        }
    }

    public void _SetTarget(Vector3 _target, float _time)
    {
        attackTarget = _target;
        moveTime = _time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IPlayerDamege>()._AddDamege(1);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBullet : MonoBehaviour
{
    private Vector3 attackTarget;
    private float moveTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTo());
    }

    IEnumerator MoveTo()
    {
        float t = 0;
        float dis = Time.deltaTime / moveTime;
        while (true)
        {
            t += Time.deltaTime;
            //float dis = t / moveTime;
            transform.position = Vector3.Lerp(transform.position, attackTarget, dis);
            if (t >= moveTime)
            {
                Destroy(gameObject);
                break;
            }
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
       // if (other.CompareTag("Enemy"))
            //Destroy(gameObject);
    }
}

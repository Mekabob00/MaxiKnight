using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Con : MonoBehaviour
{
    public GameObject Castle;
    public int HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = EnemyAttackManeger.instance.CastleHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP >1)
        {
            Destroy(gameObject);
        }
    }
}

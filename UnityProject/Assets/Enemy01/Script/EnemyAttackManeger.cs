using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManeger : MonoBehaviour
{
    public static EnemyAttackManeger instance = null;

    public int Enemy1Attack;
    public int CastleHP=10;
    public GameObject Castle;
    // Start is called before the first frame
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Enemy1Attack = 1;
        CastleHP = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if(CastleHP<=0)
        {
            CastleHP = 0;
            Destroy(Castle);
        }
    }
}

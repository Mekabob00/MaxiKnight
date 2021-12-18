using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("砲弾出現")]
    public bool isCanonAppear;
    [Header("援護射撃起動")]
    public bool isPlayerInSecondLine;
    [Header("プレイヤ死亡/消滅")]
    public bool isPlayerDead;
    [Header("ゲームオーバー")]
    public bool isGameOver;
    
    //初期化
    public GlobalData()
    {
        isCanonAppear = false;
        isPlayerInSecondLine = false;
        isPlayerDead = false;
        isGameOver = false;
    }
}

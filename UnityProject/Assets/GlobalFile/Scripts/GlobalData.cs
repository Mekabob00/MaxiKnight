using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    [Header("砲弾出現")]
    public bool isCanonAppear;
    [Header("ゲームオーバー")]
    public bool isGameOver;
    [Header("ステージクリア")]
    public bool isStageClear;

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

    //初期化
    private void Start()
    {
        isCanonAppear = false;
        isGameOver = false;
        isStageClear = false;
    }

    public void ResetData()
    {
        isStageClear = false;
        isGameOver = false;
    }
}

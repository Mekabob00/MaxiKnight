using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public struct CurrentState
    {
        int _PlayerAttackBuff;
        int _CastleHP;
        int _CastleAttackBuff;
        int _Resource;
        int _CastleRecoveryFee;
        int _CastleAttackBuffFee;
        int _PlayerAttackBuffFee;
    } 

    public static DataManager Instance;

    [Header("プレイヤー関係")]
    [Tooltip("接近武器番号")] public int _WeaponNumberSword;
    [Tooltip("遠距離武器番号")] public int _WeaponNumberGun;
    [Tooltip("攻撃力バフ")] public int _PlayerAttackBuff;

    [Header("拠点関係")]
    [Tooltip("HP")] public int _CastleHP;
    [Tooltip("最大HP")] public int _CastleMaxHP;
    [Tooltip("攻撃力バフ")] public int _CastleAttackBuff;

    [Header("ゲーム関係")]
    [Tooltip("資源")] public int _Resource;
    [Tooltip("ステージ")] public int _Stage;
    [Tooltip("最大ステージ")] public int _MaxStage;

    [Header("ショップ関係")]
    [Tooltip("拠点回復費用")] public int _CastleRecoveryFee;
    [Tooltip("拠点強化費用")] public int _CastleAttackBuffFee;
    [Tooltip("プレイヤー強化費用")] public int _PlayerAttackBuffFee;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        //初期数値設定
        Instance._WeaponNumberSword = 1;
        Instance._WeaponNumberGun = 1;
        Instance._PlayerAttackBuff = 1;
        Instance._CastleAttackBuff = 1;
        Instance._Stage = 1;
        Instance._Resource = 100;
        Instance._CastleHP = Instance._CastleMaxHP;
        Instance._CastleRecoveryFee = 100;
        Instance._CastleAttackBuffFee = 100;
        Instance._PlayerAttackBuffFee = 100;

        DontDestroyOnLoad(this);
    }

    public void Reset()
    {
        Instance._WeaponNumberSword = 1;
        Instance._WeaponNumberGun = 1;
        Instance._PlayerAttackBuff = 1;
        Instance._CastleAttackBuff = 1;
        Instance._Stage = 1;
        Instance._Resource = 100;
        Instance._CastleHP = Instance._CastleMaxHP;
        Instance._CastleRecoveryFee = 100;
        Instance._CastleAttackBuffFee = 100;
        Instance._PlayerAttackBuffFee = 100;
    }
}

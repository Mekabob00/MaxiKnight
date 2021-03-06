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

    [Header("vC[ÖW")]
    [Tooltip("ÚßíÔ")] public int _WeaponNumberSword;
    [Tooltip("£íÔ")] public int _WeaponNumberGun;
    [Tooltip("UÍot")] public int _PlayerAttackBuff;

    [Header("_ÖW")]
    [Tooltip("HP")] public int _CastleHP;
    [Tooltip("ÅåHP")] public int _CastleMaxHP;
    [Tooltip("UÍot")] public int _CastleAttackBuff;

    [Header("Q[ÖW")]
    [Tooltip("¹")] public int _Resource;
    [Tooltip("Xe[W")] public int _Stage;
    [Tooltip("ÅåXe[W")] public int _MaxStage;

    [Header("VbvÖW")]
    [Tooltip("_ñïp")] public int _CastleRecoveryFee;
    [Tooltip("_­»ïp")] public int _CastleAttackBuffFee;
    [Tooltip("vC[­»ïp")] public int _PlayerAttackBuffFee;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        //úlÝè
        Instance._WeaponNumberSword = 0;
        Instance._WeaponNumberGun = 0;
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
        Instance._WeaponNumberSword = 0;
        Instance._WeaponNumberGun = 0;
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

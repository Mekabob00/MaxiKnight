using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //ショップ内各種ボタン用の関数

    //拠点回復
    public void Button_CastleRecovery()
    {
        if (DataManager.Instance._Resource < DataManager.Instance._CastleRecoveryFee) return;

        DataManager.Instance._Resource -= DataManager.Instance._CastleRecoveryFee;
        DataManager.Instance._CastleRecoveryFee += 100;
        DataManager.Instance._CastleHP += 1;
    }
    //拠点強化
    public void Button_CastleAttackBuff()
    {
        if (DataManager.Instance._Resource < DataManager.Instance._CastleAttackBuffFee) return;

        DataManager.Instance._Resource -= DataManager.Instance._CastleAttackBuffFee;
        DataManager.Instance._CastleAttackBuffFee += 100;
        DataManager.Instance._CastleAttackBuff += 1;
    }
    //プレイヤー強化
    public void Button_PlayerAttackBuff()
    {
        if (DataManager.Instance._Resource < DataManager.Instance._PlayerAttackBuffFee) return;

        DataManager.Instance._Resource -= DataManager.Instance._PlayerAttackBuffFee;
        DataManager.Instance._PlayerAttackBuffFee += 100;
        DataManager.Instance._PlayerAttackBuff += 1;
    }

    //接近戦武器変更
    public void Button_WeaponChangeSword1()
    {
        DataManager.Instance._WeaponNumberSword = 1;
    }
    public void Button_WeaponChangeSword2()
    {
        DataManager.Instance._WeaponNumberSword = 2;
    }
    public void Button_WeaponChangeSword3()
    {
        DataManager.Instance._WeaponNumberSword = 3;
    }

    //遠距離武器変更
    public void Button_WeaponChangeGun1()
    {
        DataManager.Instance._WeaponNumberGun = 1;
    }
    public void Button_WeaponChangeGun2()
    {
        DataManager.Instance._WeaponNumberGun = 2;
    }
    public void Button_WeaponChangeGun3()
    {
        DataManager.Instance._WeaponNumberGun = 3;
    }
}

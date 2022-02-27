﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text text;
    public Text _OwnResource;
    public Text _CastleRecoveryFee;
    public Text _CastleAttackBuffFee;
    public Text _PlayerAttackBuffFee;

    public GameObject ShousaiImage;

    public enum SELECTITEM { NON, CASTLERECOVERY, CASTLEATTACKBUFF, PLAYERATTACKBUFF };
    SELECTITEM selectItem;

    private void Start()
    {
        selectItem = SELECTITEM.NON;
        _OwnResource.text = DataManager.Instance._Resource.ToString();
        _CastleRecoveryFee.text = DataManager.Instance._CastleRecoveryFee.ToString();
        _CastleAttackBuffFee.text = DataManager.Instance._PlayerAttackBuffFee.ToString();
        _PlayerAttackBuffFee.text = DataManager.Instance._PlayerAttackBuffFee.ToString();
    }

    private void Update()
    {
        switch (selectItem)
        {
            case SELECTITEM.NON:
                text.text = "商品を選んでください。";
                break;
            case SELECTITEM.CASTLERECOVERY:
                text.text = "拠点を修理して、\n耐久度を最大値まで回復させます。";
                break;
            case SELECTITEM.CASTLEATTACKBUFF:
                text.text = "拠点を強化して、\n拠点の攻撃力を上昇させます。";
                break;
            case SELECTITEM.PLAYERATTACKBUFF:
                text.text = "機体を強化して、\n機体の攻撃力を上昇させます。";
                break;
        }
        TextUpDate();
    }

    #region ショップ機能関数
    //資源表示更新
    void TextUpDate()
    {
        _OwnResource.text = DataManager.Instance._Resource.ToString();
        _CastleRecoveryFee.text = DataManager.Instance._CastleRecoveryFee.ToString();
        _CastleAttackBuffFee.text = DataManager.Instance._CastleAttackBuffFee.ToString();
        _PlayerAttackBuffFee.text = DataManager.Instance._PlayerAttackBuffFee.ToString();
    }
    //拠点回復
    void CastleRecovery()
    {
        if (DataManager.Instance._Resource < DataManager.Instance._CastleRecoveryFee) return;
        if (DataManager.Instance._CastleHP >= DataManager.Instance._CastleMaxHP) return;

        DataManager.Instance._Resource -= DataManager.Instance._CastleRecoveryFee;
        DataManager.Instance._CastleRecoveryFee += 100;
        DataManager.Instance._CastleHP += 1;
    }
    //拠点強化
    void CastleAttackBuff()
    {
        if (DataManager.Instance._Resource < DataManager.Instance._CastleAttackBuffFee) return;


        DataManager.Instance._Resource -= DataManager.Instance._CastleAttackBuffFee;
        DataManager.Instance._CastleAttackBuffFee += 100;
        DataManager.Instance._CastleAttackBuff += 1;
    }
    //プレイヤー強化
    void PlayerAttackBuff()
    {
        if (DataManager.Instance._Resource < DataManager.Instance._PlayerAttackBuffFee) return;

        DataManager.Instance._Resource -= DataManager.Instance._PlayerAttackBuffFee;
        DataManager.Instance._PlayerAttackBuffFee += 100;
        DataManager.Instance._PlayerAttackBuff += 1;
    }
    #endregion

    #region ショップ内各種ボタン用の関数
    //購入
    public void Button_Buy()
    {
        switch (selectItem)
        {
            case SELECTITEM.CASTLERECOVERY:
                CastleRecovery();
                break;
            case SELECTITEM.CASTLEATTACKBUFF:
                CastleAttackBuff();
                break;
            case SELECTITEM.PLAYERATTACKBUFF:
                PlayerAttackBuff();
                break;
            default:
                break;
        }
    }

    public void Button_SelectItem(string select_)
    {
        switch (select_)
        {
            case "Non":
                selectItem = SELECTITEM.NON;
                break;
            case "CastleRecovery":
                ShousaiImage.GetComponent<ShousaiImage>().Status = 1;

                selectItem = SELECTITEM.CASTLERECOVERY;
                break;
            case "CastleAttackBuff":
                ShousaiImage.GetComponent<ShousaiImage>().Status = 2;

                selectItem = SELECTITEM.CASTLEATTACKBUFF;
                break;
            case "PlayerAttackBuff":
                ShousaiImage.GetComponent<ShousaiImage>().Status = 3;

                selectItem = SELECTITEM.PLAYERATTACKBUFF;
                break;
        }
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

    public void Button_NextStage()
    {
        GlobalData.Instance.ResetData();
        FadeManager.Instance.LoadScene(DataManager.Instance._Stage, 1.0f);
    }
    #endregion
}
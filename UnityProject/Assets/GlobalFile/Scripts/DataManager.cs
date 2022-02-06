using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("�v���C���[�֌W")]
    [Tooltip("�ڋߕ���ԍ�")] public int _WeaponNumberSword;
    [Tooltip("����������ԍ�")] public int _WeaponNumberGun;
    [Tooltip("�U���̓o�t")] public int _PlayerAttackBuff;

    [Header("���_�֌W")]
    [Tooltip("HP")] public int _CastleHP;
    [Tooltip("�ő�HP")] public int _CastleMaxHP;
    [Tooltip("�U���̓o�t")] public int _CastleAttackBuff;

    [Header("�Q�[���֌W")]
    [Tooltip("����")] public int _Resource;
    [Tooltip("�X�e�[�W")] public int _Stage;

    [Header("�V���b�v�֌W")]
    [Tooltip("���_�񕜔�p")] public int _CastleRecoveryFee;
    [Tooltip("���_������p")] public int _CastleAttackBuffFee;
    [Tooltip("�v���C���[������p")] public int _PlayerAttackBuffFee;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    //�������l�ݒ�
    void Start()
    {
        Instance._WeaponNumberSword = 1;
        Instance._WeaponNumberGun = 1;
        Instance._Stage = 1;
        Instance._Resource = 0;
        Instance._CastleHP = 10;
        Instance._CastleMaxHP = 10;
        Instance._CastleRecoveryFee = 100;
        Instance._CastleAttackBuffFee = 100;
        Instance._PlayerAttackBuffFee = 100;
    }
}

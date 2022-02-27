using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    [Header("�C�e�o��")]
    public bool isCanonAppear;
    [Header("�Q�[���I�[�o�[")]
    public bool isGameOver;
    [Header("�X�e�[�W�N���A")]
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

    public void ResetData()
    {
        isStageClear = false;
        isGameOver = false;
    }
}

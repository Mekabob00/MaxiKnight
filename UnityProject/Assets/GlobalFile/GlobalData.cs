using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    [Header("�C�e�o��")]
    public bool isCanonAppear;
    [Header("����ˌ��N��")]
    public bool isPlayerInSecondLine;
    [Header("�v���C�����S/����")]
    public bool isPlayerDead;
    [Header("�Q�[���I�[�o�[")]
    public bool isGameOver;

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

    //������
    private void Start()
    {
        isCanonAppear = false;
        isPlayerInSecondLine = false;
        isPlayerDead = false;
        isGameOver = false;
    }
}

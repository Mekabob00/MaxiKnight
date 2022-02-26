using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReinforcementTutorial : MonoBehaviour
{
    [SerializeField, Tooltip("�e�L�X�g�E�C���h�E")]
    private GameObject TextWindow;
    [SerializeField, Tooltip("�����̃X�s�[�h")]
    private float nowDispCount = 0.0f; //���݉������ڂ܂ŕ\�����邩�̃J�E���^�[
    [SerializeField, Tooltip("�X�L�b�v�̑I���I�u�W�F�N�g")]
    private GameObject SkipObject;
    [SerializeField, Tooltip("Shop�{�^��")]
    private Button Shop;
    [SerializeField, Tooltip("����ύX�{�^��")]
    private Button WeaponChange;
    [SerializeField, Tooltip("�퓬�փ{�^��")]
    private Button NextStage;
    [SerializeField, Tooltip("��̉񕜃{�^��")]
    private Button CastleRecovery;
    [SerializeField, Tooltip("��̍U���̓A�b�v�{�^��")]
    private Button CastleAttackBuff;
    [SerializeField, Tooltip("Player�̍U���̓A�b�v�{�^��")]
    private Button PlayerAttackBuff;
    [SerializeField, Tooltip("�Z���{�^��")]
    private Button ShortSowrd;
    [SerializeField, Tooltip("����")]
    private Button buy;
    public Text text = null;
    private int TutorialNum;
    private string maxDispStr = ""; //�\�������������e�̕�����
    private string nowDispStr = ""; //���ۂɉ�ʂɕ\��������p�̕�����

    #region �{�^���t���O�ϐ��錾
    public bool case5;
    public bool case8;
    public bool case16;
    public bool case10;
    public bool case15;
    public bool case18;
    public bool case20;
    public bool case22;
    #endregion
    void Start()
    {
        TutorialNum = 0;
        SkipObject.SetActive(false);
        DataManager.Instance._CastleHP = 9;
        //��U�S�Ẵ{�^��������
        Shop.interactable = false;
        WeaponChange.interactable = false;
        NextStage.interactable = false;
        CastleRecovery.interactable = false;
        CastleAttackBuff.interactable = false;
        PlayerAttackBuff.interactable = false;
        ShortSowrd.interactable = false;
        buy.interactable = false;
        #region �{�^���t���O
        case5 = false;
        case8  = false;
        case10 = false;
        case15 = false;
        case16 = false;
        case18 = false;
        case20 = false;
        case22 = false;
        #endregion
    }


    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TutorialNum -= 1;
            nowDispCount = 0.0f;
        }*/
        switch (TutorialNum)
        {
            case 0:
                maxDispStr = "�����I���߂Ă̏o���A�ǂ��������H\n�@�̂��p�[�c�������ė�������...\n���������āA���_�̎{�݂��g���̂��ȁH";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 1;
                    nowDispCount = 0.0f;
                }
                break;
            case 1:
                maxDispStr = "��������A�{�݂̎g�����ɂ��Đ������Ă�����ˁB\n��ꂽ�炨�����񂪓{�邩��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 2;
                    nowDispCount = 0.0f;
                }
                break;
            case 2:
                maxDispStr = "���A�������ėv��Ȃ����ȁH\n�N���Ă��������̊���Ă�񂾂����H";
                SkipObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 3;
                    nowDispCount = 0.0f;
                    SkipObject.SetActive(false);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TutorialNum = 100;
                    nowDispCount = 0.0f;
                    SkipObject.SetActive(false);
                }
                break;
            case 3:
                maxDispStr = "�C���āI�������邩��I";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 4;
                    nowDispCount = 0.0f;
                }
                break;
            case 4:
                maxDispStr = "���̉�ʂł́A�@�̂⋒�_�̐����ƁA����̕ϑ����ł����B\n��ʍ��̃{�^���������邩�ȁH�ォ�珇�Ԃɐ�������ˁH";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 5;
                    nowDispCount = 0.0f;
                }
                break;
            case 5:
                Shop.interactable = true;//�V���b�v�{�^���̂݊�����
                maxDispStr = "��ԏ�̓V���b�v���ˁA�����ł��邩�́A���ɓ����Ċm�F���Ă݂悤�I";
                if (Input.GetKeyDown(KeyCode.Alpha1)||case5)
                {
                    TutorialNum = 6;
                    nowDispCount = 0.0f;
                }
                break;
            case 6:
                maxDispStr = "�����̓V���b�v�B�퓬�ŏW�߂��������g���Ă����Ȃ��Ƃ��o�����\n�����͐�قǂ̂悤�ɓG�@�����Ƃ�����A�撣���ďW�߂悤�I";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 7;
                    nowDispCount = 0.0f;
                }
                break;
            case 7:
                maxDispStr = "���āA�����炵�Ă݂悤��...�܂��͋��_�̏C�����ȁB\n���_���_���[�W�𕉂��ƁA�����ŏC�����邱�Ƃ��o�����B\n�X�e�[�W���I����Ă������I�ɉ񕜂��Ȃ����璍�ӂ��ĂˁB";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 8;
                    nowDispCount = 0.0f;
                }
                break;
            case 8:
                maxDispStr = "�}�E�X�ŋ��_�C���{�^�����������A�L�[�{�[�h��1�������ĂˁB";
                CastleRecovery.interactable = true;
                if (Input.GetKeyDown(KeyCode.Alpha1)||case8)
                {
                    TutorialNum = 9;
                    nowDispCount = 0.0f;
                }
                break;
            case 9:
                maxDispStr = "��ʂ̍��ɁA�C���Ɋւ���������o�Ă���̂��������ˁH\n���̉��ɂ���w���{�^���������ƁA�����������A���_���C��������I";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 10;
                    nowDispCount = 0.0f;
                }
                break;
            case 10:
                buy.interactable = true;
               maxDispStr = "���_�̏C�������Ă݂悤��I\n�}�E�X�Ń{�^�����������A������x1�������Ƃł����I";
                if (DataManager.Instance._CastleHP == 10)
                {
                    TutorialNum = 11;
                    nowDispCount = 0.0f;
                }
                break;
            case 11:
                buy.interactable = false;
                maxDispStr = "���_�͋߂Â��Ă����G�ɁA������x���Ȗh�q�G�ȍU�����o�����B\n�����ł́A���̍U���̍U���͂��������邱�Ƃ��ł����";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 12;
                    nowDispCount = 0.0f;
                }
                break;
            case 12:
                buy.interactable = true;
               maxDispStr = "���@�͏C���Ɠ�������B\n���A�{�^���������āA���ɏڍׂ��\�������B\n�����āA�w���{�^���������B�������̓L�[�{�[�h��2��2�񉟂��B\n����Ă݂悤��";
                CastleRecovery.interactable=false;
                CastleAttackBuff.interactable = true;
                if(DataManager.Instance._CastleAttackBuff==2)//DataManeger�ŊǗ�
                {
                    TutorialNum = 13;
                    nowDispCount = 0.0f;
                }
                break;
            case 13:
                buy.interactable = false;
                CastleAttackBuff.interactable = false;
                maxDispStr = "���񂤂�B�����ł����ˁB\n�ł��A���_�̖h��\�͂ɂ͌��E�����邩��ˁH\n���ꂶ��A�Ō��...�@�̂̋������ˁI";
                if(Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 14;
                    nowDispCount = 0.0f;
                }
                break;
            case 14:
                buy.interactable = true;
                PlayerAttackBuff.interactable = true;
                maxDispStr = "�@�̂���������ƁA�@�̂̍U���͂��������āA�G��|���₷���Ȃ��B����Ă݂悤���B\n�����{�^�����N���b�N���w���{�^�����N���b�N�B\n�������̓L�[�{�[�h��3���񉟂��B";
                if(DataManager.Instance._PlayerAttackBuff==2)//DataManeger�ŊǗ�
                {
                    TutorialNum = 15;
                    nowDispCount = 0.0f;
                }
                break;
            case 15:
                maxDispStr = "�@�̂������Ȃ����ˁI����ŁA�݂�Ȉ��S�ł��邩�ȁH\n�V���b�v�łł��鎖�̐����͂���ŏI��肾��B\n���ꂶ��A����ύX�����ɍs�����I\n���C���̋�����ʂɖ߂낤�I";
                buy.interactable = false;
                WeaponChange.interactable = true;
                if (Input.GetKeyDown(KeyCode.Backspace)||case15)
                {
                    TutorialNum = 16;
                    nowDispCount = 0.0f;
                }
                break;
            case 16:
                maxDispStr = "��Ԗڂ̃{�^���������āA����ύX������Ă݂悤�B";
                if(Input.GetKeyDown(KeyCode.Alpha2)|| case16)
                {
                    TutorialNum = 17;
                    nowDispCount = 0.0f;
                }
                break;
            case 17:
                maxDispStr = "�����ł��镐��́A�ߐځA�O��ނ����B\n���ꂼ��A����ɂ��������Ⴄ�����������ǂ�ł�";
                if(Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 18;
                    nowDispCount = 0.0f;
                }
                break;
            case 18:
                ShortSowrd.interactable = true;
                maxDispStr = "��̎O���ߋ�������B\n���ꂶ��A�ߋ��������ς��Ă݂悤���B\n�^�񒆂�<�Z��>���N���b�N..�������̓L�[�{�[�h��2�������Ă�";
                if(Input.GetKeyDown(KeyCode.Alpha2)||case18)
                {
                    TutorialNum = 19;
                    nowDispCount = 0.0f;
                }
                break;
            case 19:
                maxDispStr = "����͉���ł������ł��邩��A�����ɂ����������T���Ďg���ĂˁI\n����ƁA�}�E�X�ŃN���b�N���Ȃ��Ă�..\n�ߋ�������̓L�[�{�[�h��1,2,3�L�[�ŕς������I";
                if(Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 20;
                    nowDispCount = 0.0f;
                }
                break;
            case 20:
                maxDispStr = "���낻�뎟�̖ړI�n�Ɍ���������B���C���̋�����ʂɖ߂낤�I\n";
                if(case20)
                {
                    TutorialNum = 21;
                    nowDispCount = 0.0f;
                }
                break;
            case 21:
                NextStage.interactable = true;
                maxDispStr = "3�Ԃ�<�퓬��>�{�^���������ƁA���̃X�e�[�W�ɍs�����Ƃ��o�����B\n�������́A������ʂ̂ǂ�ȏꏊ�ł�<�G���^�[�L�[>���������ƂŁA���̃X�e�[�W�ɍs�����I";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 22;
                    nowDispCount = 0.0f;
                }
                break;
            case 22:
                maxDispStr = "����ŋ�����ʂ̐����͏I���I\n��́A�O�ɐi�ނ����B\n�s�s�݂̂�Ȃ��A�����A���݂�M���Ă����B\n�����A�s�����I";
                if(Input.GetKeyDown(KeyCode.Return) ||case22)
                {
                    FadeManager.Instance.LoadScene("Stage0", 1.5f);
                    nowDispCount = 0.0f;
                }
                break;
            case 100:
                maxDispStr = "�������A���ꂶ��A�厖�Ɏg���ĂˁI";
                DataManager.Instance._CastleHP = 10;
                DataManager.Instance._CastleAttackBuff = 1;
                DataManager.Instance._PlayerAttackBuff = 1;
                FadeManager.Instance.LoadScene("Stage0", 1.5f);
                break;
        }
        nowDispCount += Time.deltaTime / 0.05f;  //�����\�����x
        nowDispStr = maxDispStr.Substring(0, Mathf.Min((int)nowDispCount, maxDispStr.Length));
        text.text = nowDispStr;
    }
    public void Case5()
    {
        case5 = true;
    }
    public void Case8()
    {
        case8 = true;
    }
    public void Case10()
    {
        case10 = true;
    }
    public void Case15()
    {
        case15 = true;
    }
    public void Case16()
    {
        case16 = true;
    }
    public void Case18()
    {
        case18 = true;
    }
    public void Case20()
    {
        case20 = true;
    }
    public void Case22()
    {
        case22 = true;
    }
}


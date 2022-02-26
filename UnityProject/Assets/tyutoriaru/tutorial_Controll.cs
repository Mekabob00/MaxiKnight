using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorial_Controll : MonoBehaviour
{
    #region SeliarizFild
    [SerializeField, Tooltip("�e�L�X�g�E�C���h�E")]
    private GameObject TextWindow;
    [SerializeField, Tooltip("Enemy3")]
    private GameObject Enemy3;
    [SerializeField, Tooltip("���C��camera")]
    private GameObject MainCamera;
    [SerializeField, Tooltip("�T�u�J����")]
    private GameObject SubCamera;
    [SerializeField, Tooltip("��")]
    private GameObject castle;
    public Text text = null;
    public GameObject SkipSelect;
    public Rigidbody Enemy3Rigidbody;
    private int TutorialNum;
    private int TutorialCount;
    private bool EnemyDestroyFlag;
    private float time;
    private string maxDispStr = ""; //�\�������������e�̕�����
    private string nowDispStr = ""; //���ۂɉ�ʂɕ\��������p�̕�����
    [SerializeField, Tooltip("�����̃X�s�[�h")]
    private float nowDispCount = 0.0f; //���݉������ڂ܂ŕ\�����邩�̃J�E���^�[
    #endregion
    #region Unityfunction
    void Start()
    {
        Enemy3Rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
        time = 0;
        TutorialNum = 0;
        TutorialCount = 0;
        SkipSelect.SetActive(false);
        Enemy3.SetActive(false);
        TextWindow.SetActive(true);
        MainCamera.SetActive(true);
        SubCamera.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TutorialNum -= 1;
            nowDispCount = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TutorialNum = 100;
            nowDispCount = 0.0f;
        }
        switch (TutorialNum)
        {
            case 0:
                maxDispStr = "�����������Ƃ���ł̎��H�͏��߂ĂɂȂ�܂��B\n����āA��b�@�\�i�r�Q�[�V�����A�ʏ̃`���[�g���A���𐄏����܂��B\n�K�v�Ȃ��Ȃ�A�`���[�g���A�����X�L�b�v���܂����A�������������܂����H";
                SkipSelect.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TutorialNum = 100;
                    nowDispCount = 0.0f;
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 1;
                    nowDispCount = 0.0f;
                }
                break;
            case 1:
                maxDispStr = "�����B����ł́A�������J�n���܂��B";
                SkipSelect.SetActive(false);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 2;
                    nowDispCount = 0.0f;
                }
                break;
            case 2:
                maxDispStr = "���L�[�̍��E�ŁA���E�̈ړ����\�ł��B\n�E�ɂ��A���ɂ��ړ��ł��鋗���̌��E������܂��̂ŁA�����ӂ��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 3;
                    nowDispCount = 0.0f;
                }
                break;
            case 3:
                maxDispStr = "����ł́A���ۂɍ��E�̈ړ������Ă݂܂��傤�B";
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {

                    TutorialNum = 4;
                    nowDispCount = 0.0f;
                }
                break;
            case 4:
                maxDispStr = "�������ł��A���E�̈ړ��Ɋւ��Ă͖��Ȃ��ł��傤�B\n���͉���̐������s���܂��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 5;
                    nowDispCount = 0.0f;
                }
                break;
            case 5:
                maxDispStr = "C�L�[�������ƁA�����Ă�������ɉ�����邱�Ƃ��ł��܂��B\n��𒆂͏����̊Ԗ��G�ƂȂ�A\n�U�����ɂ��g�p���邱�Ƃ��o���܂��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 6;
                    nowDispCount = 0.0f;
                }
                break;
            case 6:
                maxDispStr = "����ł́A������Ă݂܂��傤�B";
                if (Input.GetKeyDown(KeyCode.C))
                {
                    TutorialNum = 7;
                    nowDispCount = 0.0f;
                }
                break;
            case 7:
                maxDispStr = "�������ł�\n����ōU���̃��[�V�������L�����Z������e�N�j�b�N����g�����\n���~���Ȑ퓬���\�ɂȂ�܂��B\n���́A���[���ړ��ɂ��Đ����������܂��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 8;
                    nowDispCount = 0.0f;
                }
                break;
            case 8:
                maxDispStr = "�X�e�[�W�̒��ɂ͏㉺�œG������Ă��铹��������Ă��āA\n���̓������[���Ə̂��܂��B\n���Ȃ��͓�̃��[�����s�������Ȃ���A�G��키�K�v������܂��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 9;
                    nowDispCount = 0.0f;
                }
                break;
            case 9:
                maxDispStr = "���[���̈ړ��́A����Ƃ��Ȃ��AC�L�[���g���܂��A\n���̎��A�㉺�̖��L�[�������Ă���K�v������܂��B\n���ۂɂ���Ă݂܂��傤�B";
                if (Input.GetKey(KeyCode.C))
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        TutorialNum = 10;
                        nowDispCount = 0.0f;
                    }
                }
                break;
            case 10:
                maxDispStr = "���[���̈ړ��ŁA�G�̐i�s�ɍ��킹�Ȃ���퓬���s���Ă�������";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 11;
                    nowDispCount = 0.0f;
                }
                break;
            case 11:
                maxDispStr = "���̋@�̂͋ߐڕ����𑕔����邱�Ƃ��ł��܂��B\n���ݑ������Ă��镐��͋ߐڕ����̓��ł�";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 12;
                    nowDispCount = 0.0f;
                }
                break;
            case 12:
                maxDispStr = "�ߐڍU���́A�߂���ɂ���G���U�����邱�Ƃ��\�A\n3��܂ŘA���ōU���o���܂��B�ߐڍU����Z�L�[�������Ă�������";
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    TutorialNum = 15;
                    nowDispCount = 0.0f;
                }
                break;
            case 13:
                maxDispStr = "���̓��C�t�����g�����������U�����s���܂��B\n�������U���͏��������܂ł̓G�ɍU�����邱�Ƃ��ł��܂����A\n��x�g���ƁA�ė��p�܂őҋ@���Ԃ�v���܂��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 14;
                    nowDispCount = 0.0f;
                }
                break;
            case 14:
                maxDispStr = "X�L�[�ŉ������U�����g���Ă݂܂��傤";
                if (Input.GetKeyDown(KeyCode.X))
                {
                    TutorialNum = 15;
                    nowDispCount = 0.0f;
                }
                break;
            case 15:
                maxDispStr = "��ʂœG�������񂹂Ă���ꍇ�A\n�������U���ŋߋ����U�����g��������K�v������܂��B";//�ύX�K�{
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 16;
                    nowDispCount = 0.0f;
                }
                break;
            case 16:
                maxDispStr = "�G�����_�܂ŋߐڂ���ƁA���_����Q��ǂ��Ă��܂��܂��B\n���_�̑ϋv�l��Tab�L�[�œW�J�ł���C���^�[�t�F�[�X�Ŋm�F�ł��܂��B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 17;
                    nowDispCount = 0.0f;
                }
                break;
            case 17:
                maxDispStr = "Tab�L�[�ŃC���^�[�t�F�[�X��W�J���āA���_�̑ϋv���m�F���Ă݂܂��傤�B";
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    TutorialNum = 18;
                    nowDispCount = 0.0f;
                }
                break;
            case 18:
                maxDispStr = "�C���^�[�t�F�X����A���_�̏󋵂�c�����Ă��������B";
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    TutorialNum = 19;
                    nowDispCount = 0.0f;
                }
                break;
            case 19:
                SubCamera.SetActive(true);
                MainCamera.SetActive(false);
                Enemy3.SetActive(true);
                maxDispStr = "...�G�̋ߐڂ��m�F�A���悢��ߐڂ̎��Ԃł��B���H�̗p�ӂ͑��v�ł���?";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 20;
                    nowDispCount = 0.0f;
                }
                break;
            case 20:
                maxDispStr = "�U�����s���A�G�@��j�󂵂Ă��������B���@�͖₢�܂���B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    MainCamera.SetActive(true);
                    SubCamera.SetActive(false);
                    TextWindow.SetActive(false);
                    Enemy3Rigidbody.constraints = RigidbodyConstraints.None;
                    Enemy3Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    nowDispCount = 0.0f;
                }
                if (!Enemy3)
                {
                    if (castle.GetComponent<CastleBehavior>()._Health == 9)
                    {
                        TutorialNum = 22;
                    }
                    else
                    {
                        TutorialNum = 21;
                    }
                }
                break;
            case 21:
                TextWindow.SetActive(true);
                maxDispStr = "�G�@�̔j������F���܂����B���̒�@�@�������悤�ł��B\n�{���Ƃ̐퓬�͂���������ɂȂ肻���ł����A���������Ă����܂��傤�B";
                if (Input.GetKeyDown(KeyCode.A))
                {
                    TutorialNum = 23;
                    nowDispCount = 0.0f;
                }
                break;
            case 22:
                TextWindow.SetActive(true);
                maxDispStr = "...�G�����_�ɓ��B���A���_���_���[�W�𕉂��Ă��܂��܂����B\n��Q�͌y���ł����A������͋C�����Ă��������B\n�{���Ƃ̐퓬�͂���������ɂȂ肻���ł����A���������Ă����܂��傤�B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 23;
                    nowDispCount = 0.0f;
                }
                break;
            case 23:
                maxDispStr = "��قǓ|�����G����A�Ċ��p�ł���p�[�c����肵�܂����B\n��x���_�ɖ߂�A������ŋ@�̂̐������s���܂��傤�B";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    DataManager.Instance._Resource = 300;
                    FadeManager.Instance.LoadScene("ShopTutorial", 1.0f);
                    TutorialNum = 1000;
                }
                break;
            case 100:
                maxDispStr = "�����B����ł́A�������F��܂��B";
                DataManager.Instance._Resource = 300;
                time += Time.deltaTime;
                if (time >= 2.0f)
                {
                    FadeManager.Instance.LoadScene("ShopTutorial", 1.5f);
                    time = 0.0f;
                    TutorialNum = 1000;
                }
                break;
            case 1000:

                break;
        }
        nowDispCount += Time.deltaTime / 0.05f;  //�����\�����x


        nowDispStr = maxDispStr.Substring(0, Mathf.Min((int)nowDispCount, maxDispStr.Length));
        text.text = nowDispStr;
    }
    #endregion
}

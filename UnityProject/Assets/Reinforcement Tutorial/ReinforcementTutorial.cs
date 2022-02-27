using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReinforcementTutorial : MonoBehaviour
{
    [SerializeField, Tooltip("テキストウインドウ")]
    private GameObject TextWindow;
    [SerializeField, Tooltip("文字のスピード")]
    private float nowDispCount = 0.0f; //現在何文字目まで表示するかのカウンター
    [SerializeField, Tooltip("スキップの選択オブジェクト")]
    private GameObject SkipObject;
    [SerializeField, Tooltip("Shopボタン")]
    private Button Shop;
    [SerializeField, Tooltip("武器変更ボタン")]
    private Button WeaponChange;
    [SerializeField, Tooltip("戦闘へボタン")]
    private Button NextStage;
    [SerializeField, Tooltip("城の回復ボタン")]
    private Button CastleRecovery;
    [SerializeField, Tooltip("城の攻撃力アップボタン")]
    private Button CastleAttackBuff;
    [SerializeField, Tooltip("Playerの攻撃力アップボタン")]
    private Button PlayerAttackBuff;
    [SerializeField, Tooltip("短剣ボタン")]
    private Button ShortSowrd;
    [SerializeField, Tooltip("買う")]
    private Button buy;
    [SerializeField, Tooltip("TextSound")]
    private AudioClip TextSound;
    [SerializeField, Tooltip("TutoSuccess")]
    private AudioClip TutoSuccess;
    public Text text = null;
    public AudioSource audioSource = null;
    private int TutorialNum;
    private string maxDispStr = ""; //表示させたい内容の文字列
    private string nowDispStr = ""; //実際に画面に表示させる用の文字列
    public GameObject TutoPannel;

    #region ボタンフラグ変数宣言
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
        SkipObject.SetActive(true);
        DataManager.Instance._CastleHP = 9;
        //一旦全てのボタン無効化
        Shop.interactable = false;
        WeaponChange.interactable = false;
        NextStage.interactable = false;
        CastleRecovery.interactable = false;
        CastleAttackBuff.interactable = false;
        PlayerAttackBuff.interactable = false;
        ShortSowrd.interactable = false;
        buy.interactable = false;
        #region ボタンフラグ
        case5 = false;
        case8 = false;
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
                maxDispStr = "お疲れ！初めての出撃、どうだった？\n機体がパーツを持って来たけど...\nもしかして、拠点の施設を使うのかな？";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 1;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 1:
                maxDispStr = "だったら、施設の使い方について説明してあげるね。\n壊れたらお父さんが怒るから。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 2;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 2:
                maxDispStr = "あ、説明って要らないかな？\n君ってこういうの慣れてるんだっけ？\n（Zキーで次に進み、\nスペースキーでチュートリアルをスキップできます）";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 3;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TutorialNum = 100;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 3:
                maxDispStr = "任せて！説明するから！";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 4;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 4:
                maxDispStr = "この画面では、機体や拠点の整備と、武器の変装ができるよ。\n画面左のボタンが見えるかな？上から順番に説明するね？";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 5;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 5:
                Shop.interactable = true;//ショップボタンのみ活性化
                maxDispStr = "一番上はショップだね、何ができるかは、中に入って確認してみよう！";
                if (Input.GetKeyDown(KeyCode.Alpha1) || case5)
                {
                    TutorialNum = 6;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 6:
                TutoPannel.transform.localPosition = new Vector3(25f, 250f, 0f);

                maxDispStr = "ここはショップ。戦闘で集めた資源を使っていろんなことが出来るよ\n資源は先ほどのように敵機が落とすから、頑張って集めよう！";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 7;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 7:
                maxDispStr = "さて、何からしてみようか...まずは拠点の修理かな。\n拠点がダメージを負うと、ここで修理することが出来るよ。\nステージが終わっても自動的に回復しないから注意してね。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 8;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 8:
                maxDispStr = "マウスで拠点修理ボタンを押してね。";
                CastleRecovery.interactable = true;
                if (Input.GetKeyDown(KeyCode.Alpha1) || case8)
                {
                    TutorialNum = 9;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 9:
                maxDispStr = "画面の左に、修理に関する説明が出ているのが見えるよね？\nその下にある購入ボタンを押すと、資源が消費され、拠点が修理されるよ！";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 10;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 10:
                buy.interactable = true;
                maxDispStr = "拠点の修理をしてみようよ！\nマウスで購入ボタンを押してみよう！";
                if (DataManager.Instance._CastleHP == 10)
                {
                    TutorialNum = 11;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 11:
                buy.interactable = false;
                maxDispStr = "拠点は近づいてきた敵に、ある程度自己防衛敵な攻撃が出来るよ。\nここでは、その攻撃の攻撃力を強化することができるの";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 12;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 12:
                buy.interactable = true;
                maxDispStr = "方法は修理と同じだよ。\n一回、ボタンを押して、左に詳細が表示される。\nそして、購入ボタンを押す。\nやってみようか";
                CastleRecovery.interactable = false;
                CastleAttackBuff.interactable = true;
                if (DataManager.Instance._CastleAttackBuff == 2)//DataManegerで管理
                {
                    TutorialNum = 13;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 13:
                buy.interactable = false;
                CastleAttackBuff.interactable = false;
                maxDispStr = "うんうん。強化できたね。\nでも、拠点の防御能力には限界があるからね？\nそれじゃ、最後に...機体の強化だね！";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 14;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 14:
                buy.interactable = true;
                PlayerAttackBuff.interactable = true;
                maxDispStr = "機体を強化すると、機体の攻撃力があがって、敵を倒しやすくなるよ。やってみようか。\n強化ボタンをクリック→購入ボタンをクリック。";
                if (DataManager.Instance._PlayerAttackBuff == 2)//DataManegerで管理
                {
                    TutorialNum = 15;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 15:
                maxDispStr = "機体が強くなったね！これで、みんな安心できるかな？\nショップでできる事の説明はこれで終わりだよ。\nそれじゃ、武器変更をしに行こう！\nメインの強化画面に戻ろう！";
                buy.interactable = false;
                WeaponChange.interactable = true;
                if (Input.GetKeyDown(KeyCode.Backspace) || case15)
                {
                    TutorialNum = 16;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 16:
                TutoPannel.transform.localPosition = new Vector3(25f, -267f, 0f);
                maxDispStr = "二番目のボタンを押して、武器変更をやってみよう。";
                if (Input.GetKeyDown(KeyCode.Alpha2) || case16)
                {
                    TutorialNum = 17;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 17:
                TutoPannel.transform.localPosition = new Vector3(25f, 250f, 0f);
                maxDispStr = "装備できる武器は、三種類あるよ。\nそれぞれ、武器による特性が違うから説明文を読んでね";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 18;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 18:
                ShortSowrd.interactable = true;
                maxDispStr = "それじゃ、近距離武器を変えてみようか。\n真ん中の<短検>をクリックしてね";
                if (Input.GetKeyDown(KeyCode.Alpha2) || case18)
                {
                    TutorialNum = 19;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 19:
                maxDispStr = "武器は何回でも交換できるから、自分にあった武器を探して使ってね！";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 20;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 20:
                maxDispStr = "そろそろ次の目的地に向かおうよ。メインの強化画面に戻ろう！";
                if (case20)
                {
                    TutorialNum = 21;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 21:
                NextStage.interactable = true;
                TutoPannel.transform.localPosition = new Vector3(25f, -267f, 0f);

                maxDispStr = "3番の<戦闘へ>ボタンを押すと、次のステージに行くことが出来るよ。\nもしくは、強化画面のどんな場所でも<エンターキー>を押すことで、次のステージに行けるよ！";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    SkipObject.SetActive(false);
                    TutorialNum = 22;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 22:
                maxDispStr = "これで強化画面の説明は終わり！\n後は、前に進むだけ。\n都市のみんなも、私も、きみを信じているよ。\nさあ、行こう！";
                if (Input.GetKeyDown(KeyCode.Return) || case22)
                {
                    FadeManager.Instance.LoadScene("Stage0", 1.5f);
                    nowDispCount = 0.0f;
                }
                break;
            case 100:
                maxDispStr = "そっか、それじゃ、大事に使ってね！";
                DataManager.Instance._CastleHP = 10;
                DataManager.Instance._CastleAttackBuff = 2;
                DataManager.Instance._PlayerAttackBuff = 2;
                FadeManager.Instance.LoadScene("Stage0", 1.5f);
                break;
        }
        nowDispCount += Time.deltaTime / 0.05f;  //文字表示速度
        nowDispStr = maxDispStr.Substring(0, Mathf.Min((int)nowDispCount, maxDispStr.Length));
        text.text = nowDispStr;
    }
    private void SEPlay(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("オーディオソースが設定されてない");
        }
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

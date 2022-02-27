using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorial_Controll : MonoBehaviour
{
    #region SeliarizFild
    [SerializeField, Tooltip("テキストウインドウ")]
    private GameObject TextWindow;
    [SerializeField, Tooltip("Enemy3")]
    private GameObject Enemy3;
    [SerializeField, Tooltip("メインcamera")]
    private GameObject MainCamera;
    [SerializeField, Tooltip("サブカメラ")]
    private GameObject SubCamera;
    [SerializeField, Tooltip("城")]
    private GameObject castle;
    [SerializeField, Tooltip("TextSound")]
    private AudioClip TextSound;
    [SerializeField, Tooltip("TutoSuccess")]
    private AudioClip TutoSuccess;
    public Text text = null;
    public GameObject SkipSelect;
    public Rigidbody Enemy3Rigidbody;
    private int TutorialNum;
    private int TutorialCount;
    private int AttakConut;
    private bool EnemyDestroyFlag;
    private float time;
    private string maxDispStr = ""; //表示させたい内容の文字列
    private string nowDispStr = ""; //実際に画面に表示させる用の文字列
    [SerializeField, Tooltip("文字のスピード")]
    private float nowDispCount = 0.0f; //現在何文字目まで表示するかのカウンター

    public AudioSource audioSource = null;
    #endregion
    #region Unityfunction
    void Start()
    {
        AttakConut = 0;
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
                maxDispStr = "道幅が狭いところでの実践は初めてになります。\nよって、基礎機能ナビゲーション、通称チュートリアルを推奨します。\n必要ないなら、チュートリアルをスキップしますか、いかがいたしますか？";
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
                    SEPlay(TextSound);
                }
                break;
            case 1:
                maxDispStr = "了解。それでは、説明を開始します。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 2;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 2:
                maxDispStr = "矢印キーの左右で、左右の移動が可能です。\n右にも、左にも移動できる距離の限界がありますので、ご注意を。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 3;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 3:
                maxDispStr = "それでは、実際に左右の移動をしてみましょう。\n□矢印キーを使って左右の移動をしてみる。";
                SkipSelect.SetActive(false);
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    TutorialNum = 4;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 4:
                maxDispStr = "さすがです、左右の移動に関しては問題ないでしょう。\n次は回避の説明を行います。";
                TextWindow.SetActive(true);
                SkipSelect.SetActive(true);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 5;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 5:
                maxDispStr = "Cキーを押すと、向いている方向に回避することができます。\n回避中は少しの間無敵となり、\n攻撃中にも使用することが出来ます。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 6;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 6:
                maxDispStr = "それでは、回避してみましょう。\n□Cキーを使って回避をしてみる。";
                SkipSelect.SetActive(false);
                if (Input.GetKeyDown(KeyCode.C))
                {
                    TutorialNum = 7;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 7:
                maxDispStr = "さすがです\n回避で攻撃のモーションをキャンセルするテクニックを駆使すると\nより円滑な戦闘が可能になります。\n次は、レーン移動について説明いたします。";
                TextWindow.SetActive(true);
                SkipSelect.SetActive(true);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 8;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 8:
                maxDispStr = "ステージの中には上下で敵がやってくる道が分かれていて、\nこの道をレーンと称します。\nあなたは二つのレーンを行き来しながら、敵を戦う必要があります。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 9;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 9:
                maxDispStr = "レーンは上下の矢印キーを使うことで移動できます。\n実際にやってみましょう\n□上下キーを使ってレーンを変更してみる";
                SkipSelect.SetActive(false);
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        TutorialNum = 10;
                        nowDispCount = 0.0f;
                        SEPlay(TutoSuccess);
                    }
                break;
            case 10:
                maxDispStr = "レーンの移動で、敵の進行に合わせながら戦闘を行ってください。\n次は、攻撃行動に関するチュートリアルを行います。";
                TextWindow.SetActive(true);
                SkipSelect.SetActive(true);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 11;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 11:
                maxDispStr = "この機体は近接武装を装備することができます。\n現在装備している武器は近接武装の刀です";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 12;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 12:
                maxDispStr = "近接攻撃は、近い一にある敵を攻撃することが可能、\n3回まで連続で攻撃出来ます。近接攻撃はZキーを押してください。\n□Zキーで3連攻撃をしてみる";
                SkipSelect.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    AttakConut++;
                    if (AttakConut >= 3)
                    {
                        TutorialNum = 15;
                        nowDispCount = 0.0f;
                        SEPlay(TutoSuccess);
                    }
                }
                break;
            case 13:
                SkipSelect.SetActive(true);
                maxDispStr = "次はライフルを使った遠距離攻撃を行います。\n遠距離攻撃は少し遠くまでの敵に攻撃することができますが、\n一度使うと、再利用まで待機時間を要します。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 14;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 14:
                maxDispStr = "Xキーで遠距離攻撃を使ってみましょう";
                if (Input.GetKeyDown(KeyCode.X))
                {
                    TutorialNum = 15;
                    nowDispCount = 0.0f;
                }
                break;
            case 15:
                maxDispStr = "大量で敵が押し寄せてくる場合、\n慌てず、レーン移動と近距離攻撃で対応しましょう。";//変更必須
                TextWindow.SetActive(true);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 16;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 16:
                maxDispStr = "敵が拠点まで近接すると、拠点が被害を追ってしまいます。\n拠点の耐久値はTabキーで展開できるインターフェースで確認できます。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 17;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 17:
                maxDispStr = "Tabキーでインターフェースを展開して、拠点の耐久を確認しましょう。\n□Tabキーでインターフェースを展開してみる";
                SkipSelect.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    TutorialNum = 18;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 18:
                maxDispStr = "インターフェスを閉じ、拠点の状況を把握してください。\n□Tabキーでインターフェースを閉じる。";
               
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    TutorialNum = 19;
                    nowDispCount = 0.0f;
                    SEPlay(TutoSuccess);
                }
                break;
            case 19:
                SkipSelect.SetActive(true);
                SubCamera.SetActive(true);
                MainCamera.SetActive(false);
                Enemy3.SetActive(true);
                maxDispStr = "...敵の近接を確認、いよいよ近接の時間です。実践の用意は大丈夫ですか?";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 20;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 20:
                maxDispStr = "攻撃を行い、敵機を破壊してください。方法は問いません。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    MainCamera.SetActive(true);
                    SubCamera.SetActive(false);
                    TextWindow.SetActive(false);
                    Enemy3Rigidbody.constraints = RigidbodyConstraints.None;
                    Enemy3Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
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
                maxDispStr = "敵機の破壊を視認しました。今の偵察機だったようです。\n本隊との戦闘はもう少し先になりそうですが、整備をしておきましょう。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 23;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 22:
                TextWindow.SetActive(true);
                maxDispStr = "...敵が拠点に到達し、拠点がダメージを負ってしまいました。\n被害は軽微ですが、次からは気をつけてください。\n本隊との戦闘はもう少し先になりそうですが、整備をしておきましょう。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    TutorialNum = 23;
                    nowDispCount = 0.0f;
                    SEPlay(TextSound);
                }
                break;
            case 23:
                maxDispStr = "先ほど倒した敵から、再活用できるパーツを入手しました。\n一度拠点に戻り、そちらで機体の整備を行いましょう。";
                if (Input.GetKeyDown(KeyCode.V))
                {
                    SkipSelect.SetActive(false);
                    SEPlay(TextSound);
                    DataManager.Instance._Resource = 300;
                    FadeManager.Instance.LoadScene("ShopTutorial", 1.0f);
                    TutorialNum = 1000;
                }
                break;
            case 100:
                maxDispStr = "了解。それでは、健闘を祈ります。";
                DataManager.Instance._Resource = 300;
                time += Time.deltaTime;
                if (time >= 2.0f)
                {
                    SEPlay(TextSound);
                    FadeManager.Instance.LoadScene("ShopTutorial", 1.5f);
                    time = 0.0f;
                    TutorialNum = 1000;
                }
                break;
            case 1000:

                break;
        }
        nowDispCount += Time.deltaTime / 0.05f;  //文字表示速度


        nowDispStr = maxDispStr.Substring(0, Mathf.Min((int)nowDispCount, maxDispStr.Length));
        text.text = nowDispStr;
    }
    #endregion
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage12ClearText : MonoBehaviour
{
    private string maxDispStr = ""; //表示させたい内容の文字列
    private string nowDispStr = ""; //実際に画面に表示させる用の文字列
    [SerializeField, Tooltip("文字のスピード")]
    private float nowDispCount = 0.0f; //現在何文字目まで表示するかのカウンター
    public Text text = null;
    public GameObject MainBGM;
    public GameObject ClearBGM;
    // Start is called before the first frame update
    void Start()
    {
        MainBGM.GetComponent<AudioSource>().enabled = false;
        ClearBGM.GetComponent<AudioSource>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        maxDispStr = "敵の殲滅を確認しました。\nお疲れ様です。";
        nowDispCount += Time.deltaTime / 0.05f;  //文字表示速度
        nowDispStr = maxDispStr.Substring(0, Mathf.Min((int)nowDispCount, maxDispStr.Length));
        text.text = nowDispStr;
        if (Input.GetKeyDown(KeyCode.V))
        {
            FadeManager.Instance.LoadScene("shop", 1.0f);
        }
    }
}

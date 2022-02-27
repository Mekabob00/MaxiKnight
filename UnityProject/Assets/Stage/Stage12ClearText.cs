using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage12ClearText : MonoBehaviour
{
    private string maxDispStr = ""; //�\�������������e�̕�����
    private string nowDispStr = ""; //���ۂɉ�ʂɕ\��������p�̕�����
    [SerializeField, Tooltip("�����̃X�s�[�h")]
    private float nowDispCount = 0.0f; //���݉������ڂ܂ŕ\�����邩�̃J�E���^�[
    public Text text = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        maxDispStr = "�N���A������";
        nowDispCount += Time.deltaTime / 0.05f;  //�����\�����x
        nowDispStr = maxDispStr.Substring(0, Mathf.Min((int)nowDispCount, maxDispStr.Length));
        text.text = nowDispStr;
        if (Input.GetKeyDown(KeyCode.V))
        {
            FadeManager.Instance.LoadScene("shop", 1.0f);
        }
    }
}

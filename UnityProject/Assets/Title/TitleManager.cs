using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject _Cursor;
    public GameObject _StartText;
    public GameObject _ContinueText;
    public GameObject _ZBottonText;
    public GameObject _LoadPanel;
    public GameObject _LoadCursor;

    enum CURSOR { NON, START, CONTINUE, LOAD };
    CURSOR cursor;

    // Start is called before the first frame update
    void Start()
    {
        cursor = CURSOR.NON;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchState();
    }

    void SwitchState()
    {
        switch (cursor)
        {
            case CURSOR.NON:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    cursor = CURSOR.START;
                    _Cursor.SetActive(true);
                    _StartText.SetActive(true);
                    _ContinueText.SetActive(true);
                    _ZBottonText.SetActive(false);
                }
                break;
            case CURSOR.START:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    cursor = CURSOR.CONTINUE;
                    _Cursor.transform.localPosition = new Vector3(-350, -475, 0);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Debug.Log("チュートリアルシーン");
                }
                break;
            case CURSOR.CONTINUE:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    cursor = CURSOR.START;
                    _Cursor.transform.localPosition = new Vector3(-350, -230, 0);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    cursor = CURSOR.LOAD;
                    _LoadPanel.SetActive(true);
                    _Cursor.SetActive(false);
                    _StartText.SetActive(false);
                    _ContinueText.SetActive(false);
                }
                break;
            case CURSOR.LOAD:
                if (Input.GetKeyDown(KeyCode.UpArrow) && _LoadCursor.transform.localPosition.y < 500)
                    _LoadCursor.transform.Translate(0, 300, 0);

                if (Input.GetKeyDown(KeyCode.DownArrow) && _LoadCursor.transform.localPosition.y > -300)
                    _LoadCursor.transform.Translate(0, -300, 0);

                if (Input.GetKeyDown(KeyCode.X))
                {
                    Debug.Log("Exit Load Data");
                    cursor = CURSOR.START;
                    _LoadPanel.SetActive(false);
                    _Cursor.SetActive(true);
                    _StartText.SetActive(true);
                    _ContinueText.SetActive(true);
                }
                break;
        }
    }
}

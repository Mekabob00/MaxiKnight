using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject _Cursor;
    public float _WaitTime;

    Animator m_anim;
    enum CURSOR { CONTINUE, GIVEUP };
    CURSOR m_cursor;
    float m_timer;
  
    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_cursor = CURSOR.CONTINUE;
    }

    void Update()
    {
        if (GlobalData.Instance.isGameOver)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= _WaitTime)
            {
                m_anim.SetTrigger("Open");
                SwitchSelect();
            }
        }
    }

    void SwitchSelect()
    {
        switch (m_cursor)
        {
            case CURSOR.CONTINUE:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m_cursor = CURSOR.GIVEUP;
                    _Cursor.transform.position += new Vector3(1560, 0, 0);
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    SwitchScene.ChangeSceneToShop();
                }
                break;
            case CURSOR.GIVEUP:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m_cursor = CURSOR.CONTINUE;
                    _Cursor.transform.position += new Vector3(-1560, 0, 0);
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    SwitchScene.ChangeSceneToTitle();
                }
                break;
        }
    }
}
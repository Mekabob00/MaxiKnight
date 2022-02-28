using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject _Cursor;
    [Header("BGM")]public AudioSource _StageBGM;

    public float _WaitTime;

    Animator m_animtor;
    AudioSource m_audioSource;
    enum CURSOR { CONTINUE, GIVEUP };
    CURSOR m_cursor;
    float m_timer;
  
    void Start()
    {
        _StageBGM = GameObject.Find("Main Camera (1)").GetComponent<AudioSource>();
        m_animtor = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_cursor = CURSOR.CONTINUE;
    }

    void Update()
    {
        if (GlobalData.Instance.isGameOver)
        {
            _StageBGM.Stop();
            m_timer += Time.deltaTime;
            if (m_timer >= _WaitTime)
            {
                if(!m_audioSource.isPlaying)
                    m_audioSource.Play();
                m_animtor.SetTrigger("Open");
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
                    _Cursor.transform.localPosition = new Vector3(100, -45, 0);
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    DataManager.Instance._CastleHP = DataManager.Instance._CastleMaxHP;
                    FadeManager.Instance.LoadScene("Shop", 1.0f);
                }
                break;
            case CURSOR.GIVEUP:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m_cursor = CURSOR.CONTINUE;
                    _Cursor.transform.localPosition = new Vector3(-620, -45, 0);
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    FadeManager.Instance.LoadScene("Title(scene)", 1.0f);
                }
                break;
        }
    }
}

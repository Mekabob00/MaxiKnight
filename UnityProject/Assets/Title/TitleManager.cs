using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SceneChange();
    }
    private void SceneChange()
    {
        if(Input.anyKeyDown && !Input.GetMouseButtonDown(0)&&!Input.GetMouseButtonDown(1)&&!Input.GetMouseButtonDown(2))
        {
            DataManager.Instance.Reset();
            GlobalData.Instance.ResetData();
            FadeManager.Instance.LoadScene("tutorial", 0.5f);
            GetComponent<AudioSource>().Play();
        }
    }
}

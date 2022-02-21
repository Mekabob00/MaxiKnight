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
        //SceneChange();
        if(Input.GetKeyDown(KeyCode.Z))
        {
            FadeManager.Instance.LoadScene("Stage0", 1.0f);
            GetComponent<AudioSource>().Play();
        }
    }
    private void SceneChange()
    {
        if(Input.anyKeyDown && !Input.GetMouseButtonDown(0)&&!Input.GetMouseButtonDown(1)&&!Input.GetMouseButtonDown(2))
        {
            FadeManager.Instance.LoadScene("Stage0",0.5f);
            GetComponent<AudioSource>().Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public AudioClip startSE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //SceneChange();
        if(Input.anyKeyDown)
        {
            FadeManager.Instance.LoadScene("tutorial", 1.5f);
            GetComponent<AudioSource>().PlayOneShot(startSE);
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

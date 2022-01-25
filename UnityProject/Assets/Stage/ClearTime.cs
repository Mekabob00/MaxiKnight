using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            SceneManager.LoadScene("Clear0");
        }

        if (Input.GetKey(KeyCode.F1))
        {
            SceneManager.LoadScene("Stage0");
        }
    }
}

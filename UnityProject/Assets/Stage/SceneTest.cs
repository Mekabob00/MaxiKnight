using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SceneManager.LoadScene("Shop");
        }

        if (Input.GetKey(KeyCode.F1))
        {
            SceneManager.LoadScene("Stage0");
        }

    }
}

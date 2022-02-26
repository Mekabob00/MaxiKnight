using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial_Maneger : MonoBehaviour
{
    public static tutorial_Maneger instance = null;

    public int tutorialNum;
    public int tutorialCount;
    // Start is called before the first frame update
    void Start()
    {
        tutorialNum = 0;
        tutorialCount = 0;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShousaiImage : MonoBehaviour
{
    public Sprite Repair;
    public Sprite Castle;
    public Sprite Robot;
    public int Status;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (Status)
        {
            case 0:
                this.GetComponent<Image>().enabled=false;
                break;
            case 1:
                this.GetComponent<Image>().enabled = true;
                this.GetComponent<Image>().sprite = Repair;
                break;
            case 2:
                this.GetComponent<Image>().enabled = true;
                this.GetComponent<Image>().sprite = Castle;
                break;
            case 3:
                this.GetComponent<Image>().enabled = true;
                this.GetComponent<Image>().sprite = Robot;
                break;
        }
    }
}

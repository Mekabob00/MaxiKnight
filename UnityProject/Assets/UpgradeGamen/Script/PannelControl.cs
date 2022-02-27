using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelControl : MonoBehaviour
{
    public GameObject Pannel;
    public int status;
    public int ThisPannel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopClick()
    {
        if (status == 0)
        {
            Pannel.GetComponent<Animator>().SetInteger("Status", 1);
            status = 1;
        }
    }

    public void Exit()
    {
        Pannel.GetComponent<Animator>().SetInteger("Status", 2);
    }

    public void BackClick()
    {
        if (status == 1)
        {
            Pannel.GetComponent<Animator>().SetInteger("Status", 3);
            status = 0;

        }
    }

    public void Enter()
    {
        Pannel.GetComponent<Animator>().SetInteger("Status", 0);
    }
}

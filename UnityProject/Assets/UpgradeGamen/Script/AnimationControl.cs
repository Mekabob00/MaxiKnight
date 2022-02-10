using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationControl : MonoBehaviour
{
    public GameObject CharaPannel;
    public GameObject ShopButton;

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
        CharaPannel.GetComponent<Animator>().SetInteger("Status", 2);
    }

    public void CharaOut()
    {
        CharaPannel.GetComponent<Animator>().SetInteger("Status", 3);
    }
}

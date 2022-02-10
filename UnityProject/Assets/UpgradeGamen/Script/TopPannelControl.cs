using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TopPannelControl : MonoBehaviour
{
    public GameObject TopPannel;
    public Image img;
    public Sprite NormalTop;
    public Sprite ShopTop;
    public Sprite WeaponTop;
    public int Spritenum;
    public int Selectnum;

    // Start is called before the first frame update
    void Start()
    {
        TopPannel = GameObject.Find("TopPannel");
        img = TopPannel.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Onclick()
    {
        TopPannel.GetComponent<TopPannelControl>().Spritenum = Selectnum;
        switch (TopPannel.GetComponent<TopPannelControl>().Spritenum)
        {
            case 1:
                img.sprite = TopPannel.GetComponent<TopPannelControl>().NormalTop;
                break;
            case 2:
                img.sprite = TopPannel.GetComponent<TopPannelControl>().ShopTop;
                break;
            case 3:
                img.sprite = TopPannel.GetComponent<TopPannelControl>().WeaponTop;
                break;
        }
    }
}

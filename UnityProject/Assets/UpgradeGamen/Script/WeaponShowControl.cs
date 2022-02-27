using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShowControl : MonoBehaviour
{
    public GameObject Katana;
    public GameObject Knife;
    public GameObject Taiken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (DataManager.Instance._WeaponNumberSword)
        {
            case 0:
                Katana.SetActive(true);
                Knife.SetActive(false);
                Taiken.SetActive(false);
                break;
            case 1:
                Katana.SetActive(false);
                Knife.SetActive(true);
                Taiken.SetActive(false);
                break;
            case 2:
                Katana.SetActive(false);
                Knife.SetActive(false);
                Taiken.SetActive(true);
                break;

        }
    }
}

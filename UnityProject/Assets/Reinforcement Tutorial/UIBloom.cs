using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBloom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        this.gameObject.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camela : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(Player.transform.position.x+2f, 2.8f, -8.45f);
    }

}

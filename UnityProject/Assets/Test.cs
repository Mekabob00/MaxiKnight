using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject Target = null;

    void Start()
    {
        
    }

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Target.GetComponent<IPlayerDamege>()._AddDamege(1);
        }



    }
}

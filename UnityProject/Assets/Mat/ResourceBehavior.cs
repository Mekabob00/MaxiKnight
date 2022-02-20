using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{

    [SerializeField, Tooltip("Šl“¾•¨¿—Ê")]
    private float _ResourceAmount = 10;

    [SerializeField, Tooltip("Šl“¾Effect")]
    private ParticleSystem _Effect;


    void Start()
    {

    }


    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Effect‚ÌÄ¶
            if (_Effect != null)
            {
                _Effect.Play();
            }


            //‘Œ¹‚ÌŠl“¾
            DataManager.Instance._Resource += (int)_ResourceAmount;
        }


    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{

    [SerializeField, Tooltip("l¾¨¿Ê")]
    private float _ResourceAmount = 10;

    [SerializeField, Tooltip("l¾Effect")]
    private ParticleSystem _Effect;
    [SerializeField, Tooltip("ACe")]
    private GameObject Item;
    [SerializeField, Tooltip("Audio Souce")]
    private AudioSource audioSource;
    public AudioClip SE;
    public AudioSource castleAudioSource;


    void Start()
    {
        castleAudioSource = GameObject.Find("Castle").GetComponent<AudioSource>();
    }


    void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("AAA");
            castleAudioSource.PlayOneShot(SE);
            //¹Ìl¾
            DataManager.Instance._Resource += (int)_ResourceAmount;
            Destroy(Item);
        }
    }
}

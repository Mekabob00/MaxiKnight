using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{

    [SerializeField, Tooltip("Šl“¾•¨Ž¿—Ê")]
    private float _ResourceAmount = 10;

    [SerializeField, Tooltip("Šl“¾Effect")]
    private ParticleSystem _Effect;
    [SerializeField, Tooltip("ƒAƒCƒeƒ€")]
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
            //Ž‘Œ¹‚ÌŠl“¾
            DataManager.Instance._Resource += (int)_ResourceAmount;
            Destroy(Item);
        }
    }
}

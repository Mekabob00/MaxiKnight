using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlay : MonoBehaviour
{
    public GameObject Audio;
    public AudioSource As;
    public AudioClip Chain;
    public AudioClip SelButton;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GameObject.Find("Audio");
        As = Audio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        As.PlayOneShot(Chain);
        As.PlayOneShot(SelButton);
	}
}

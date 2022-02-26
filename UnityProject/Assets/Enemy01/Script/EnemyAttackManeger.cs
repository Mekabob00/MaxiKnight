using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyAttackManeger : MonoBehaviour
{
    public static EnemyAttackManeger instance = null;

    public int Enemy1Attack;
    public Slider CastleSlider;
    public int CastleHP=10;
    public GameObject Castle;

    public AudioSource audioSource = null;
    // Start is called before the first frame
    private void Awake()
    {
        if (instance == null)
        {            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Enemy1Attack = 1;
        CastleHP = 3;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if(CastleHP<=0)
        {
            CastleHP = 0;
            Destroy(Castle);
        }
    }
    public void PlaySE(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("オーディオソースが設定されてない");
        }
    }
}

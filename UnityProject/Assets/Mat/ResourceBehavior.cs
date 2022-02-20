using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{

    [SerializeField, Tooltip("�l��������")]
    private float _ResourceAmount = 10;

    [SerializeField, Tooltip("�l��Effect")]
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
            //Effect�̍Đ�
            if (_Effect != null)
            {
                _Effect.Play();
            }


            //�����̊l��
            DataManager.Instance._Resource += (int)_ResourceAmount;
        }


    }


}

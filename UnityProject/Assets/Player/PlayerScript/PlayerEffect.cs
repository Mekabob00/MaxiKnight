using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField, Tooltip("EffectList")]
    private List<GameObject> _Effect;



    void Start()
    {

    }


    void Update()
    {

    }



    //Effect�𐶐�
    public GameObject CreateEffect(int Number,Vector3 pos)
    {
        return Instantiate(_Effect[Number], pos, new Quaternion(0, 0, 0, 0));
    }

}

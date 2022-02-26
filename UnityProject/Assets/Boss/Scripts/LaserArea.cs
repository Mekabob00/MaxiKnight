using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserArea : MonoBehaviour
{
    [Header("���[�U�[�_���[�W")]
    public int _Damage;

    private float timeCount;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //�^�C�}�[
            timeCount -= Time.deltaTime;
            if(timeCount <= 0)
            {
                Debug.Log("hit player!");
                timeCount = 0.1f;
                other.gameObject.GetComponent<Player_Controll>()._AddDamege(_Damage);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserArea : MonoBehaviour
{
    [Header("�e�X�g ���[�U�[�_���[�W")]
    public int _Damage;
    [Header("�U���Ԋu")]
    public float _IntervalTIme;

    private float timeCount;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //�^�C�}�[
            timeCount += Time.deltaTime;
            if(timeCount >= _IntervalTIme)
            {
                timeCount = 0;
                other.gameObject.GetComponent<Player_Controll>()._AddDamege(_Damage);
            }
        }
    }
}

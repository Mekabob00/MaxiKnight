using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefencePos : MonoBehaviour
{

    [SerializeField, Tooltip("���̃X�N���v�g")]
    private SwordControll _SwordControll = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //�͈͓��ɓ������u��
        if (collision.tag == "2st defense")
        {
            //�U���͑���
            _SwordControll.AttakUPFlag = true;
            Debug.Log("�͈͓�");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "2st defense")
        {
            //�͈͊O�ɏo����U���͂�ቺ
            _SwordControll.AttakUPFlag = false;

        }
    }


}

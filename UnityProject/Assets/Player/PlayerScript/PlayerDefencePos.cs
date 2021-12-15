using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefencePos : MonoBehaviour
{

    [SerializeField, Tooltip("剣のスクリプト")]
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
        //範囲内に入った瞬間
        if (collision.tag == "2st defense")
        {
            //攻撃力増加
            _SwordControll.AttakUPFlag = true;
            GlobalData.Instance.isPlayerInSecondLine = true;　//追加 (ジョ)
            Debug.Log("範囲内");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "2st defense")
        {
            //範囲外に出たら攻撃力を低下
            _SwordControll.AttakUPFlag = false;
            GlobalData.Instance.isPlayerInSecondLine = false;　//追加 (ジョ)
        }
    }


}

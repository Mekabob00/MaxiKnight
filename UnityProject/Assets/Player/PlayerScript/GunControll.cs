using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    [SerializeField, Tooltip("Animtor")]
    private Animator _PlayerAnimtor;

    [SerializeField, Tooltip("剣")]
    private GameObject _Sword;

    [SerializeField, Tooltip("銃")]
    private GameObject _Gun;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    //GanAnimeの最後にイベント発生
    public void GunAnimeEnd()
    {
        _Sword.SetActive(true);
        _PlayerAnimtor.SetBool("IsAttack", false);
    }

    public void GunAttack()
    {
        if (_PlayerAnimtor.GetCurrentAnimatorStateInfo(0).IsName("run") || _PlayerAnimtor.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                //剣を非表示にする
                _Sword.SetActive(false);

                _PlayerAnimtor.SetFloat("Wepon", 1);
                _PlayerAnimtor.SetBool("IsAttack", true);
            }
        }


    }

}

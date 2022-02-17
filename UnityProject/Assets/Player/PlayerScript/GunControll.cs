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

    [SerializeField, Tooltip("弾丸")]
    private GameObject _Bullet;

    [SerializeField, Tooltip("発射位置")]
    private Transform ShotPos;

    void Start()
    {
        _Gun.SetActive(false);
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
        _Gun.SetActive(false);
        _PlayerAnimtor.SetFloat("Wepon", 0);
    }

    public void BulletShot()
    {

        GameObject obj = Instantiate(_Bullet, ShotPos.position, new Quaternion(0, 0, 0, 0));
        obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1000));

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
                _Gun.SetActive(true);
                
            }
        }


    }

}

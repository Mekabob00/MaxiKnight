using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    [SerializeField, Tooltip("Animtor")]
    private Animator _PlayerAnimtor;

    [SerializeField, Tooltip("åï")]
    private GameObject _Sword;

    [SerializeField, Tooltip("èe")]
    private GameObject _Gun;

    [SerializeField, Tooltip("íeä€")]
    private GameObject _Bullet;

    [SerializeField, Tooltip("î≠éÀà íu")]
    private Vector3 ShotPos;

    private Ray ray;

    void Start()
    {
        _Gun.SetActive(false);
        ShotPos = this.transform.position;
        ShotPos.y += 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(_Gun.transform.position, this.transform.forward);
        //RayÇÃï\é¶
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

    }

    //GanAnimeÇÃç≈å„Ç…ÉCÉxÉìÉgî≠ê∂
    public void GunAnimeEnd()
    {
        _Sword.SetActive(true);
        _PlayerAnimtor.SetBool("IsAttack", false);
        _Gun.SetActive(false);
        _PlayerAnimtor.SetFloat("Wepon", 0);
    }

    public void BulletShot()
    {

        GameObject obj = Instantiate(_Bullet, ShotPos, new Quaternion(0, 0, 0, 0));
        obj.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1000));

        
        RaycastHit hit;

        //íeä€ÇÃìñÇΩÇËîªíË
        if (Physics.Raycast(ray, out hit))
        {
            //çUåÇÇÃèàóù

        }
        

    }


    public void GunAttack()
    {
        

        if (Input.GetKeyDown(KeyCode.X))
        {
            //åïÇîÒï\é¶Ç…Ç∑ÇÈ
            _Sword.SetActive(false);

            _PlayerAnimtor.SetFloat("Wepon", 1);
            //          _PlayerAnimtor.SetBool("IsAttack", true);
            _PlayerAnimtor.SetTrigger("Attack");
            _Gun.SetActive(true);


            
        }
         
        


    }

}

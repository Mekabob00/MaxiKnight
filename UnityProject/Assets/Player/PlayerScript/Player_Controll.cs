//更新日 12月10日 担当:下川和馬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controll : MonoBehaviour
{
    [SerializeField]
    float PlayerWlakSpeed;
    [SerializeField]
    private float applySpeed = 0.2f;       // 回転の適用速度

    [SerializeField]
    public int PlayerAttackPoint;


    private Vector3 latestPosition;
    private Animator PlayerAttackAnimator;

    //-----------------------------------------------
    //攻撃コライダー追加（ジョ）
    [SerializeField]private Collider attackArea;
    //-----------------------------------------------

    private void Awake()
    {//スタート関数前に何か初期化する時用
    }
    void Start()
    {
        PlayerAttackAnimator = GetComponent<Animator>();
        latestPosition = transform.position;
        PlayerAttackPoint = 10;
    }
    void Update()
    {
        PlayerWalk();//プレイヤー移動関数呼び出し
        PlayerAttackAnimation();//Zキーを押した時にアニメーションをさせる


        //-----------------------------------------------
        //プレイヤー死亡（テスト用）
        if (Input.GetKeyDown(KeyCode.K))
        {
            GlobalData.Instance.isPlayerDead = true;
            GlobalData.Instance.isPlayerInSecondLine = false;
            Destroy(gameObject);
        }
        //-----------------------------------------------
    }

    



    private void PlayerWalk()
    {//プレイヤーの移動関数
        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerWlakSpeed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * PlayerWlakSpeed;

        transform.position = new Vector3(
            transform.position.x + dx, 0.5f, transform.position.z + dz
        );
        Vector3 diff = transform.position - latestPosition;   //現在居る位置からどの方向に進んだか
        latestPosition = transform.position;
        diff.y = 0;//y方向は無視する
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //プレイヤーの向き変更
            PlayerAttackAnimator.SetBool("Run", true);
        }
        else
        {
            PlayerAttackAnimator.SetBool("Run", false);
        }
    }
    public void PlayerAttackAnimation()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //攻撃アニメーション再生
            PlayerAttackAnimator.SetTrigger("Attack");
        }
    }
    public void _AddDamege(int _Damege)
    {//使わないかも
    }

    //---------------------------------------------------------------
    //追加 AnimationEvent 攻撃コライダーの有効化と無効化（ジョ）
    public void StartAttack()
    {
        attackArea.enabled = true;
    }
    public void EndAttack()
    {
        attackArea.enabled = false;
    }
    //---------------------------------------------------------------
}


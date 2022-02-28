using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Behaviour : MonoBehaviour, IPlayerDamege
{
    #region SeliarizFild
    [SerializeField, Tooltip("通常の移動速度")]
    private float _MoveSpeed = 10.0f;

    [SerializeField, Tooltip("スローモーション時の移動速度")]
    private float _SlowMoveSpeed = 1.0f;

    [SerializeField, Tooltip("体力,float型")]
    private float _HP;

    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField, Tooltip("Renderer")]
    private Renderer _Renderer = null;

    [SerializeField, Tooltip("グローバルデータ")]
    private GameObject _GlobalDataObject = null;
    [SerializeField, Tooltip("エフェクト")]
    private GameObject Effct;
    [SerializeField, Tooltip("死亡時のエフェクト")]
    private GameObject DidEffect;
    [SerializeField, Tooltip("SE")]
    private AudioClip Damege;
    [SerializeField, Tooltip("アイテム")]
    private GameObject Item;
    [SerializeField, Tooltip("Player")]
    private GameObject Player;
    [SerializeField, Tooltip("死亡時のSE")]
    private AudioClip DidSe;
 


    public bool _IsEnemy4explosionFlag;
    #endregion

    #region Defalut


    private GlobalData _GlobalData = null;

    private float _HighPos = 1.0f;
    private float PlayerStopTime=0.0f;
    public float span = 3f;
    private float currentTime = 0f;

    public bool _IsMoveActive = false;
    private bool _IsAttackFlag = false;

    #endregion

    //城
    [SerializeField]
    private GameObject castle;
    [SerializeField]
    private GameObject Enemy;
    public Vector3 PlayerPosition;
    private Vector3 EnemyPosition;

    private float dis;
    [SerializeField]
    private float FocusSpeed;

    //Player
    public Rigidbody _PlayerRigidBody;

    #region Unity function
    private void Start()
    {
        _IsEnemy4explosionFlag = false;
        _IsMoveActive = true;
        _IsAttackFlag = false;
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();
    }
    void Update()
    {
        if (_IsMoveActive && !_IsAttackFlag)
        {
            _RigidBody.ForontMove(this.transform, _MoveSpeed);
        }
        else 
        {
            _RigidBody.ForontMove(this.transform, 0.0f);
        }
        if (_IsAttackFlag)
        {
            IsAttackFlag();
        }
    }
    #endregion

    #region public function

    /// <summary>
    /// ダメージの加算
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void _AddDamege(float _Damege)
    {
        var after = _HP - _Damege;

        //体力が0なら
        if (after <= 0)
        {
            //仮
            Instantiate(DidEffect, transform.position, transform.rotation);
            EnemyAttackManeger.instance.PlaySE(DidSe);
            Instantiate(Item, transform.position, transform.rotation);
            Destroy(this.gameObject);
            return;
        }
        else if (after != _HP)//ダメージを受けたら
        {
            Instantiate(Effct, transform.position, transform.rotation);
            EnemyAttackManeger.instance.PlaySE(Damege);
            //ダメージを受けた時の処理
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }
    public void OnCollisionEnter(Collision collision)
    {
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //現在仮タグでEnemyと付けています。随時変更していただけると助かります
        {
            transform.SetParent(Player.transform);
            Player.GetComponent<Player_Controll>().enabled = false;
            Player.GetComponent<Animator>().enabled = false;
            this.transform.rotation = Quaternion.Euler(0, -90, 70);
            _IsMoveActive = false;
            _IsAttackFlag = true;
        }
        if (other.gameObject.tag == "Castle")
        {
            Instantiate(DidEffect, transform.position, transform.rotation);
            EnemyAttackManeger.instance.PlaySE(DidSe);
            Destroy(gameObject);
        }
    }

    #endregion

    #region private function
    private void IsAttackFlag()
    {
        currentTime += Time.deltaTime;
        if(currentTime>=1.5f)
        {
            Instantiate(DidEffect, transform.position, transform.rotation);
            this.transform.position = new Vector3(1000, 1000, 1000);
            Player.GetComponent<Player_Controll>().enabled =true;
            Player.GetComponent<Animator>().enabled = true;
            EnemyAttackManeger.instance.PlaySE(DidSe);
            Destroy(Enemy);
            currentTime = 0.0f;
        }
    }
  /*  private void InPlayerStop()
    {
        if (_IsEnemy4explosionFlag)
        {
            PlayerStopTime += Time.deltaTime;
            if (PlayerStopTime >= 2.0f)
            {
                Destroy(Enemy);
                Player.GetComponent<Animator>().enabled = true;
                Player.GetComponent<Player_Controll>().enabled = true;
            }
        }
    }*/
    private void OnTriggerExit(Collider other)
    {

        //最終防衛ラインを超えたら動きを止めるStop
        if (other.tag == "Stage")
        {
            _IsMoveActive = false;
        }
    }

    #endregion

    #region コルーチン
    IEnumerator AddDamageMove()
    {
        //重力をONにする
        //  _RigidBody.useGravity = true;
        //飛び上がる
        //   _RigidBody.AddForce(new Vector3(0, 300.0f, 0));

        //[TODO]
        //初撃に対して色を変更する

        yield break;
    }
    #endregion
}

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
    [SerializeField, Tooltip("SE")]
    private AudioClip Damege;
    [SerializeField, Tooltip("アイテム")]
    private GameObject Item;

    #endregion

    #region Defalut


    private GlobalData _GlobalData = null;

    private float _HighPos = 1.0f;

    public float span = 3f;
    private float currentTime = 0f;

    //Flag
    private bool _IsAddDamageEffect = false;
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
        _IsAddDamageEffect = false;
        _IsMoveActive = true;
        _IsAttackFlag = false;
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();
    }
    void Update()
    {
        Debug.Log(_PlayerRigidBody);
        PlayerFocus();
        if (_IsMoveActive && !_IsAttackFlag)
        { 
              Enemy.transform.position= Vector3.MoveTowards(transform.position, castle.transform.position,2*Time.deltaTime);
        }
        else
        {
            
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
        if (after<= 0)
        {
            //仮
            Instantiate(Effct, transform.position, transform.rotation);
            EnemyAttackManeger.instance.PlaySE(Damege);
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

           _PlayerRigidBody.constraints = RigidbodyConstraints.FreezePositionX;
        }
    }

    #endregion

    #region private function

    private void PlayerFocus()
    {
        // 対象物と自分自身の座標からベクトルを算出してQuaternion(回転値)を取得
        Vector3 vector3 = castle.transform.position - this.transform.position;
      
        Quaternion quaternion = Quaternion.LookRotation(vector3);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, quaternion, Time.deltaTime * FocusSpeed);
    }
    private void IsAttackFlag()
      {
          PlayerPosition = castle.transform.position;
          EnemyPosition = Enemy.transform.position;
          dis = Vector3.Distance(PlayerPosition, EnemyPosition);
          if (dis < 5f)
          {
              _IsAttackFlag = true;
              _IsMoveActive = false;
          }
          else
          {
              _IsAttackFlag = false;
              _IsMoveActive = true;
          }
          Debug.Log("距離" + dis);
      }
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

        _IsAddDamageEffect = true;

        yield break;
    }
    #endregion
}

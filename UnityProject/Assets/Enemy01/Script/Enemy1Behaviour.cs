using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behaviour : MonoBehaviour, IPlayerDamege
{
    #region SeliarizFild
    [SerializeField, Tooltip("通常の移動速度")]
    private float _MoveSpeed = 10.0f;

    [SerializeField, Tooltip("スローモーション時の移動速度")]
    private float _SlowMoveSpeed = 1.0f;

    [SerializeField, Tooltip("体力,int型")]
    private int _HP;

    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField, Tooltip("Renderer")]
    private Renderer _Renderer = null;

    [SerializeField, Tooltip("グローバルデータ")]
    private GameObject _GlobalDataObject = null;

    #endregion

    #region Defalut


    private GlobalData _GlobalData = null;
    private Collider _MyCollider=null;

    private float _HighPos = 1.0f;

    //Flag
    private bool _IsAddDamageEffect = false;
    private bool _IsMoveActive = false;

    #endregion


    #region Unity function
    void Start()
    {
        _IsAddDamageEffect = false;
        _IsMoveActive = true;

        //グローバルデータのコンポーネントを取得
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();

        //コライダーを取得する
        _MyCollider = GetComponent<Collider>();

    }

    void Update()
    {
        if (_IsAddDamageEffect)//ダメージを食らった後のEffect最中
        {
            if (this.transform.position.y < _HighPos)
            {
                //当たり判定を有効
                _MyCollider.enabled = true;


                //一定の高さのになったらEffectを終了させる
                _IsAddDamageEffect = false;
                _RigidBody.useGravity= false;

                //位置を補正
                Vector3 pos = this.transform.position;
                pos.y = _HighPos;
                this.transform.position = pos;
            }
            return;
        }


        if (_IsMoveActive)
        {

            //前方進む
            if (!_GlobalData.isCanonAppear)//通常
            {
                _RigidBody.ForontMove(this.transform, _MoveSpeed);
            }
            else
            {
                //スロー移動
                _RigidBody.ForontMove(this.transform, _SlowMoveSpeed);
            }

        }


    }

    private void OnTriggerEnter(Collider other)
    {


        
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

    #region public function

    /// <summary>
    /// ダメージの加算
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void _AddDamege(int _Damege)
    {
        if (_IsAddDamageEffect || !_IsMoveActive)//ダメージを与えない
        {
            return;
        }

        var after = _HP - _Damege;

        //体力が0なら
        if (after <= 0)
        {
            //仮
            Destroy(this.gameObject);
            return;
        }
        else if (after != _HP)//ダメージを受けたら
        {

            //ダメージを受けた時の処理
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }

    #endregion

    #region private function


    #endregion

    #region Coroutine
    IEnumerator AddDamageMove()
    {
        //重力をONにする
        _RigidBody.useGravity= true;

        //飛び上がる
        _RigidBody.AddForce(new Vector3(0,300.0f,0));

        //当たり判定を無効
        _MyCollider.enabled = false;

        //[TODO]
        //初撃に対して色を変更する

        _IsAddDamageEffect = true;

        yield break;
    }

    




    #endregion


}

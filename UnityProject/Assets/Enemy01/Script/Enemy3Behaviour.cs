using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Behaviour : MonoBehaviour, IPlayerDamege
{
    #region SeliarizFild
    [SerializeField, Tooltip("通常の移動速度")]
    private float _MoveSpeed = 10.0f;

    [SerializeField, Tooltip("スローモーション時の移動速度")]
    private float _SlowMoveSpeed = 1.0f;

    [SerializeField, Tooltip("体力,int型")]
    private float _HP;

    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField, Tooltip("Renderer")]
    private Renderer _Renderer = null;

    [SerializeField, Tooltip("グローバルデータ")]
    private GameObject _GlobalDataObject = null;
    [SerializeField, Tooltip("Enemyオブジェクト取得")]
    private GameObject Enemy3;
    [SerializeField, Tooltip("Enemy3攻撃力")]
    private int _Damage;

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
    public Vector3 castlePosition;
    private Vector3 EnemyPosition;

   
    [SerializeField, Tooltip("ダメージ効果音")]
    private AudioClip DamegeSE;

    [SerializeField, Tooltip("攻撃SE")]
    private AudioClip AttackSE;

    [SerializeField, Tooltip("死亡SE")]
    private AudioClip DieSE;
    [SerializeField, Tooltip("エフェクト")]
    private GameObject Effect;
    [SerializeField,Tooltip("アニメーション")]
    private Animator EnemyAnimator;

    private float dis;

    #region Unity function
    private void Start()
    {
        _Damage = 1;
        _IsAddDamageEffect = false;
        _IsMoveActive = true;
        _IsAttackFlag = false;
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();
    }
    void Update()
    {
        if (_IsAddDamageEffect)//ダメージを食らった後のEffect最中
        {
            if (this.transform.position.y < _HighPos)
            {
                //一定の高さのになったらEffectを終了させる
                _IsAddDamageEffect = false;
                _RigidBody.useGravity = false;

                //位置を補正
                Vector3 pos = this.transform.position;
                pos.y = _HighPos;
                this.transform.position = pos;
            }
            return;
        }
        if (_IsMoveActive && !_IsAttackFlag)
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
    #endregion

    #region public function

    /// <summary>
    /// ダメージの加算
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void _AddDamege(float _Damege)
    {
     
        if (_IsAddDamageEffect)//ダメージを与えない
        {
            return;
        }

        var after = _HP - _Damege;

        //体力が0なら
        if (after<=0)
        {
            //仮
            EnemyAnimator.SetTrigger("Damege");
            Instantiate(Effect, transform.position, transform.rotation);
            Destroy(Enemy3);
            return;
        }
        else if (after != _HP)//ダメージを受けたら
        {
            EnemyAnimator.SetTrigger("Damege");
            Instantiate(Effect, transform.position, transform.rotation);
            EnemyAttackManeger.instance.PlaySE(DamegeSE);
            //ダメージを受けた時の処理
            StartCoroutine(AddDamageMove());

        }

        _HP = after;
    }
    public void AnimationStop()
    {
        _RigidBody.constraints = RigidbodyConstraints.FreezePositionX;

    }
    public void AnimationStart()
    {
        _RigidBody.constraints = RigidbodyConstraints.None;
        _RigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    #endregion

    #region private function
    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag== "Castle")
        {
            castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
            Destroy(Enemy3);
        }
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
    /*    //重力をONにする
        _RigidBody.useGravity = true;
        //飛び上がる
        _RigidBody.AddForce(new Vector3(0, 300.0f, 0));

        //[TODO]
        //初撃に対して色を変更する

        _IsAddDamageEffect = true;
    */
        yield break;
    }






    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1BossBehaviour : MonoBehaviour, IPlayerDamege
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

    [SerializeField, Tooltip("ダメージ効果音")]
    private AudioClip DamegeSE;

    [SerializeField, Tooltip("攻撃SE")]
    private AudioClip AttackSE;

    [SerializeField, Tooltip("死亡SE")]
    private AudioClip DieSE;
    [SerializeField, Tooltip("Enemy1攻撃力")]
    private int _Damage;
    [SerializeField, Tooltip("エフェクト")]
    private GameObject Effect;
    [SerializeField, Tooltip("死亡時のエフェクト")]
    private GameObject DidEffect;
    [SerializeField, Tooltip("アイテム生成")]
    private GameObject Item;
    [SerializeField, Tooltip("クリアウインドウ")]
    private GameObject panel;
    [SerializeField, Tooltip("オーラ")]
    private GameObject Aura;
    #endregion

    #region Defalut

    private Animator EnemyAnimator;

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

    private float dis;

 

    #region Unity function
    private void Start()
    {
        panel.SetActive(false);
        _Damage = 1;
        _IsAddDamageEffect = false;
        _IsMoveActive = true;
        _IsAttackFlag = false;
        var obj = GameObject.Find("GlobalData");
        _GlobalData = obj.GetComponent<GlobalData>();
        EnemyAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        IsAttackFlag();
      
        if (_IsMoveActive && !_IsAttackFlag)
        {
            EnemyAnimator.SetBool("Walk", true);
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
        else
        {
            EnemyAnimator.SetBool("Walk", false);
            _RigidBody.ForontMove(this.transform, 0.0f);
            IsAttack();
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
            EnemyAttackManeger.instance.PlaySE(DieSE);
            Destroy(this.gameObject);
            EnemyAnimator.SetTrigger("Damege");
            EnemyDestroy("Enemy");
            panel.SetActive(true);
            Instantiate(DidEffect, transform.position, transform.rotation);
            Instantiate(Item, transform.position, Item.transform.rotation);
            DataManager.Instance._Stage++;
            return;
        }
        else if (after != _HP)//ダメージを受けたら
        {
            EnemyAnimator.SetTrigger("Damege");
            EnemyAttackManeger.instance.PlaySE(DamegeSE);
            Instantiate(Effect, transform.position, transform.rotation);
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
    public void EnemyDestroy(string tag_Enemyname)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag_Enemyname);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
    #endregion

    #region private function

    private void IsAttackFlag()
    {
        castlePosition = castle.transform.position;
        EnemyPosition = Enemy.transform.position;
        dis = Vector3.Distance(castlePosition, EnemyPosition);
        if (dis < 10.0f)
        {
            _IsAttackFlag = true;
            _IsMoveActive = false;
        }
        //  Debug.Log("距離" + dis);
    }
    private void IsAttack()
    {
        if (DataManager.Instance._CastleHP >= 1)
        {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {
                EnemyAttackManeger.instance.PlaySE(AttackSE);
                EnemyAnimator.SetTrigger("Attack");
                castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
                Debug.Log("城に攻撃");
                currentTime = 0f;
            }
        }
        else if (DataManager.Instance._CastleHP <= 0)
        {
            return;
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

    #region コルーチン
    IEnumerator AddDamageMove()
    {
        /*   //重力をONにする
           _RigidBody.useGravity = true;
           //飛び上がる
           _RigidBody.AddForce(new Vector3(0, 300.0f, 0));
        */
        //EnemyAnimator.SetTrigger("Damege");

        //[TODO]
        //初撃に対して色を変更する

        _IsAddDamageEffect = true;

        yield break;
    }





}
#endregion
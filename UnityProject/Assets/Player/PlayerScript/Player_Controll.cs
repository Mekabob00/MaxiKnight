//更新日 12月10日 担当:下川和馬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controll : MonoBehaviour,IPlayerDamege
{
    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField]
    float PlayerWlakSpeed;
    [SerializeField]
    private float applySpeed = 0.2f;       // 回転の適用速度

    [SerializeField]
    public int PlayerAttackPoint;

    [SerializeField, Tooltip("当たり判定")]
    private Collider _HitBase;

    [SerializeField, Tooltip("武器のオブジェクト")]
    private GameObject _SwordObject = null;

    [SerializeField, Tooltip("攻撃速度")]
    private float _AttackSpeed = 1.0f;

    [SerializeField, Tooltip("連続攻撃の猶予時間")]
    private float _CombAttackGraceTime = 1.0f;

    [SerializeField, Tooltip("攻撃パターンの種類")]
    private int _AttackPatternNum = 3;



    [SerializeField, Tooltip("回避の大きさ")]
    private float _AvoidanceValue = 50.0f;

    [SerializeField, Tooltip("最大ライフ")]
    private float _MAXHP = 100;

    private Vector3 latestPosition;
    private Animator PlayerAttackAnimator;
    public Collider _SwordCollider;
    private GunControll _GunControll;
    
    private int _AttackType = 0;
    private float _NextActionTime;


    private bool _IsGaraceTime = false;
    private bool _IsAvoid = false;
    private bool _IsAttack = false;

    public float SmoothTime = 2f;
    public float Speed = 1f;
    public float JourneyLength = 10f;
    private float _StartTime = 0;
    private float _NowHP;


    private Vector3 AvoidPos_Start = new Vector3();
    private Vector3 AvoidPos_End = new Vector3();


    public static float AttackBuff = 1;


    private void Awake()
    {//スタート関数前に何か初期化する時用
    }
    void Start()
    {
        //animtion
        PlayerAttackAnimator = GetComponent<Animator>();
        PlayerAttackAnimator.SetFloat("AttackSpeed", 1);

        _GunControll = GetComponent<GunControll>();

        latestPosition = transform.position;
        PlayerAttackPoint = 10;
        _AttackType = 0;
        _StartTime = Time.time;
        _NowHP = _MAXHP;
        AttackBuff = DataManager.Instance._PlayerAttackBuff;


        //武器の当たり判定の設定
        _SwordCollider = _SwordObject.GetComponent<BoxCollider>();
        _SwordCollider.enabled = false;//コライダーはOFF

        _NextActionTime = _CombAttackGraceTime;

        _IsGaraceTime = false;
        _IsAvoid = false;
        _IsAttack = false;
    }
    void Update()
    {
        

        if (_IsAvoid)
        {
            Avoidance();
            return;
        }

        PlayerWalk();//プレイヤー移動関数呼び出し
        PlayerAttackAnimation();//Zキーを押した時にアニメーションをさせる
        PlayerAvoidance();
        PlayercolliderONOFF();

        //Playerの攻撃力反映
        AttackBuff = DataManager.Instance._PlayerAttackBuff;
    }


    float CalcMoveRatio()
    {
        var distCovered = (Time.deltaTime - _StartTime) * 5;
        return distCovered / JourneyLength;
    }

    private void PlayercolliderONOFF()
    {
        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("OnDamage")||!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _HitBase.enabled = true;
        }


    }

    private void PlayerWalk()
    {//プレイヤーの移動関数
        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerWlakSpeed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * PlayerWlakSpeed;

        bool IsAttack = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

        //攻撃時に動かさない
        if (!IsAttack)
        {
            transform.position = new Vector3(
            transform.position.x + dx, 0.5f, transform.position.z + dz
        );
        }
        

        Vector3 diff = transform.position - latestPosition;   //現在居る位置からどの方向に進んだか
        latestPosition = transform.position;
        diff.y = 0;//y方向は無視する
        if (diff.magnitude > 0.01f)
        {
            if (!_IsAvoid)
            {
                transform.rotation = Quaternion.LookRotation(diff); //プレイヤーの向き変更
                PlayerAttackAnimator.SetBool("Run", true);
            }
            
        }
        else
        {
            PlayerAttackAnimator.SetBool("Run", false);
        }





    }

    /// <summary>
    /// 回避
    /// </summary>
    private bool PlayerAvoidance()
    {
        //攻撃モーション中
        if (!_IsGaraceTime && PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return false;
        }

        if (Input.GetKeyDown(KeyCode.C) && !_IsAvoid)
        {

            PlayerAttackAnimator.SetTrigger("Avoid");

            _IsAvoid = true;
            AvoidPos_Start = transform.position;
            AvoidPos_End = transform.position + transform.forward * _AvoidanceValue;
            return true;
        }

        return false;
    }

    private void Avoidance()
    {
        transform.position = Vector3.Lerp(transform.position, AvoidPos_End, CalcMoveRatio());

        if (PlayerAttackAnimator.IsInTransition(0))//遷移中
        {
            return;
        }

        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _IsAvoid = false;
        }

    }

    public void PlayerAttackAnimation()
    {

        
        if (PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("run") || PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            //剣の攻撃
            PlayerAttackMove();

            //銃での攻撃
            _GunControll.GunAttack();
        }
       


    }
    public void _AddDamege(float _Damege)
    {
        _NowHP -= _Damege;
        PlayerAttackAnimator.SetTrigger("OnDamage");
        _HitBase.enabled = false;
    }

    public void AttackAnimEnd()
    {
        _IsAttack = false;
        PlayerAttackAnimator.SetFloat("AttackSpeed", 0);
        //0.5秒にColliderをOFF
        _SwordCollider.enabled = false;

        //猶予時間のリセット
        _NextActionTime = 0;

        //猶予時間の始まり
        _IsGaraceTime = true;
        StartCoroutine(AttackColliderTime());

        PlayerAttackAnimator.SetBool("IsAttack", false);
    }

    //剣での攻撃
    bool PlayerAttackMove()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_SwordCollider.enabled)
        {
            _SwordCollider.enabled = true;
          
            PlayerAttackAnimator.SetTrigger("Attack");
            
            PlayerAttackAnimator.Play("Attack", 0, 0);
            return true;
        }
        return false;
    }

    IEnumerator AttackColliderTime()
    {
        float time = 0;

        while (time <= 0.5f)//猶予時間
        {
            //攻撃イベントが発生したら
            if (PlayerAttackMove())
            {
                //連続攻撃のカウント
                _AttackType++;
                if (_AttackType >= 3)
                {
                    _AttackType = 0;
                }
                PlayerAttackAnimator.SetFloat("AttackType", _AttackType);
                PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed);

                yield break;
            }
            else if (PlayerAvoidance())//回避
            {
                break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed);

        _AttackType = 0;
        PlayerAttackAnimator.SetFloat("AttackType", _AttackType);
        yield break;
    }

}


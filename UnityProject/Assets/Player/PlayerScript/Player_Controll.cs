//更新日 12月10日 担当:下川和馬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controll : MonoBehaviour, IPlayerDamege
{
    [SerializeField, Tooltip("RigidBody")]
    private Rigidbody _RigidBody = null;

    [SerializeField]
    private List<float> PlayerWlakSpeed;
    [SerializeField]
    private float applySpeed = 0.2f;       // 回転の適用速度

    [SerializeField]
    public int PlayerAttackPoint;

    [SerializeField, Tooltip("当たり判定")]
    private Collider _HitBase;

    [SerializeField, Tooltip("武器の当たり判定")]
    private List<Collider> _SwordColliderList;

    [SerializeField, Tooltip("武器のオブジェクト")]
    private SwordControll _SwordBeh = null;

    [SerializeField, Tooltip("攻撃速度")]
    private List<float> _AttackSpeed;

    [SerializeField, Tooltip("連続攻撃の猶予時間")]
    private List<float> _AttackGraceTime;

    [SerializeField, Tooltip("回避の大きさ")]
    private float _AvoidanceValue = 50.0f;

    [SerializeField, Tooltip("回避のリロード時間")]
    private float AvoidReLoadTime = 3.0f;

    [SerializeField, Tooltip("レーン座標Y")]
    private List<float> _LanePosYList;

    [SerializeField, Tooltip("レーンの座標Z")]
    private List<float> _LanePosZList;

    [SerializeField, Tooltip("大きさ")]
    private List<float> _ScaleList;

    [SerializeField, Tooltip("最大ライフ")]
    private float _MAXHP = 100;

    private Vector3 latestPosition;
    private Animator PlayerAttackAnimator;
    public Collider _SwordCollider;
    private GunControll _GunControll;

    //Effect
    [SerializeField]
    private ParticleSystem RunSmork_P;
    [SerializeField]
    private ParticleSystem JunpDownSmork_P;

    private int _AttackType = 0;
    private float _NextActionTime;


    private bool _IsGaraceTime = false;
    public bool _IsAvoid = false;
    private bool _IsAttack = false;
    private bool _IsLaneChamge = false;



    public float SmoothTime = 2f;
    public float Speed = 1f;
    public float JourneyLength = 10f;
    private float _StartTime = 0;
    private float _NowHP;
    private int _NowLane;
    private float Input_tmp = 0;
    private float InputZ_tmp = 0;
    private Vector3 AvoidPos_Start = new Vector3();
    private Vector3 AvoidPos_End = new Vector3();
    private float AvoidAngle = 0;


    //外部からのデータを受け取る
    private int DataSowrdNum;
    public static float AttackBuff = 1;


    private void Awake()
    {//スタート関数前に何か初期化する時用
    }
    void Start()
    {
        //外部からデータを受け取る
        DataSowrdNum = DataManager.Instance._WeaponNumberSword;

        //animtion
        PlayerAttackAnimator = GetComponent<Animator>();
        PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed[DataSowrdNum]);
        PlayerAttackAnimator.SetFloat("RunSpeed", PlayerWlakSpeed[DataSowrdNum] * 0.7f);

        _GunControll = GetComponent<GunControll>();

        latestPosition = transform.position;
        PlayerAttackPoint = 10;
        _AttackType = 0;
        _StartTime = Time.time;
        _NowHP = _MAXHP;
        AttackBuff = DataManager.Instance._PlayerAttackBuff;

        _NowLane = 0;//初期

        //武器の当たり判定の設定
        _SwordCollider = _SwordColliderList[DataSowrdNum];
        _SwordCollider.enabled = false;//コライダーはOFF

        Input_tmp = 1;
        _IsGaraceTime = false;
        _IsAvoid = false;
        _IsAttack = false;

    }
    void Update()
    {

        bool isAvoid = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance");
        if (isAvoid)
        {
            Avoidance();
            return;
        }

        PlayerWalk();//プレイヤー移動関数呼び出し
        PlayerAttackAnimation();//Zキーを押した時にアニメーションをさせる
        PlayerAvoidance();
        PlayercolliderONOFF();
        ChangeLaneMove();


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
        if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("OnDamage") || !PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            _HitBase.enabled = true;
        }


    }

    private void PlayerWalk()
    {//プレイヤーの移動関数
        float dx = Input.GetAxisRaw("Horizontal") * Time.deltaTime * PlayerWlakSpeed[DataSowrdNum]*7;
        float dz = Input.GetAxisRaw("Vertical") * Time.deltaTime * 1;

        bool IsAttack = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        bool IschangeLane = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("ChangeLane");
        bool IsAvoid = PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance");

        //ローカル設定
        const float PositionX_Left = 30;
        const float PositionX_Right = 0.3f;

        //攻撃時に動かさない
        if (!IsAttack && !IschangeLane)
        {

            transform.position = new Vector3(
            transform.position.x + dx, _LanePosYList[_NowLane], _LanePosZList[_NowLane]);

            //バグ修正
            _SwordCollider.enabled = false;

            //アニメーション再生
            if (dx != 0.0f)
            {
                PlayerAttackAnimator.SetBool("Run", true);

                //Effectの再生
                RunSmork_P.Play();
            }
            else
            {
                PlayerAttackAnimator.SetBool("Run", false);
                RunSmork_P.Stop();
            }

            //Xじくの限界設定
            if (transform.position.x >= PositionX_Left)
            {
                transform.position = new Vector3(PositionX_Left, _LanePosYList[_NowLane], _LanePosZList[_NowLane]);
            }
            else if(transform.position.x<=PositionX_Right)
            {
                transform.position = new Vector3(PositionX_Right, _LanePosYList[_NowLane], _LanePosZList[_NowLane]);
            }
            else
            {
                
            }
        }

        //向きデータを保存
        if (dx != 0)
        {
            Input_tmp = dx;
        }


        Vector3 diff = transform.position - latestPosition;   //現在居る位置からどの方向に進んだか
        latestPosition = transform.position;
        diff.y = 0;//y方向は無視する
        diff.z = 0;
        if (diff.magnitude > 0.01f)
        {
            if (!IsAvoid||!_IsLaneChamge)
            {
                transform.rotation = Quaternion.LookRotation(diff); //プレイヤーの向き変更
                
            }

        }
        else
        {
            
        }

        //レーン移動
        if (!_IsLaneChamge)
        {
            ChangeLaneFun(dz);
        }


    }

    /// <summary>
    /// レーン移動
    /// </summary>
    /// <param name="inp">入力情報</param>
    void ChangeLaneFun(float inp)
    {
        if (inp != 0)
        {
            if (_NowLane == 0)
            {
                if (inp < 0)
                {
                    return;
                }
            }
            else
            {
                if (inp > 0)
                {
                    return;
                }
            }
            //アニメーション再生
            PlayerAttackAnimator.SetTrigger("ChangeLane");

            InputZ_tmp = inp;
        }


       

    }

    /// <summary>
    /// レーン移動するときの動き
    /// </summary>
    void ChangeLaneMove()
    {
        //2点間の距離を代入する
        float distance = _LanePosZList[1] - _LanePosZList[0];

        //現在の位置
        float present_Location = (Time.time*0.01f) / distance;

        if (_IsLaneChamge)
        {
            //オブジェクトの移動
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, _LanePosYList[_NowLane], _LanePosZList[_NowLane]), 0.08f);

            //レーン移動の終了
            if (present_Location >= 1)
            {
                //_IsLaneChamge = false;
            }

        }



    }

    void ChnageLanetoScale()
    {
        this.transform.localScale = new Vector3(_ScaleList[_NowLane], _ScaleList[_NowLane], _ScaleList[_NowLane]);
    }

    public void ChangeLaneAnime_Start()
    {
        _IsLaneChamge = true;

        if (InputZ_tmp > 0)//加算
        {
            _NowLane++;
            //上限保護
            if (_NowLane >= _LanePosZList.Count)
            {
                _NowLane = _LanePosZList.Count - 1;
                return;
            }
        }
        else if (InputZ_tmp < 0)//減算
        {
            _NowLane--;
            //下限保護
            if (_NowLane < 0.00f)
            {
                _NowLane = 0;
                return;
            }
        }

        ChnageLanetoScale();
    }

    public void ChangeLaneAnime_End()
    {
        _IsLaneChamge = false;
        PlayerAttackAnimator.ResetTrigger("ChangeLane");

        JunpDownSmork_P.Stop();
        if (JunpDownSmork_P.isStopped)
        {
            JunpDownSmork_P.Play();
        }
        

    }

    public int GetNowLane()
    {
        return _NowLane;
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
            //アニメーション再生
            PlayerAttackAnimator.SetTrigger("Avoid");

            //バグの保険
            _SwordCollider.enabled = false;

            _IsAvoid = true;
            AvoidPos_Start = transform.position;

            if (Input_tmp > 0)
            {
                AvoidPos_End = transform.position + new Vector3(_AvoidanceValue, 0, 0);//普通の回
            }
            else
            {
                AvoidPos_End = transform.position + new Vector3(-_AvoidanceValue, 0, 0);//普通の回
            }
            StartCoroutine(AvoidReLoad_IE());

            return true;
        }

        return false;
    }

    private void Avoidance()
    {


        if (PlayerAttackAnimator.IsInTransition(0))//遷移中
        {
            return;
        }
        else if (!PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            //_IsAvoid = false;
        }
        else if (PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Avoidance"))
        {
            transform.position = Vector3.Lerp(transform.position, AvoidPos_End, CalcMoveRatio());
        }

    }

    public void PlayerAttackAnimation()
    {


        if (PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("run") || PlayerAttackAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            //剣の攻撃
            PlayerAttackMove();

            //銃での攻撃
            //_GunControll.GunAttack();
        }



    }
    public void _AddDamege(float _Damege)
    {
        _NowHP -= _Damege;
        PlayerAttackAnimator.SetTrigger("OnDamage");
        _HitBase.enabled = false;
    }

    public void AttackAnim_ColldierON()
    {
        _SwordCollider.enabled = true;
    }

    public void AttackAnimEnd()
    {
        _IsAttack = false;
        PlayerAttackAnimator.SetFloat("AttackSpeed", 0);
        //0.5秒にColliderをOFF
        _SwordCollider.enabled = false;

        //猶予時間の始まり
        _IsGaraceTime = true;
        StartCoroutine(AttackColliderTime());

        PlayerAttackAnimator.ResetTrigger("Attack");
    }

    //剣での攻撃
    bool PlayerAttackMove()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !_SwordCollider.enabled)
        {
            
            PlayerAttackAnimator.SetTrigger("Attack");

            PlayerAttackAnimator.Play("Attack", 0, 0);
            Debug.Log("SwordAttack!!");
            return true;
        }
        return false;
    }

    IEnumerator AttackColliderTime()
    {
        float time = 0;
        int AttackNum = DataManager.Instance._WeaponNumberSword;
        while (time <= _AttackGraceTime[AttackNum])//猶予時間
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
                PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed[AttackNum]);

                yield break;
            }
            else if (PlayerAvoidance())//回避
            {
                break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        PlayerAttackAnimator.SetFloat("AttackSpeed", _AttackSpeed[AttackNum]);

        _AttackType = 0;
        PlayerAttackAnimator.SetFloat("AttackType", _AttackType);
        yield break;
    }

    IEnumerator AvoidReLoad_IE()
    {
        yield return new WaitForSeconds(AvoidReLoadTime);
        _IsAvoid = false;

        yield break;


    }

}


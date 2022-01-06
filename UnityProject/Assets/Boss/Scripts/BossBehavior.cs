using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Move, StandBy, AttackPlayer, Charge, AttackCastle, Return };

public class BossBehavior : MonoBehaviour, IPlayerDamege
{
    [Header("�����l")]
    public int _Life;
    [Header("�ړ����x")]
    public float _MoveSpeed;
    [Header("���f��ړ����x")]
    public float _BreakSpeed;
    [Header("�_���[�W")]
    public int _Damage;
    [Header("�U���͈�")]
    public float _Area;
    [Header("�`���[�W")]
    public float _ChargeTime;
    [Header("�`���[�W���f��")]
    public int _StopChargeCount;
    [Header("��")]
    public GameObject _Castle;
    [Header("�v���C���[")]
    public GameObject _Player;
    [Header("�U���G���A")]
    public GameObject _AttackArea;
    [Header("���[�U�[")]
    public GameObject _Laser;

    Animator anim;
    [SerializeField]
    State state;            //�X�e�[�^�X
    Vector3 startPoint;     //�����ʒu
    int hitCount;           //�`���[�W��Ԃ̉����񐔌v�Z
    float castleDistance;   //��Ƃ̋���
    float chargeTimeCount;  //�`���[�W���Ԍv�Z
    bool isWait;            //��ԑJ�ڑ҂�
    bool isAttackPlayer;    //�v���C���[�ɍU��
    bool isMove;            //�ړ�(Anim)
    bool isCharge;          //�`���[�W��ԁiAnim�j

    void Start()
    {
        state = State.Move;
        startPoint = transform.position;
        anim = GetComponent<Animator>();
        isMove = false;
        isWait = false;
        isAttackPlayer = false;
        isCharge = false;
    }

    void Update()
    {
        castleDistance = Vector3.Distance(transform.position, _Castle.transform.position);

        SetState();
        SetAnim();
    }

    void SetState()
    {
        switch (state)
        {
            case State.Move:
                {
                    isMove = true;
                    if (!isWait)
                        transform.position += new Vector3(-_MoveSpeed * Time.deltaTime, 0, 0);

                    //��ԑJ��
                    if (castleDistance <= 40 && !isAttackPlayer)
                    {
                        //�U���\�����
                        state = State.StandBy;
                        //�U���͈͐���
                        for (int i = 0; i < 3; ++i)
                        {
                            GameObject temp = Instantiate(_AttackArea, new Vector3(_Player.transform.position.x, _AttackArea.transform.position.y, _Player.transform.position.z + (-_Area + (_Area + 1) * i)), Quaternion.identity);
                            temp.GetComponent<SearchArea>()._Area = _Area;
                            temp.GetComponent<SearchArea>()._Damage = _Damage;
                            temp.GetComponent<SearchArea>()._SetTime = 1.5f;
                        }
                        StartCoroutine(WaitTime(1.5f));
                    }
                    else if (castleDistance <= 25)
                    {
                        state = State.Charge;
                    }
                    break;
                }
            case State.StandBy:
                {
                    isMove = false;

                    //��ԑJ��
                    if (!isWait)
                    {
                        anim.SetTrigger("Attack");
                        state = State.AttackPlayer;
                        isAttackPlayer = true;
                        StartCoroutine(WaitTime(4.0f));
                    }
                    break;
                }
            case State.AttackPlayer:
                {
                    if (!isWait)
                        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                            state = State.Move;
                    break;
                }
            case State.Charge:
                {
                    isMove = false;
                    isCharge = true;
                    chargeTimeCount += Time.deltaTime;

                    //�`���[�W����
                    if (chargeTimeCount >= _ChargeTime)
                    {
                        state = State.AttackCastle;
                        _Castle.GetComponent<CastleBehavior>()._AddDamage(_Damage);
                        isCharge = false;
                        anim.SetTrigger("Attack");
                        _Laser.SetActive(true);
                        StartCoroutine(WaitTime(0.5f));
                    }
                    //�`���[�W���f
                    if (hitCount >= _StopChargeCount)
                    {
                        state = State.Return;
                        transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        isCharge = false;
                        StartCoroutine(WaitTime(4.0f));
                    }
                    break;
                }
            case State.AttackCastle:
                {
                    //�J�n�n�_�ɖ߂�
                    if (!isWait)
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), 20 * Time.deltaTime);

                    if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
                    {
                        //��ԃ��Z�b�g
                        StateReset();
                        StartCoroutine(WaitTime(4.0f));
                    }
                    break;
                }
            case State.Return:
                {
                    //�J�n�n�_�ɖ߂�
                    if (!isWait)
                    {
                        isMove = true;
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPoint.x, transform.position.y, transform.position.z), _BreakSpeed * Time.deltaTime);
                    }

                    if (Mathf.Abs(transform.position.x - startPoint.x) <= 0.5)
                    {
                        //��ԃ��Z�b�g
                        StateReset();
                        StartCoroutine(WaitTime(4.0f));
                        transform.rotation = Quaternion.AngleAxis(270, Vector3.up);
                    }
                    break;
                }
        }
    }

    void SetAnim()
    {
        anim.SetBool("Move", isMove);
        anim.SetBool("Charge", isCharge);
    }

    void StateReset()
    {
        _Laser.SetActive(false);
        state = State.Move;
        hitCount = 0;
        chargeTimeCount = 0;
        isAttackPlayer = false;
        isWait = false;
        isCharge = false;
    }

    //���̏�Ԃ��ڍs�܂ł̑҂�����
    IEnumerator WaitTime(float time)
    {
        isWait = true;
        yield return new WaitForSeconds(time);
        isWait = false;
    }

    //�_���[�W���󂯂�
    public void _AddDamege(int _Damage)
    {
        _Life--;
        hitCount++;
        chargeTimeCount -= 1.0f;
        if (_Life <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player Attack Area")
        {
            _AddDamege(1);
        }
    }
}

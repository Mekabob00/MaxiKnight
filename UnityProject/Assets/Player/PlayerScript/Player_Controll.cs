//�X�V�� 12��10�� �S��:����a�n
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
    private float applySpeed = 0.2f;       // ��]�̓K�p���x

    [SerializeField]
    public int PlayerAttackPoint;


    private Vector3 latestPosition;
    private Animator PlayerAttackAnimator;

    //-----------------------------------------------
    //�U���R���C�_�[�ǉ��i�W���j
    [SerializeField]private Collider attackArea;
    //-----------------------------------------------

    private void Awake()
    {//�X�^�[�g�֐��O�ɉ������������鎞�p
    }
    void Start()
    {
        PlayerAttackAnimator = GetComponent<Animator>();
        latestPosition = transform.position;
        PlayerAttackPoint = 10;
    }
    void Update()
    {
        PlayerWalk();//�v���C���[�ړ��֐��Ăяo��
        PlayerAttackAnimation();//Z�L�[�����������ɃA�j���[�V������������


        //-----------------------------------------------
        //�v���C���[���S�i�e�X�g�p�j
        if (Input.GetKeyDown(KeyCode.K))
        {
            GlobalData.Instance.isPlayerDead = true;
            GlobalData.Instance.isPlayerInSecondLine = false;
            Destroy(gameObject);
        }
        //-----------------------------------------------
    }

    



    private void PlayerWalk()
    {//�v���C���[�̈ړ��֐�
        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * PlayerWlakSpeed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * PlayerWlakSpeed;

        transform.position = new Vector3(
            transform.position.x + dx, 0.5f, transform.position.z + dz
        );
        Vector3 diff = transform.position - latestPosition;   //���݋���ʒu����ǂ̕����ɐi�񂾂�
        latestPosition = transform.position;
        diff.y = 0;//y�����͖�������
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //�v���C���[�̌����ύX
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
            //�U���A�j���[�V�����Đ�
            PlayerAttackAnimator.SetTrigger("Attack");
        }
    }
    public void _AddDamege(int _Damege)
    {//�g��Ȃ�����
    }

    //---------------------------------------------------------------
    //�ǉ� AnimationEvent �U���R���C�_�[�̗L�����Ɩ������i�W���j
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


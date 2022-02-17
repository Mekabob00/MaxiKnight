using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	enum STATE { NORMAL, STAGECLEAR, GAMEOVER };
	STATE m_state;

	public Transform _GameOverViewPos; //���_
	public Transform _Castle;
	public Transform _StageClearViewPos; //�{�X
	public Transform _Boss;

	private Vector3 m_camTagPos;
	// Use this for initialization
	void Start()
	{
		m_state = STATE.NORMAL;
	}

	// Update is called once per frame
	void Update()
	{
		SwitchState();

	}

	void SwitchState()
    {
        switch (m_state)
        {
			case STATE.NORMAL:
				if (GlobalData.Instance.isGameOver)
					m_state = STATE.GAMEOVER;
				if (GlobalData.Instance.isStageClear)
					m_state = STATE.STAGECLEAR;
				break;
			case STATE.STAGECLEAR:
				UpdateCameraRotation(_Boss);
				UpdateCameraPosition(_StageClearViewPos);
				break;
			case STATE.GAMEOVER:
				UpdateCameraRotation(_Castle);
				UpdateCameraPosition(_GameOverViewPos);
				break;
        }
    }

	//�X�V�Ƒ��@�����C���������߉�
	void UpdateCameraRotation(Transform target_)
	{
		//
		Vector3 vec = target_.position - transform.position;
		if (Vector3.Angle(vec, transform.forward) > 0.1f)
        {
			Quaternion rotate = Quaternion.LookRotation(vec);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, 0.05f);
        }
	}

	//�ɓ����ڕW�y
	void UpdateCameraPosition(Transform target_)
	{
		transform.position = Vector3.Slerp(transform.position, target_.position, 0.05f);
	}
}

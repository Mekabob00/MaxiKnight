using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	enum STATE { NORMAL, STAGECLEAR, GAMEOVER };
	STATE m_state;

	public Transform _GameOverViewPos; //拠点
	public Transform _Castle;
	public Transform _StageClearViewPos; //ボス
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
				UpdateCameraRotation(_Boss.position + new Vector3(0, 1, 0));
				UpdateCameraPosition(_StageClearViewPos);
				break;
			case STATE.GAMEOVER:
				UpdateCameraRotation(_Castle.position + new Vector3(0, 4, 0));
				UpdateCameraPosition(_GameOverViewPos);
				break;
        }
    }

	//カメラの向き調整
	void UpdateCameraRotation(Vector3 target_)
	{
		Vector3 vec = target_ - transform.position;
		if (Vector3.Angle(vec, transform.forward) > 0.1f)
        {
			Quaternion rotate = Quaternion.LookRotation(vec);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, 0.03f);
        }
	}

	//目標地まで移動
	void UpdateCameraPosition(Transform target_)
	{
		if(Vector3.Distance(transform.position, target_.position) > 0.5f)
			transform.position = Vector3.Slerp(transform.position, target_.position, 0.03f);
	}
}

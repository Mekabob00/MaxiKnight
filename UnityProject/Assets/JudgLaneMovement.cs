using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgLaneMovement : MonoBehaviour
{
    [SerializeField, Tooltip("���݂̃��[��")]
    private int _NowLane = 0;
    public int GetNowLane()
    {
        return _NowLane;
    }

    public void SetNowLane(int SetNum)
    {
        _NowLane = SetNum;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public Animator _MapUIAnimator;
    public Animator _HPUIAnimator;

    enum UISTATE { OPEN, CLOSE, NON };
    [SerializeField]UISTATE m_uistate;

    void Start()
    {
        m_uistate = UISTATE.CLOSE;
    }

    public void OnClick()
    {
        Debug.Log("Pressed");

        if (_MapUIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _HPUIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            m_uistate = UISTATE.CLOSE;
        else if (_MapUIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opend") && _HPUIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opend"))
            m_uistate = UISTATE.OPEN;
        else
            m_uistate = UISTATE.NON;

        switch (m_uistate)
        {
            case UISTATE.CLOSE:
                _HPUIAnimator.SetTrigger("Open");
                _MapUIAnimator.SetTrigger("Open");
                break;
            case UISTATE.OPEN:
                _HPUIAnimator.SetTrigger("Close");
                _MapUIAnimator.SetTrigger("Close");
                break;
        }
    }
}

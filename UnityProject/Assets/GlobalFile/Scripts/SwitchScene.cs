using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public static bool _ToShopScene;
    public static bool _ToTitleScene;
    public static bool _ToClearScene;

    Animator m_animator;
    void Start()
    {
        _ToShopScene = false;
        _ToTitleScene = false;
        _ToClearScene = false;
        m_animator = GetComponent<Animator>();
    }

    public void ChangeScene()
    {
        if (_ToShopScene)
            ChangeSceneToShop();
        if (_ToTitleScene)
            ChangeSceneToTitle();
        if (_ToClearScene)
            ChangeScceneToClear();
    }

    void ChangeSceneToTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void ChangeSceneToStage()
    {
        SceneManager.LoadScene("Stage0");
    }
    void ChangeSceneToShop()
    {
        SceneManager.LoadScene("Shop");
    }
    void ChangeScceneToClear()
    {
        SceneManager.LoadScene("Clear0");
    }
}

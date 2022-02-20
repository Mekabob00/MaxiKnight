using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SwitchScene
{
    public static void ChangeSceneToTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public static void ChangeSceneToStage()
    {
        SceneManager.LoadScene("Stage0");
    }
    public static void ChangeSceneToShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public static void ChangeScceneToClear()
    {
        SceneManager.LoadScene("Clear0");
    }
}

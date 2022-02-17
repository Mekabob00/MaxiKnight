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
        SceneManager.LoadScene("Stage");
    }
    public static void ChangeSceneToShop()
    {
        SceneManager.LoadScene("Shop");
    }
}

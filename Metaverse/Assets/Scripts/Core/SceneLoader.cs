using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static string LastHubScene = "Hub";

    public static void LoadMini(string sceneName)
    {
        LastHubScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public static void BackToHub()
    {
        SceneManager.LoadScene(LastHubScene);
    }
}

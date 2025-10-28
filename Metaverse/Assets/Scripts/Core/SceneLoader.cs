using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // 허브 씬 이름을 저장하는 변수
    public static string LastHubScene = "Hub"; // 허브 기본 이름 (네 실제 허브씬 이름으로 바꿔도 됨)

    /// 허브에서 미니게임 씬으로 이동
    public static void LoadMini(string sceneName)
    {
        // 현재 씬 이름 저장 (허브 씬이라고 가정)
        LastHubScene = SceneManager.GetActiveScene().name;

        // 미니게임 씬 로드
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// 허브 씬 이름 직접 지정해서 이동
    public static void LoadHub(string sceneName)
    {
        LastHubScene = sceneName;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// 마지막 허브 씬으로 복귀
    public static void BackToHub()
    {
        SceneManager.LoadScene(LastHubScene, LoadSceneMode.Single);
    }
}

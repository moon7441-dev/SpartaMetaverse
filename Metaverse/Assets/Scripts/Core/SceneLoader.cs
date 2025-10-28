using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // ��� �� �̸��� �����ϴ� ����
    public static string LastHubScene = "Hub"; // ��� �⺻ �̸� (�� ���� ���� �̸����� �ٲ㵵 ��)

    /// ��꿡�� �̴ϰ��� ������ �̵�
    public static void LoadMini(string sceneName)
    {
        // ���� �� �̸� ���� (��� ���̶�� ����)
        LastHubScene = SceneManager.GetActiveScene().name;

        // �̴ϰ��� �� �ε�
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// ��� �� �̸� ���� �����ؼ� �̵�
    public static void LoadHub(string sceneName)
    {
        LastHubScene = sceneName;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// ������ ��� ������ ����
    public static void BackToHub()
    {
        SceneManager.LoadScene(LastHubScene, LoadSceneMode.Single);
    }
}

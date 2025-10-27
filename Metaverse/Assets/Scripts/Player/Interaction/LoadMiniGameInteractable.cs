using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMiniGameInteractable : Interactable
{
    public string miniSceneName = "Mini_Stack";  //stack ����

    public override void Interact()
    {
        Debug.Log($"�̴ϰ��� �� �ε� �õ�: {miniSceneName}");
        SceneLoader.LoadMini(miniSceneName);
    }
}

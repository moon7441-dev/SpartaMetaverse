using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMiniGameInteractable : Interactable
{
    public string miniSceneName = "Mini_Stack";  //stack 전용

    public override void Interact()
    {
        Debug.Log($"미니게임 씬 로드 시도: {miniSceneName}");
        SceneLoader.LoadMini(miniSceneName);
    }
}

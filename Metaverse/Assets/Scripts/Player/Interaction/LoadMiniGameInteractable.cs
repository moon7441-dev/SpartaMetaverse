using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMiniGameInteractable : Interactable
{
    public string miniSceneName = "Mini_stack";  //stack ����

    public override void Interact()
    {
        SceneLoader.LoadMini(miniSceneName);
    }
}

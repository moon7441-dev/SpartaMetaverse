using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStackPortal : MonoBehaviour
{
    public string miniSceneName = "Mini_Stack";

    void OnMouseDown()
    {
        // 카메라에 Physics2D Raycast가 닿으려면
        // 이 오브젝트에 Collider2D가 붙어 있어야 함
        SceneLoader.LoadMini(miniSceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStackPortal : MonoBehaviour
{
    public string miniSceneName = "Mini_Stack";

    void OnMouseDown()
    {
        // ī�޶� Physics2D Raycast�� ��������
        // �� ������Ʈ�� Collider2D�� �پ� �־�� ��
        SceneLoader.LoadMini(miniSceneName);
    }
}

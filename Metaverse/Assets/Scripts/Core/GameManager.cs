using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}

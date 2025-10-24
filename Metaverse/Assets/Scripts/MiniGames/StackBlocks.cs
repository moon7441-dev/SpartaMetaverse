using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBlocks : MonoBehaviour
{
    public float width = 1f;
    public Transform visual;

    public void SetWidth(float w)
    {
        width = Mathf.Max(0.1f, w);
        if (visual)
        {
            visual.localScale = new Vector3(width, visual.localScale.y, visual.localScale.z);
        }
    }
}

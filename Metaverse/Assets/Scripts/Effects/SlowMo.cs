using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    public static SlowMo I;
    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
    }
    public void Pulse(float scale = 0.7f, float duration = 0.1f)
    {
        StartCoroutine(Co(scale, duration));
    }
    IEnumerator Co(float scale,  float duration)
    {
        Time.timeScale = Mathf.Clamp(scale, 0.05f, 1f);
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
    }
}

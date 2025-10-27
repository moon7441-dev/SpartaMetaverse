using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScale : MonoBehaviour
{
    public void Play(float punch = 0.15f, float time = 0.12f)
    {
        StopAllCoroutines();
        StartCoroutine(Co(punch,time));

        IEnumerator Co(float punch, float time)
        {
            Vector3 b = transform.localScale, t = b*(1f+punch);
            float x = 0f;
            while (x < time)
            {
                x += Time.unscaledDeltaTime;
                float k = x / time;
                float e = 1f - Mathf.Pow(1f - k, 2f);
                transform.localScale = Vector3.Lerp(b, t, e);
                yield return null;
            }
            x = 0f;
            while (x < time)
            {
                x += Time.unscaledDeltaTime;
                float k = x/ time;
                float e = 1f - Mathf.Pow(1f - k, 2f);
                transform.localScale = Vector3.Lerp(t, b, e);
                yield return null;
            }
            transform.localScale = b;
        }
    }
}

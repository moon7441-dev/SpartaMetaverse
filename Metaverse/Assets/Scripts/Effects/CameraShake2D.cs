using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake2D : MonoBehaviour
{
    public static CameraShake2D I;
    [Range(0f, 2f)]
    public float defaultDuration = 0.12f;
    [Range(0f, 2f)]
    public float defaultIntensity = 0.25f;

    Vector3 _origin;
    Coroutine _co;

    private void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
        _origin = transform.localPosition;
    }

    public void Shake(float intensity = -1f, float duration = -1f)
    {
        if (intensity < 0) intensity = defaultIntensity;
        if (duration < 0) duration = defaultDuration;
        if ( _co != null ) StopCoroutine( _co );
        _co = StartCoroutine(CoShake(intensity, duration));
    }

    IEnumerator CoShake(float intensity, float duration)
    {
        float t = 0f;
        while (t < duration) 
        {
            t += Time.unscaledDeltaTime;
            Vector2 r = Random.insideUnitCircle * intensity;
            transform.localPosition = _origin + new Vector3(r.x, r.y, 0f);
            yield return null;
        }
        transform.localPosition =_origin;
        _co = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndFade2D : MonoBehaviour
{
    public float gravity = -9f;
    public float rotSpeed = 180f;
    public float life = 0.8f;
    SpriteRenderer _sr;
    Vector2 _vel;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Launch(Vector2 v)
    {
        _vel = v;
        StartCoroutine(Co());
    }
    IEnumerator Co()
    {
        float t = 0f;
        Color c = _sr.color;
        while (t < life)
        {
            t += Time.deltaTime;
            _vel.y += gravity * Time.deltaTime;
            transform.Translate(_vel*Time.deltaTime);
            transform.Rotate(0,0,rotSpeed*Time.deltaTime);
            float a = Mathf.Lerp(1f,0f,t/life);
            _sr.color = new Color(c.r,c.g,c.b,a);
            yield return null;
        }
        Destroy(gameObject);
    }
}

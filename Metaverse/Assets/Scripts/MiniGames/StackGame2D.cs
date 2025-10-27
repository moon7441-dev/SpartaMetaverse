using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGame2D : MonoBehaviour
{
    public GameObject baseBlock; // 시작/기준 블록
    public GameObject movingPrefab; // 이동 블록 프리팹 (SpriteRenderer + BoxCollider2D)
    public GameObject blockPiecePrefab; // 잘려나간 조각 프리팹 (SpriteRenderer + FallAndFade2D)
    public Transform effectsRoot; // 조각 정리용 부모


    public float moveSpeed = 2.6f;
    public float laneLimit = 3.2f; // 좌우 왕복 한계


    GameObject current; float dir = 1f; float lastWidth = 3f;


    void Start() { if (CameraShake2D.I == null) Camera.main.gameObject.AddComponent<CameraShake2D>(); if (SlowMo.I == null) new GameObject("_SlowMo").AddComponent<SlowMo>(); SpawnNext(); }


    void Update()
    {
        if (!current) return;
        current.transform.Translate(Vector2.right * dir * moveSpeed * Time.deltaTime);
        if (Mathf.Abs(current.transform.position.x) > laneLimit) dir *= -1f;
        if (Input.GetKeyDown(KeyCode.Space)) Place();
    }


    void Place()
    {
        Transform prev = baseBlock.transform; float dx = current.transform.position.x - prev.position.x; float remain = lastWidth - Mathf.Abs(dx);
        if (remain <= 0f)
        {
            Impact(0.6f); // TODO: GameOver 처리
            return;
        }


        float cut = Mathf.Abs(dx);
        if (cut > 0.001f) SpawnCutPiece(dx, cut);


        // 겹친 영역 중심/폭 재설정
        float cx = (current.transform.position.x + prev.position.x) * 0.5f;
        current.transform.position = new Vector3(cx, current.transform.position.y, 0f);
        current.transform.localScale = new Vector3(remain, current.transform.localScale.y, 1f);


        // 타격감
        float mag = Mathf.InverseLerp(0f, lastWidth, remain); // 0~1
        Impact(0.18f + 0.22f * mag);
        var ps = current.GetComponent<PunchScale>(); if (ps) ps.Play(0.12f + 0.08f * mag, 0.12f);


        lastWidth = remain; baseBlock = current; SpawnNext();
    }


    void SpawnNext()
    {
        Vector3 pos = baseBlock.transform.position + Vector3.up * 0.6f;
        current = Instantiate(movingPrefab, pos, Quaternion.identity);
        current.transform.localScale = new Vector3(lastWidth, current.transform.localScale.y, 1f);
        if (!current.GetComponent<PunchScale>()) current.AddComponent<PunchScale>();
        dir = Random.value > 0.5f ? 1f : -1f;
    }


    void SpawnCutPiece(float dx, float cutWidth)
    {
        float sign = Mathf.Sign(dx);
        float pieceCenterX = current.transform.position.x + (lastWidth * 0.5f * sign);
        var go = Instantiate(blockPiecePrefab, new Vector3(pieceCenterX, current.transform.position.y, 0f), Quaternion.identity, effectsRoot);
        go.transform.localScale = new Vector3(cutWidth, current.transform.localScale.y, 1f);
        var src = current.GetComponent<SpriteRenderer>(); var dst = go.GetComponent<SpriteRenderer>(); if (src && dst) dst.color = src.color;
        var fall = go.GetComponent<FallAndFade2D>(); if (fall) fall.Launch(new Vector2(0.6f * sign, 1.2f));
    }


    void Impact(float magnitude)
    {
        if (CameraShake2D.I) CameraShake2D.I.Shake(magnitude, 0.1f);
        if (SlowMo.I) SlowMo.I.Pulse(0.75f, 0.08f + magnitude * 0.05f);
    }
}

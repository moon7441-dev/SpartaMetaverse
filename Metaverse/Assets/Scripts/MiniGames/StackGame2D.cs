using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGame2D : MonoBehaviour
{
    public GameObject baseBlock; // 시작/기준 블록
    public GameObject movingPrefab; // 이동 블록 프리팹 (SpriteRenderer + BoxCollider2D)
    public GameObject blockPiecePrefab; // 잘려나간 조각 프리팹 (SpriteRenderer + FallAndFade2D)
    public Transform effectsRoot; // 조각 정리용 부모

    public GameObject backgroundTilePrefab;  // BackgroundTile 프리팹
    public float backgroundTileHeight = 12f; // 프리팹 한 장의 월드 높이
    List<GameObject> spawnedBackgrounds = new List<GameObject>();

    public float moveSpeed = 2.6f;
    public float laneLimit = 3.2f; // 좌우 왕복 한계
    public float stackHeightStep = 0.6f; // 한 층 쌓일 때마다 올라가는 Y 간격

    [Header("Camera Follow")]
    public Camera mainCam;
    public float camFollowLerp = 8f;
    public float camYOffset = 2f;

    GameObject current; 
    float dir = 1f; 
    float lastWidth;
    bool isGameOver = false;


    void Start()
    {
        if (!mainCam) mainCam = Camera.main;

        // 효과 매니저 보장
        if (CameraShake2D.I == null)
            mainCam.gameObject.AddComponent<CameraShake2D>();
        if (SlowMo.I == null)
            new GameObject("_SlowMo").AddComponent<SlowMo>();

        // 시작 블록 크기를 기준으로 폭 세팅
        lastWidth = baseBlock.transform.localScale.x;

        // 첫 움직이는 블록 소환
        SpawnNext();


    }

    void EnsureBackgroundCoverage()
    {
        if (!mainCam) return;

        // 카메라 현재 y의 위쪽까지 덮어야 하는 최소 y
        float camTopY = mainCam.transform.position.y + mainCam.orthographicSize;

        // 지금까지 생성한 배경들 중 가장 위에 있는 y를 구한다
        float highestBgY = float.NegativeInfinity;
        foreach (var bg in spawnedBackgrounds)
        {
            if (bg == null) continue;
            float y = bg.transform.position.y;
            if (y > highestBgY) highestBgY = y;
        }

        // 만약 아무 배경도 없으면, 첫 장은 BaseBlock 근처에 깔자
        if (spawnedBackgrounds.Count == 0)
        {
            var first = Instantiate(backgroundTilePrefab,
                new Vector3(0f, 0f, 10f), // z=10f: 카메라 뒤쪽 (스프라이트의 Order in Layer는 낮게)
                Quaternion.identity);
            spawnedBackgrounds.Add(first);
            highestBgY = first.transform.position.y;
        }

        // camTopY가 highestBgY + (backgroundTileHeight * 0.5f)보다 높으면
        // = 카메라가 이미 거의 맨 위 배경보다 위로 갔다 → 새로운 배경 타일을 위에 더 깐다
        while (camTopY > highestBgY + backgroundTileHeight * 0.5f)
        {
            // 위로 한 장 더
            var next = Instantiate(backgroundTilePrefab,
                new Vector3(0f,highestBgY + backgroundTileHeight,10f ),
                Quaternion.identity);

            spawnedBackgrounds.Add(next);
            highestBgY = next.transform.position.y;
        }
    }

    void Update()
    {
        if (isGameOver) return;
        if (!current) return;

        // 좌우 왕복 이동
        current.transform.position += Vector3.right * dir * moveSpeed * Time.deltaTime;
        if (Mathf.Abs(current.transform.position.x) > laneLimit)
        {
            dir *= -1f;
        }

        // 스페이스로 고정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Place();
        }

        // 카메라 따라가기 (마지막 쌓인 블록 기준)
        FollowCamera();
        EnsureBackgroundCoverage();
    }

    void Place()
    {
        // 이전 블록(기준)과의 차이
        Transform prev = baseBlock.transform;

        float dx = current.transform.position.x - prev.position.x;
        float overlap = lastWidth - Mathf.Abs(dx); // 남길 폭
        if (overlap <= 0f)
        {
            // 쌓을 데가 없으면 게임오버
            Impact(0.6f);
            isGameOver = true;
            return;
        }

        // ------------------------------
        // 1) 잘려나가는 조각 떼서 떨어뜨리기
        // ------------------------------
        float cutSize = Mathf.Abs(dx);
        if (cutSize > 0.001f)
        {
            SpawnCutPiece(dx, cutSize, prev, current, lastWidth);
        }

        // ------------------------------
        // 2) current 블록 "정제": 겹친 영역만 남기기
        // ------------------------------
        // 새로운 블록의 중앙은 prev와 current의 중앙 사이
        float newCenterX = (prev.position.x + current.transform.position.x) * 0.5f;

        current.transform.position = new Vector3(
            newCenterX,
            current.transform.position.y,
            0f
        );

        current.transform.localScale = new Vector3(
            overlap,
            current.transform.localScale.y,
            1f
        );

        // ------------------------------
        // 3) 연출 (카메라 흔들림, 살짝 커졌다가 원복 등)
        // ------------------------------
        float quality = Mathf.InverseLerp(0f, lastWidth, overlap); // 얼마나 잘 맞췄는지 0~1
        Impact(0.18f + 0.22f * quality);

        var ps = current.GetComponent<PunchScale>();
        if (ps) ps.Play(0.15f, 0.12f);

        // ------------------------------
        // 4) 상태 업데이트:
        //    방금 놓은 current가 이제 새로운 baseBlock이 됨
        // ------------------------------
        lastWidth = overlap;
        baseBlock = current;

        // ------------------------------
        // 5) 새로운 current 하나 위에 또 소환
        // ------------------------------
        SpawnNext();
    }

    void SpawnNext()
    {
        // 새 블록이 쌓이는 위치 = 마지막 블록(baseBlock) 위로 일정 높이
        Vector3 spawnPos = baseBlock.transform.position + new Vector3(0f, stackHeightStep, 0f);

        current = Instantiate(movingPrefab, spawnPos, Quaternion.identity);

        // 새 블록의 폭 = 방금 쌓인 블록 폭과 동일
        current.transform.localScale = new Vector3(
            lastWidth,
            baseBlock.transform.localScale.y,
            1f
        );

        // 이펙트용 컴포넌트 보장
        if (!current.GetComponent<PunchScale>())
            current.AddComponent<PunchScale>();

        // 새 블록은 다시 좌우로 움직이기 시작
        dir = (Random.value > 0.5f) ? 1f : -1f;
    }

    void SpawnCutPiece(float dx, float cutSize, Transform prev, GameObject cur, float prevWidth)
    {
        // dx의 방향(왼쪽에서 삐져나갔나 오른쪽에서 삐져나갔나)
        float side = Mathf.Sign(dx);

        // 잘려 나가는 조각의 중심 위치 계산
        // prev 블록의 가장자리 방향 쪽으로 붙어서 떨어지도록 설정해보자
        float pieceCenterX = baseBlock.transform.position.x
                             + (prevWidth * 0.5f * side);

        // 조각 생성
        var piece = Instantiate(
            blockPiecePrefab,
            new Vector3(pieceCenterX, cur.transform.position.y, 0f),
            Quaternion.identity,
            effectsRoot
        );

        piece.transform.localScale = new Vector3(
            cutSize,
            cur.transform.localScale.y,
            1f
        );

        // 색 맞추기 (떨어지는 조각도 같은 색)
        var src = cur.GetComponent<SpriteRenderer>();
        var dst = piece.GetComponent<SpriteRenderer>();
        if (src && dst) dst.color = src.color;

        // 떨어지는 애니메이션
        var fall = piece.GetComponent<FallAndFade2D>();
        if (fall)
        {
            // 약간 바깥쪽 + 위로 톡 튀고 떨어지게
            fall.Launch(new Vector2(0.6f * side, 1.2f));
        }
    }

    void Impact(float mag)
    {
        if (CameraShake2D.I) CameraShake2D.I.Shake(mag, 0.1f);
        if (SlowMo.I) SlowMo.I.Pulse(0.8f, 0.08f + mag * 0.05f);
    }

    void FollowCamera()
    {
        if (!mainCam) return;

        // 기준은 지금까지 쌓인 마지막 블록 (baseBlock)
        Vector3 targetPos = baseBlock.transform.position;
        targetPos += new Vector3(0f, camYOffset, -10f); // 카메라는 z=-10 유지

        Vector3 camPos = mainCam.transform.position;
        mainCam.transform.position = Vector3.Lerp(
            camPos,
            targetPos,
            Time.unscaledDeltaTime * camFollowLerp
        );
    }
}
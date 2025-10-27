using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGame2D : MonoBehaviour
{
    public GameObject baseBlock; // ����/���� ���
    public GameObject movingPrefab; // �̵� ��� ������ (SpriteRenderer + BoxCollider2D)
    public GameObject blockPiecePrefab; // �߷����� ���� ������ (SpriteRenderer + FallAndFade2D)
    public Transform effectsRoot; // ���� ������ �θ�

    public GameObject backgroundTilePrefab;  // BackgroundTile ������
    public float backgroundTileHeight = 12f; // ������ �� ���� ���� ����
    List<GameObject> spawnedBackgrounds = new List<GameObject>();

    public float moveSpeed = 2.6f;
    public float laneLimit = 3.2f; // �¿� �պ� �Ѱ�
    public float stackHeightStep = 0.6f; // �� �� ���� ������ �ö󰡴� Y ����

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

        // ȿ�� �Ŵ��� ����
        if (CameraShake2D.I == null)
            mainCam.gameObject.AddComponent<CameraShake2D>();
        if (SlowMo.I == null)
            new GameObject("_SlowMo").AddComponent<SlowMo>();

        // ���� ��� ũ�⸦ �������� �� ����
        lastWidth = baseBlock.transform.localScale.x;

        // ù �����̴� ��� ��ȯ
        SpawnNext();


    }

    void EnsureBackgroundCoverage()
    {
        if (!mainCam) return;

        // ī�޶� ���� y�� ���ʱ��� ����� �ϴ� �ּ� y
        float camTopY = mainCam.transform.position.y + mainCam.orthographicSize;

        // ���ݱ��� ������ ���� �� ���� ���� �ִ� y�� ���Ѵ�
        float highestBgY = float.NegativeInfinity;
        foreach (var bg in spawnedBackgrounds)
        {
            if (bg == null) continue;
            float y = bg.transform.position.y;
            if (y > highestBgY) highestBgY = y;
        }

        // ���� �ƹ� ��浵 ������, ù ���� BaseBlock ��ó�� ����
        if (spawnedBackgrounds.Count == 0)
        {
            var first = Instantiate(backgroundTilePrefab,
                new Vector3(0f, 0f, 10f), // z=10f: ī�޶� ���� (��������Ʈ�� Order in Layer�� ����)
                Quaternion.identity);
            spawnedBackgrounds.Add(first);
            highestBgY = first.transform.position.y;
        }

        // camTopY�� highestBgY + (backgroundTileHeight * 0.5f)���� ������
        // = ī�޶� �̹� ���� �� �� ��溸�� ���� ���� �� ���ο� ��� Ÿ���� ���� �� ���
        while (camTopY > highestBgY + backgroundTileHeight * 0.5f)
        {
            // ���� �� �� ��
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

        // �¿� �պ� �̵�
        current.transform.position += Vector3.right * dir * moveSpeed * Time.deltaTime;
        if (Mathf.Abs(current.transform.position.x) > laneLimit)
        {
            dir *= -1f;
        }

        // �����̽��� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Place();
        }

        // ī�޶� ���󰡱� (������ ���� ��� ����)
        FollowCamera();
        EnsureBackgroundCoverage();
    }

    void Place()
    {
        // ���� ���(����)���� ����
        Transform prev = baseBlock.transform;

        float dx = current.transform.position.x - prev.position.x;
        float overlap = lastWidth - Mathf.Abs(dx); // ���� ��
        if (overlap <= 0f)
        {
            // ���� ���� ������ ���ӿ���
            Impact(0.6f);
            isGameOver = true;
            return;
        }

        // ------------------------------
        // 1) �߷������� ���� ���� ����߸���
        // ------------------------------
        float cutSize = Mathf.Abs(dx);
        if (cutSize > 0.001f)
        {
            SpawnCutPiece(dx, cutSize, prev, current, lastWidth);
        }

        // ------------------------------
        // 2) current ��� "����": ��ģ ������ �����
        // ------------------------------
        // ���ο� ����� �߾��� prev�� current�� �߾� ����
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
        // 3) ���� (ī�޶� ��鸲, ��¦ Ŀ���ٰ� ���� ��)
        // ------------------------------
        float quality = Mathf.InverseLerp(0f, lastWidth, overlap); // �󸶳� �� ������� 0~1
        Impact(0.18f + 0.22f * quality);

        var ps = current.GetComponent<PunchScale>();
        if (ps) ps.Play(0.15f, 0.12f);

        // ------------------------------
        // 4) ���� ������Ʈ:
        //    ��� ���� current�� ���� ���ο� baseBlock�� ��
        // ------------------------------
        lastWidth = overlap;
        baseBlock = current;

        // ------------------------------
        // 5) ���ο� current �ϳ� ���� �� ��ȯ
        // ------------------------------
        SpawnNext();
    }

    void SpawnNext()
    {
        // �� ����� ���̴� ��ġ = ������ ���(baseBlock) ���� ���� ����
        Vector3 spawnPos = baseBlock.transform.position + new Vector3(0f, stackHeightStep, 0f);

        current = Instantiate(movingPrefab, spawnPos, Quaternion.identity);

        // �� ����� �� = ��� ���� ��� ���� ����
        current.transform.localScale = new Vector3(
            lastWidth,
            baseBlock.transform.localScale.y,
            1f
        );

        // ����Ʈ�� ������Ʈ ����
        if (!current.GetComponent<PunchScale>())
            current.AddComponent<PunchScale>();

        // �� ����� �ٽ� �¿�� �����̱� ����
        dir = (Random.value > 0.5f) ? 1f : -1f;
    }

    void SpawnCutPiece(float dx, float cutSize, Transform prev, GameObject cur, float prevWidth)
    {
        // dx�� ����(���ʿ��� ���������� �����ʿ��� ����������)
        float side = Mathf.Sign(dx);

        // �߷� ������ ������ �߽� ��ġ ���
        // prev ����� �����ڸ� ���� ������ �پ ���������� �����غ���
        float pieceCenterX = baseBlock.transform.position.x
                             + (prevWidth * 0.5f * side);

        // ���� ����
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

        // �� ���߱� (�������� ������ ���� ��)
        var src = cur.GetComponent<SpriteRenderer>();
        var dst = piece.GetComponent<SpriteRenderer>();
        if (src && dst) dst.color = src.color;

        // �������� �ִϸ��̼�
        var fall = piece.GetComponent<FallAndFade2D>();
        if (fall)
        {
            // �ణ �ٱ��� + ���� �� Ƣ�� ��������
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

        // ������ ���ݱ��� ���� ������ ��� (baseBlock)
        Vector3 targetPos = baseBlock.transform.position;
        targetPos += new Vector3(0f, camYOffset, -10f); // ī�޶�� z=-10 ����

        Vector3 camPos = mainCam.transform.position;
        mainCam.transform.position = Vector3.Lerp(
            camPos,
            targetPos,
            Time.unscaledDeltaTime * camFollowLerp
        );
    }
}
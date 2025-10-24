using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target; //Player
    public float smooth = 5f;

    [Header("Bound")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if(!target) return;

        Vector3 desired = new Vector3(target.position.x, target.position.y, target.position.z);

        //카메라 뷰포트 크기 고려
        float vertExtent = _cam.orthographicSize;
        float horzExtent = vertExtent * _cam.aspect;

        float clampedX = Mathf.Clamp(desired.x, minBounds.x + horzExtent, maxBounds.x - horzExtent);
        float clampedY = Mathf.Clamp(desired.y, minBounds.y + horzExtent, maxBounds.y - vertExtent);

        Vector3 clamped = new Vector3(clampedX, clampedY, clamped.z);
        transform.position = Vector3.Lerp(transform.position, clamped, Time.deltaTime * smooth);
    }
}

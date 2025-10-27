using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    
    Rigidbody2D _rb;
    Vector2 _input;
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;  //탑다운이므로 중력X
        _rb.freezeRotation = true;
    }
    void Update()
    {
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
        _input = _input.normalized;

        _anim.SetFloat("Speed", _input.magnitude);

        if (_input.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(_input.x), 1, 1);
    }

    private void FixedUpdate()
    {
        _rb.velocity = _input * moveSpeed;
    }
}

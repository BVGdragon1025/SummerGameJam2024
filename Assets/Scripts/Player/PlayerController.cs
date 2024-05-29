using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    private Rigidbody _rb;
    [SerializeField]
    private Vector3 _movement;
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        _movement = new Vector3(hInput, 0, vInput).normalized;

        _animator.SetFloat("xValue", hInput);
        _animator.SetFloat("yValue", vInput);

        //Test SFX
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.testSFXEvent, transform.position);
        }

    }
    
    private void FixedUpdate()
    {
        MovePlayer(_movement, _playerSpeed);
    }


    void MovePlayer(Vector3 movementVector, float playerSpeed)
    {
        _rb.velocity = movementVector * playerSpeed;
    }



}

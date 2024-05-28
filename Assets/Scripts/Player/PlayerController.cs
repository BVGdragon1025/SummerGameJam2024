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
    [SerializeField]
    private GameObject _currentObjectToBuild;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        _movement = new Vector3(hInput, 0, vInput).normalized;

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

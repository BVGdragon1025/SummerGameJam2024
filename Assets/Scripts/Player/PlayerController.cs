using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingManager))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    private Rigidbody2D _rb;
    [SerializeField]
    private BuildingManager _buildingManager;
    private Vector2 _movement;
    [SerializeField]
    private GameObject _currentObjectToBuild;
    public Collider2D playerRange;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _buildingManager = GetComponent<BuildingManager>();
        playerRange = GetComponentInChildren<Collider2D>();
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        _movement = new Vector2(hInput, vInput).normalized;
        Debug.Log($"Velocity: { _movement }");

        //Test SFX
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.testSFXEvent, transform.position);
        }

        //_buildingManager.BuildStructure(_currentObjectToBuild);

    }
    
    private void FixedUpdate()
    {
        MovePlayer(_movement, _playerSpeed);
    }


    void MovePlayer(Vector2 movementVector, float playerSpeed)
    {
        _rb.velocity = movementVector * playerSpeed;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;
using FMOD.Studio;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    private Rigidbody _rb;
    public VisualEffect vfxRenderer;
    [SerializeField]
    private Vector3 _movement;
    private Animator _animator;
    [SerializeField]
    private StudioEventEmitter _interactionsEmitter;
    public StudioEventEmitter InteractionsEmiter { get { return _interactionsEmitter; } }

    private EventInstance _footstepsInstance;

    public float PlayerMovement { get { return _movement.magnitude; } }
  
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _footstepsInstance = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.playerFootsteps);
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        _movement = new Vector3(hInput, 0, vInput).normalized;

        _animator.SetFloat("xValue", hInput);
        _animator.SetFloat("yValue", vInput);

        UpdateSound();

        /*
        //Test SFX
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.testSFXEvent, transform.position);
        }
        */

        vfxRenderer.SetVector3("ColliderPos", new Vector3(transform.position.x, transform.position.z, transform.position.y));

    }
    
    private void FixedUpdate()
    {
        MovePlayer(_movement, _playerSpeed);
    }


    void MovePlayer(Vector3 movementVector, float playerSpeed)
    {
        _rb.velocity = movementVector * playerSpeed;
    }

    void UpdateSound()
    {
        if(_rb.velocity.magnitude != 0)
        {
            PLAYBACK_STATE playbackState;
            _footstepsInstance.getPlaybackState(out playbackState);

            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _footstepsInstance.start();
            }
        }
        else
        {
            _footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using TMPro;

[RequireComponent(typeof(StudioEventEmitter))]

public class HealingComponent : MonoBehaviour
{
    [Header("Helper values")]
    [SerializeField]
    private float _healingElementDelay;
    [SerializeField]
    private float _collectingDelay;
    [SerializeField]
    private float _plagueElementPenalty;
    [SerializeField]
    private float _plagueStructurePenalty;

    [Header("Interactions Section")]
    [SerializeField]
    private InteractionsEnum _interactions;
    [SerializeField]
    private TextMeshPro _collectText;
    [SerializeField]
    private TextMeshPro _healText;

    [Header("Object References")]
    [SerializeField]
    private List<GameObject> _structures;
    [SerializeField]
    private GameObject _element;
    private PlayerController _playerController;
    private Animator _animator;
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private Coroutine _healingElementCoroutine;

    private EventInstance _collectingInstance;

    private void Awake()
    {
        _interactions = InteractionsEnum.NotInteracting;
        _playerController = GetComponentInParent<PlayerController>();
        _animator = GetComponentInParent<Animator>();
        
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _gameManager = GameManager.Instance;
        _collectingInstance = _audioManager.CreateEventInstance(FMODEvents.Instance.collecting);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Element"))
        {
            _element = other.gameObject;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            _structures.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_structures.Count != 0)
        {
            for (int i = 0; i < _structures.Count; i++)
            {
                if (_structures[i] != null)
                {
                    Building building = _structures[i].GetComponent<Building>();
                    if (building.PlagueState == PlagueState.Infected)
                    {
                        building.ChangePlagueState(PlagueState.Healing);
                    }

                    if (building.HasFinished && building.PlagueState == PlagueState.Healthy)
                    {
                        _collectText.gameObject.SetActive(true);
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.F) && _interactions == InteractionsEnum.NotInteracting)
            {
                foreach (GameObject structure in _structures)
                {
                    if (structure != null && structure.CompareTag("Building"))
                    {
                        BuildingBasic building = structure.GetComponent<BuildingBasic>();

                        if (building.HasFinished && building.PlagueState == PlagueState.Healthy)
                        {
                            Debug.Log("Collecting resources");
                            _animator.SetBool("isCollecting", true);
                            StartCoroutine(CollectResources(building));
                            PlayCollectSound();

                        }
                    }

                }

            }
        }

        if (_element != null)
        {
            ElementsController element = _element.GetComponentInParent<ElementsController>();
            if (element.PlagueState == PlagueState.Infected)
            {
                _healText.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) && _interactions == InteractionsEnum.NotInteracting)
                {
                    _animator.SetBool("isHealing", true);
                    _healingElementCoroutine = StartCoroutine(HealElement(element));
                }
            }

        }

        if (_playerController.PlayerMovement != 0)
        {
            if (_interactions == InteractionsEnum.Collecting)
            {
                StopAllCoroutines();
                _interactions = InteractionsEnum.NotInteracting;
                Debug.Log("Collecting interupted!");
                _collectingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _animator.SetBool("isCollecting", false);
            }

            if (_interactions == InteractionsEnum.CuringElement)
            {
                _playerController.InteractionsEmiter.Stop();
                _animator.SetBool("isHealing", false);
                _audioManager.StopEvent(FMODEvents.Instance.ambience);
                StopCoroutine(_healingElementCoroutine);
                _interactions = InteractionsEnum.NotInteracting;
                Debug.LogFormat("<color=red>Healing interupted!</color>");
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Element"))
        {
            _element = null;
            _healText.gameObject.SetActive(false);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            if (other.TryGetComponent(out Building component) && component.PlagueState == PlagueState.Healing)
            {
                component.ChangePlagueState(PlagueState.Infected);
            }
            _structures.Remove(other.gameObject);
            _collectText.gameObject.SetActive(false);
        }

    }

    private IEnumerator HealElement(ElementsController element)
    {
        _interactions = InteractionsEnum.CuringElement;
        _playerController.InteractionsEmiter.Play();
        Debug.LogFormat("<color=green>Healing started</color>");
        yield return new WaitForSeconds(_healingElementDelay);
        HealElementMethod(element);
        _interactions = InteractionsEnum.NotInteracting;

    }

    private IEnumerator CollectResources(Building building)
    {
        _interactions = InteractionsEnum.Collecting;
        yield return new WaitForSeconds(_collectingDelay);
        _collectingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if (building.PlagueState == PlagueState.Healthy)
        {
            building.GiveResource();
        }
        _collectText.gameObject.SetActive(false);
        _playerController.Animator.SetBool("isCollecting", false);
        _interactions = InteractionsEnum.NotInteracting;
        Debug.Log("Collecting finished!");
    }

    private void HealElementMethod(ElementsController element)
    {
        _animator.SetBool("isHealing", false);
        _gameManager.ChangePlayerPlagueLevel(_plagueElementPenalty);
        element.ChangePlagueState(PlagueState.Healthy);
        _healText.gameObject.SetActive(false);
        Debug.LogFormat("<color=green>Healing completed!</color>");
        
    }

    private void PlayCollectSound()
    {
        _collectingInstance.getPlaybackState(out PLAYBACK_STATE playbackState);

        if(playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            Debug.Log("Playing collect sound!");
            _collectingInstance.start();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(StudioEventEmitter))]

public class HealingComponent : MonoBehaviour
{
    [Header("Helper values")]
    [SerializeField]
    private float _healingElementDelay;
    [SerializeField]
    private float _healingStructureDelay;
    [SerializeField]
    private float _plagueElementPenalty;
    [SerializeField]
    private float _plagueStructurePenalty;

    [Header("Helper bools")]
    [SerializeField]
    private bool _isHealingElement;
    [SerializeField]
    private bool _isHealingStructure;
    [SerializeField]
    private bool _isCollecting;

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
        _isHealingElement = false;
        _isHealingStructure = false;
        _isCollecting = false;
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
                        StopCoroutine(CollectResources(building));
                        HealStructure(building);

                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.F) && !_isCollecting)
            {
                foreach (GameObject structure in _structures)
                {
                    if (structure != null && structure.CompareTag("Building"))
                    {
                        BuildingBasic building = structure.GetComponent<BuildingBasic>();

                        if (building.hasFinished && _gameManager.CurrentCurrency >= building.NatureCost)
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
                if (Input.GetKeyDown(KeyCode.F) && !_isHealingElement)
                {
                    _animator.SetBool("isHealing", true);
                    _healingElementCoroutine = StartCoroutine(HealElement(element));
                }
            }
        }

        if (_playerController.PlayerMovement != 0)
        {
            if (_isCollecting)
            {
                _isCollecting = false;
                Debug.Log("Collecting interupted!");
                _collectingInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _animator.SetBool("isCollecting", false);
                StopAllCoroutines();
            }

            if (_isHealingElement)
            {
                _playerController.InteractionsEmiter.Stop();
                _animator.SetBool("isHealing", false);
                _audioManager.StopEvent(FMODEvents.Instance.ambience);
                _isHealingElement = false;
                Debug.LogFormat("<color=red>Healing interupted!</color>");
                StopCoroutine(_healingElementCoroutine);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Element"))
        {
            _element = null;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            _structures.Remove(other.gameObject);
        }

    }

    private IEnumerator HealElement(ElementsController element)
    {
        _isHealingElement = true;
        _playerController.InteractionsEmiter.Play();
        Debug.LogFormat("<color=green>Healing started</color>");
        yield return new WaitForSeconds(_healingElementDelay);
        HealElementMethod(element);
        _isHealingElement = false;

    }

    private void HealStructure(Building building)
    {
        _isHealingStructure = true;
        Debug.LogFormat("<color=orange>Structure healing start!</color>");
        _audioManager.SetPublicVariable("Danger_Phase", 0.0f);
        if (building.hasFinished)
        {
            building.GiveResource();
        }
        building.ChangePlagueState(PlagueState.Healthy);
        _gameManager.buildingsInfected -= 1;
        Debug.LogFormat("<color=green>Healing structure completed!</color>");
        _isHealingStructure = false;

    }

    private IEnumerator CollectResources(Building building)
    {
        _isCollecting = true;
        yield return new WaitForSeconds(3.0f);
        _collectingInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _isCollecting = false;
        building.GiveResource();
        _playerController.Animator.SetBool("isCollecting", false);
        Debug.Log("Collecting finished!");
    }

    private void HealElementMethod(ElementsController element)
    {
        _animator.SetBool("isHealing", false);
        _gameManager.ChangePlayerPlagueLevel(_plagueElementPenalty);
        element.ChangePlagueState(PlagueState.Healthy);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
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
    [SerializeField]
    private float _plagueHealingValue;

    [Header("Helper bools")]
    [SerializeField]
    private bool _isHealingElement;
    [SerializeField]
    private bool _isCuringStructure;
    [SerializeField]
    private bool _isCollecting;

    [Header("Object References")]
    [SerializeField]
    private List<GameObject> _structures;
    [SerializeField]
    private GameObject _element;
    private PlayerController _playerController;
    private Animator _animator;
    [SerializeField]
    private StudioEventEmitter _structuresEmitter;
    private AudioManager _audioManager;
    private GameManager _gameManager;

    private void Awake()
    {
        _isHealingElement = false;
        _isCuringStructure = false;
        _isCollecting = false;
        _playerController = GetComponentInParent<PlayerController>();
        _animator = GetComponentInParent<Animator>();
        
    }

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _gameManager = GameManager.Instance;
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
            for(int i = 0; i < _structures.Count; i++)
            {
                if (_structures[i] != null)
                {
                    Building building = _structures[i].GetComponent<Building>();
                    if (building.isInfected)
                    {
                        HealStructure(building);

                    }
                }
                
            }

            int randomStructure = Random.Range(0, _structures.Count);

            if (_structures[randomStructure] != null)
            {
                Building building = _structures[randomStructure].GetComponent<Building>();
                if (building.hasPlague)
                {
                    if (Input.GetKeyDown(KeyCode.F) && !_isCuringStructure)
                    {
                        _animator.SetBool("isHealing", true);
                        StartCoroutine(CureStructure(building));
                    }
                }

                if (_isCuringStructure && _playerController.PlayerMovement != 0)
                {
                    StopAllCoroutines();
                    _playerController.InteractionsEmiter.Stop();
                    _animator.SetBool("isHealing", false);
                    _isCuringStructure = false;
                }

            }


        }
    }

    private void Update()
    {
        if(_element != null)
        {
            ElementsController element = _element.GetComponentInParent<ElementsController>();
            if (element.hasPlague)
            {
                if (Input.GetKeyDown(KeyCode.F) && !_isHealingElement)
                {
                    _animator.SetBool("isHealing", true);
                    StartCoroutine(HealElement(element));
                }
            }

            if(_isHealingElement && _playerController.PlayerMovement != 0)
            {
                StopAllCoroutines();
                _playerController.InteractionsEmiter.Stop();
                _animator.SetBool("isHealing", false);
                _audioManager.StopEvent(FMODEvents.Instance.ambience);
                _isHealingElement = false;
                Debug.LogFormat("<color=red>Healing interupted!</color>");
            }   
        }

        if(_structures.Count != 0)
        {
            int randomStructure = Random.Range(0, _structures.Count);

            if (_structures[randomStructure] != null)
            {
                Building building = _structures[randomStructure].GetComponent<Building>();
                if (building.hasPlague)
                {
                    if (Input.GetKeyDown(KeyCode.F) && !_isCuringStructure)
                    {
                        _animator.SetBool("isHealing", true);
                        StartCoroutine(CureStructure(building));
                    }
                }

                if(_isCuringStructure && _playerController.PlayerMovement != 0)
                {
                    StopAllCoroutines();
                    _playerController.InteractionsEmiter.Stop();
                    _animator.SetBool("isHealing", false);
                    _isCuringStructure = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && !_isCollecting)
            {
                
                foreach (GameObject structure in _structures)
                {
                    if(structure != null && structure.CompareTag("Building"))
                    {
                        BuildingBasic building = structure.GetComponent<BuildingBasic>();
                        BuildingHelper buildingHelper = structure.GetComponentInChildren<BuildingHelper>();

                        if (building.hasFinished && _gameManager.CurrentCurrency >= building.NatureCost)
                        {
                            Debug.Log("Collecting resources");
                            _animator.SetBool("isCollecting", true);
                            StartCoroutine(CollectResources(building, buildingHelper));
                            if (!_playerController.CollectingEmitter.IsPlaying())
                            {
                                _playerController.CollectingEmitter.Play();
                            }

                        }
                    }
                    
                }
                
            }

            if (_isCollecting && _playerController.PlayerMovement != 0)
            {
                _isCollecting = false;
                Debug.Log("Collecting interupted!");
                _playerController.CollectingEmitter.Stop();
                StopAllCoroutines();
                _animator.SetBool("isCollecting", false);

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
        Debug.LogFormat("<color=orange>Structure healing start!</color>");
        _audioManager.SetPublicVariable("Danger_Phase", 0.0f);
        building.isInfected = false;
        building.infectedState.SetActive(false);
        building.healthyState.SetActive(true);
        building.CurrentPlague = 0.0f;
        if(building.hasFinished)
        {
            building.GiveResourceToPlayer();
        }
        _gameManager.buildingsInfected -= 1;
        Debug.LogFormat("<color=green>Healing structure completed!</color>");

    }

    private IEnumerator CureStructure(Building building)
    {
        _isCuringStructure = true;
        _playerController.InteractionsEmiter.Play();
        Debug.LogFormat("<color=green>Curing structure started</color>");
        yield return new WaitForSeconds(_healingElementDelay);
        CureStructureMethod(building);
        _isCuringStructure = false;
    }

    private IEnumerator CollectResources(Building building, BuildingHelper buildingHelper)
    {
        _isCollecting = true;
        yield return new WaitForSeconds(3.0f);
        _isCollecting = false;
        building.GiveResourceToPlayer();
        _playerController.Animator.SetBool("isCollecting", false);
        buildingHelper.ResetProduction();
        Debug.Log("Collecting finished!");
    }

    private void HealElementMethod(ElementsController element)
    {
        _animator.SetBool("isHealing", false);
        _gameManager.ChangePlayerPlagueLevel(_plagueElementPenalty);
        element.ObjectWithPlague.SetActive(false);
        element.HealthyObject.SetActive(true);
        element.hasPlague = false;
        Debug.LogFormat("<color=green>Healing completed!</color>");
        
    }

    private void CureStructureMethod(Building building)
    {
        _animator.SetBool("isHealing", false);
        _gameManager.buildingsInfected -= 1;
        _gameManager.ChangePlayerPlagueLevel(_plagueStructurePenalty);
        building.plagueState.SetActive(false);
        building.healthyState.SetActive(true);
        building.hasPlague = false;
        Debug.LogFormat("<color=green>Curing structure completed!</color>");
    }

}

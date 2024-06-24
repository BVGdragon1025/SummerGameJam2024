using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Indicator data")]
    [Tooltip("Indicator Game Object")]
    public GameObject _targetIndicator;

    [Header("Building Placement Data")]
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public MeshRenderer meshComponent;
    private Dictionary<MeshRenderer, List<Material>> _initialMaterials;

    [HideInInspector] public bool hasValidPlacement;
    [HideInInspector] public bool isPlaced;

    private Building _building;
    private bool _isDying;
    private bool _hasCoroutineStarted;
    [SerializeField]
    private float _infectionTimer;
    public float InfectionTimer { get { return _infectionTimer; } }
    [SerializeField]
    private float _healingTimer;
    public float HealingTimer { get { return _healingTimer; } }

    private int _numberOfObstacles;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private BuildingType _elementTag;
    private GameObject _player;
    private GameObject _element;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private BuildingPlacer _buildingPlacer;
    private Coroutine _deathCoroutine;
    

    private void Awake()
    {
        _isDying = false;
        hasValidPlacement = true;
        isPlaced = true;
        _numberOfObstacles = 0;
        _building = GetComponent<Building>();
        _elementTag = _building.BuildingType;
        InitializeMaterials();

    }

    private void Start()
    {
        _buildingPlacer = BuildingPlacer.Instance;
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (_building.PlagueState == PlagueState.Infected)
        {
            _infectionTimer += _gameManager.Timer(0.0f, _building.MaxPlagueTime);
            _audioManager.SetPublicVariable("Danger_Phase", _infectionTimer);
            //Debug.Log(_infectionTimer);
            if (!_isDying)
            {
                _deathCoroutine = StartCoroutine(DeathTimer());
                _targetIndicator.SetActive(true);
            }
            
        }

        if(_building.PlagueState == PlagueState.Healthy && _isDying)
        {
            StopDeathCoroutine();
            _targetIndicator.SetActive(false);
            _isDying = false;
            _infectionTimer = _gameManager.ResetTimer();
        }

        if(_building.PlagueState == PlagueState.Healing)
        {
            StopDeathCoroutine();
            _healingTimer += _gameManager.Timer(0.0f, _building.MaxHealingTime);
            if (_healingTimer >= 1.0f)
            {
                if (_building.HasFinished)
                {
                    _building.GiveResource();
                }
                _healingTimer = _gameManager.ResetTimer();
                _building.ChangePlagueState(PlagueState.Healthy);
                _gameManager.buildingsInfected--;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        if (((1 << other.gameObject.layer) & _playerLayer.value) != 0)
        {
            _player = other.gameObject;
        }

        if (_building.CompareTag("Building"))
        {
            if (other.gameObject.CompareTag(_elementTag.ToString()))
            {
                _element = other.gameObject;
            }
        }

        if (_building.CompareTag("Building"))
        {
            if (_player != null && _element != null)
            {
                Debug.Log("You can plant!");
                SetPlacementMode(BuildingState.Valid);
                if (IsPlaced(other.gameObject))
                {
                    //Debug.Log("Checking layers!");
                    SetPlacementMode(BuildingState.NotValid);
                    return;
                }
                return;
            }
        }

        if (_building.CompareTag("BuildingSpecial"))
        {
            if (_player != null)
            {
                Debug.Log("You can plant!");
                SetPlacementMode(BuildingState.Valid);
                if (IsPlaced(other.gameObject))
                {
                    Debug.Log("Checking layers!");
                    SetPlacementMode(BuildingState.NotValid);
                    return;
                }
                return;
            }
        }

        _numberOfObstacles++;
        SetPlacementMode(BuildingState.NotValid);

    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlaced) return;

        if (IsPlaced(other.gameObject)) return;

        if (((1 << other.gameObject.layer) & _playerLayer.value) != 0)
        {
            _player = null;
        }

        if (_building.CompareTag("Building"))
        {
            if (other.gameObject.CompareTag(_elementTag.ToString()))
            {
                _element = null;
            }
        }


        if (_building.CompareTag("BuildingSpecial"))
        {
            if (_player == null)
            {
                SetPlacementMode(BuildingState.NotValid);
                return;
            }
        }

        if (_building.CompareTag("Building"))
        {
            if (_player == null || _element == null)
            {
                Debug.Log("You can't plant!");
                SetPlacementMode(BuildingState.NotValid);
                return;
            }
        }
        
        _numberOfObstacles--;


        if (_numberOfObstacles == 0)
        {
            Debug.Log("No obstacles!");
            if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Element"))
            {
                SetPlacementMode(BuildingState.Valid);
            }
        }
    }

    public void SetPlacementMode(BuildingState state)
    {
        switch (state)
        {
            case BuildingState.Placed:
                isPlaced = true;
                hasValidPlacement = true;
                _building.enabled = true;
                _building.GetComponent<Collider>().excludeLayers = 0;
                if (gameObject.CompareTag("Building"))
                {
                    StartCoroutine(_building.StartProduction(_building.SpawnRate));
                    _targetIndicator = _buildingPlacer.CreateWaypoint(transform);
                    _gameManager.structures.Add(_building);
                    ElementsHelper element = _element.GetComponent<ElementsHelper>();
                    element.AmountOfStructures++;
                }
                break;
            case BuildingState.Valid:
                hasValidPlacement = true;
                break;
            case BuildingState.NotValid:
                hasValidPlacement = false;
                break;
        }

        SetMaterial(state);
    }

    public void SetMaterial(BuildingState state)
    {
        if (state == BuildingState.Placed)
        {
            meshComponent.sharedMaterials = _initialMaterials[meshComponent].ToArray();

        }
        else
        {
            Material materialToApply = state == BuildingState.Valid
                ? validPlacementMaterial : invalidPlacementMaterial;

            Material[] materials;
            int numberOfMaterials;

            numberOfMaterials = _initialMaterials[meshComponent].Count;
            materials = new Material[numberOfMaterials];
            for (int i = 0; i < numberOfMaterials; i++)
            {
                materials[i] = materialToApply;
            }
            meshComponent.sharedMaterials = materials;
            
        }
    }

    private void InitializeMaterials()
    {
        if (_initialMaterials == null)
        {
            _initialMaterials = new Dictionary<MeshRenderer, List<Material>>();
        }

        if (_initialMaterials.Count > 0)
        {
            foreach (var initialMaterial in _initialMaterials)
            {
                initialMaterial.Value.Clear();
            }
            _initialMaterials.Clear();
        }

        _initialMaterials[meshComponent] = new List<Material>(meshComponent.sharedMaterials);

    }

    private bool IsPlaced(GameObject gameObject)
    {
        return ((1 << gameObject.layer) & _buildingPlacer.groundLayerMask.value) != 0;
    }

    private IEnumerator DeathTimer()
    {
        _isDying = true;
        _hasCoroutineStarted = true;
        yield return new WaitForSeconds(_building.MaxPlagueTime);
        _hasCoroutineStarted = false;
        Destroy(gameObject);
    }

    private void StopDeathCoroutine()
    {
        if (_hasCoroutineStarted)
        {
            StopCoroutine(_deathCoroutine);
            _hasCoroutineStarted = false;
        }
    }

    private void LerpMaterials(Material currentMaterial, Material firstMaterial, Material secondMaterial, float timerValue)
    {
        currentMaterial.Lerp(firstMaterial, secondMaterial, timerValue);
    }

    private void OnDestroy()
    {
        if (isPlaced)
        {
            if(_element != null)
            {
                _element.GetComponent<ElementsHelper>().AmountOfStructures--;
            }

            if(_gameManager != null)
            {
                _gameManager.buildingsInfected--;
                _gameManager.structures.Remove(_building);
            }

            if(_targetIndicator != null)
                _targetIndicator.SetActive(false);
        }

    }

}

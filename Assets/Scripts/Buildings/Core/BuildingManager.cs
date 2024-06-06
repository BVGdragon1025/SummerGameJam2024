using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WayPoints))]

public class BuildingManager : MonoBehaviour
{

    [Header("Waypoints data")]
    [Tooltip("Sprite used by Waypoint")]
    public Sprite _waypointSprite;

    [Header("Building Placement Data")]
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public MeshRenderer[] meshComponents;
    private Dictionary<MeshRenderer, List<Material>> _initialMaterials;

    [HideInInspector] public bool hasValidPlacement;
    [HideInInspector] public bool isPlaced;

    private Building _building;
    private bool _deathTimer;
    public float timer;

    private int _numberOfObstacles;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private BuildingType _elementTag;
    private GameObject _player;
    private GameObject _element;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private BuildingPlacer _buildingPlacer;
    [HideInInspector] public WayPoints _wayPoints;

    private void Awake()
    {
        _deathTimer = false;
        hasValidPlacement = true;
        isPlaced = true;
        _numberOfObstacles = 0;
        _building = GetComponent<Building>();
        _elementTag = _building.BuildingType;
        _wayPoints = GetComponent<WayPoints>();
        _wayPoints.enabled = false;
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
        if (_building.isInfected)
        {
            timer += _gameManager.Timer(_building.CurrentPlague, _building.MaxPlagueTime);
            _audioManager.SetPublicVariable("Danger_Phase", timer);
            //Debug.Log(timer);
            if (!_deathTimer)
            {
                _wayPoints.TurnOnWaypoint();
                _building.healthyState.SetActive(false);
                _building.infectedState.SetActive(true);
                StartCoroutine(nameof(DeathTimer));
            }
            
        }

        if(!_building.isInfected && _deathTimer)
        {
            StopCoroutine(nameof(DeathTimer));
            _wayPoints.TurnOffWaypoint();
            _deathTimer = false;
            timer = _gameManager.ResetTimer();
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
                if (gameObject.CompareTag("Building"))
                {
                    _wayPoints.enabled = true;
                    _gameManager.structures.Add(_building);
                    ElementsHelper element = _element.GetComponent<ElementsHelper>();
                    if (element.buildingType == _building.BuildingType)
                    {
                        element.AddStructureToList(gameObject);
                    }
                }
                _building.triggerGameObject.SetActive(true);
                _building.GetComponent<Collider>().excludeLayers = 0;
                _buildingPlacer.CreateWaypoint(_wayPoints, _waypointSprite, transform);
                
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
            foreach (MeshRenderer renderer in meshComponents)
            {
                renderer.sharedMaterials = _initialMaterials[renderer].ToArray();
            }
        }
        else
        {
            Material materialToApply = state == BuildingState.Valid
                ? validPlacementMaterial : invalidPlacementMaterial;

            Material[] materials;
            int numberOfMaterials;

            foreach (MeshRenderer renderer in meshComponents)
            {
                numberOfMaterials = _initialMaterials[renderer].Count;
                materials = new Material[numberOfMaterials];
                for (int i = 0; i < numberOfMaterials; i++)
                {
                    materials[i] = materialToApply;
                }
                renderer.sharedMaterials = materials;
            }
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

        foreach (MeshRenderer renderer in meshComponents)
        {
            _initialMaterials[renderer] = new List<Material>(renderer.sharedMaterials);
        }

    }

    private bool IsPlaced(GameObject gameObject)
    {
        return ((1 << gameObject.layer) & _buildingPlacer.groundLayerMask.value) != 0;
    }

    private IEnumerator DeathTimer()
    {
        _deathTimer = true;
        yield return new WaitForSeconds(_building.MaxPlagueTime);
        _building.isInfected = false;
        _building.hasPlague = true;
        _wayPoints.TurnOffWaypoint();
        _building.infectedState.SetActive(false);
        _building.plagueState.SetActive(true);
        _building.CurrentPlague = 0.0f;
        timer = _gameManager.ResetTimer();
        _deathTimer = false;
    }

}

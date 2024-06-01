using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

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
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _element;

    private void Awake()
    {
        _deathTimer = false;
        hasValidPlacement = true;
        isPlaced = true;
        _numberOfObstacles = 0;
        _building = GetComponent<Building>();

        InitializeMaterials();

    }

    private void Update()
    {
        if (_building.isInfected)
        {
            timer += GameManager.Instance.Timer(_building.CurrentPlague, _building.MaxPlagueTime);
            AudioManager.Instance.SetPublicVariable("Danger_Phase", timer);
            Debug.Log(timer);
            if (!_deathTimer)
            {
                _building.healthyState.SetActive(false);
                _building.infectedState.SetActive(true);
                StartCoroutine(nameof(DeathTimer));
            }
            
        }

        if(!_building.isInfected && _deathTimer)
        {
            StopCoroutine(nameof(DeathTimer));
            _deathTimer = false;
            timer = GameManager.Instance.ResetTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask playerLayer = LayerMask.NameToLayer("PlayerRadius");
        LayerMask elementLayer = LayerMask.NameToLayer("ElementLayer");

        if (isPlaced) return;

        if (other.gameObject.layer == playerLayer)
        {
            _player = other.gameObject;
        }

        if (other.gameObject.layer == elementLayer)
        {
            _element = other.gameObject;
        }

        if (_building.CompareTag("Building"))
        {
            if (_player != null && _element != null)
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
        LayerMask playerLayer = LayerMask.NameToLayer("PlayerRadius");
        LayerMask elementLayer = LayerMask.NameToLayer("ElementLayer");

        if (isPlaced) return;

        if (IsPlaced(other.gameObject)) return;

        if (other.gameObject.layer == playerLayer)
        {
            _player = null;
        }

        if (other.gameObject.layer == elementLayer)
        {
            _element = null;
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
            
        //SetPlacementMode(BuildingState.Valid);


    }

    public void SetPlacementMode(BuildingState state)
    {
        switch (state)
        {
            case BuildingState.Placed:
                isPlaced = true;
                hasValidPlacement = true;
                if(gameObject.CompareTag("Building"))
                    GameManager.Instance.structures.Add(_building);
                _building.triggerGameObject.SetActive(true);
                _building.GetComponent<Collider>().excludeLayers = 0;
                ElementsHelper element = _element.GetComponent<ElementsHelper>();
                if (element.buildingType == _building.BuildingType)
                {
                    element.AddStructureToList(gameObject);
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
        return ((1 << gameObject.layer) & BuildingPlacer.Instance.groundLayerMask.value) != 0;
    }

    private IEnumerator DeathTimer()
    {
        _deathTimer = true;
        yield return new WaitForSeconds(_building.MaxPlagueTime);
        _building.isInfected = false;
        _building.hasPlague = true;
        _building.infectedState.SetActive(false);
        _building.plagueState.SetActive(true);
        _building.CurrentPlague = 0.0f;
        timer = GameManager.Instance.ResetTimer();
        _deathTimer = false;
    }


}

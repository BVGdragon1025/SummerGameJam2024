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

    private int _numberOfObstacles;

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
        if (_building.isInfected && !_deathTimer)
        {
            _building.healthyState.SetActive(false);
            _building.infectedState.SetActive(true);
            StartCoroutine(nameof(DeathTimer));
        }

        if(!_building.isInfected && _deathTimer)
        {
            StopCoroutine(nameof(DeathTimer));
            _deathTimer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        if (IsPlaced(other.gameObject)) return;

        _numberOfObstacles++;

        SetPlacementMode(BuildingState.NotValid);

    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlaced) return;

        if (IsPlaced(other.gameObject)) return;

        _numberOfObstacles--;

        if (_numberOfObstacles == 0)
            SetPlacementMode(BuildingState.Valid);
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
        _deathTimer = false;
    }


}

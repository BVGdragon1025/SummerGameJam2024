using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [Header("Building Data"), Tooltip("Resource values and other things")]
    [SerializeField]
    protected int buildingCost = 0;
    [SerializeField, Tooltip("Specifies which layers this building needs")]
    private LayerMask layers;

    [Header("Building Placement Data")]
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    public MeshRenderer[] meshComponents;
    private Dictionary<MeshRenderer, List<Material>> _initialMaterials;

    [HideInInspector] public bool hasValidPlacement;
    [HideInInspector] public bool isPlaced;

    [SerializeField]
    private int _numberOfObstacles;

    private void Awake()
    {
        hasValidPlacement = true;
        isPlaced = true;
        _numberOfObstacles = 0;

        InitializeMaterials();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(isPlaced) return;

        if(IsPlaced(other.gameObject)) return;

        if (other.CompareTag("Player")) return;

        _numberOfObstacles++;

        SetPlacementMode(BuildingState.NotValid);

    }

    private void OnTriggerExit(Collider other)
    {
        if(isPlaced) return;

        if (IsPlaced(other.gameObject)) return;

        if (other.CompareTag("Player")) return;

        _numberOfObstacles--;

        if (_numberOfObstacles == 0)
            SetPlacementMode(BuildingState.Valid);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeMaterials(); 
    }
#endif

    public void SetPlacementMode(BuildingState state)
    {
        switch(state)
        {
            case BuildingState.Placed:
                isPlaced = true;
                hasValidPlacement = true;
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
        if(state == BuildingState.Placed)
        {
            foreach(MeshRenderer renderer in meshComponents)
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

            foreach(MeshRenderer renderer in meshComponents)
            {
                numberOfMaterials = _initialMaterials[renderer].Count;
                materials = new Material[numberOfMaterials];
                for(int i = 0; i < numberOfMaterials; i++)
                {
                    materials[i] = materialToApply;
                }
                renderer.sharedMaterials = materials;
            }
        }
    }

    private void InitializeMaterials()
    {
        if(_initialMaterials == null)
        {
            _initialMaterials = new Dictionary<MeshRenderer, List<Material>>();
        }

        if(_initialMaterials.Count > 0)
        {
            foreach(var initialMaterial in _initialMaterials)
            {
                initialMaterial.Value.Clear();
            }
            _initialMaterials.Clear();
        }

        foreach(MeshRenderer renderer in meshComponents)
        {
            _initialMaterials[renderer] = new List<Material>(renderer.sharedMaterials);
        }

    }

    public bool CheckLayerMask(Vector3 position, float radius)
    {
        int maxColliders = 10;

        Collider[] memory = new Collider[maxColliders];
        int colliders = Physics.OverlapSphereNonAlloc(position, radius, memory, layers);

        if(colliders >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private bool IsPlaced(GameObject gameObject)
    {
        return ((1 << gameObject.layer) & BuildingPlacer.Instance.groundLayerMask.value) != 0;
    }

}

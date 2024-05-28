using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    private GameObject _buildingPrefab;
    private GameObject _toBuild;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;

    public LayerMask groundLayerMask;
    public static BuildingPlacer Instance { get; private set; }

    private void Awake()
    {
        /*
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        */

        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Update()
    {
        if(_buildingPrefab != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(_toBuild.activeSelf)
                    _toBuild.SetActive(false);
            }
            else if (!_toBuild.activeSelf)
            {
                _toBuild.SetActive(true);
            }

            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
            {
                Debug.Log($"GameObject {_hit.collider.name} with LayerMask {LayerMask.LayerToName(_hit.collider.gameObject.layer)}");

                if(_toBuild.activeSelf)
                    _toBuild.SetActive(true);

                _toBuild.transform.position = new Vector3(_hit.point.x,  _toBuild.transform.position.y, _hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    Building building = _toBuild.GetComponent<Building>();

                    if (building.hasValidPlacement)
                    {
                        building.SetPlacementMode(BuildingState.Placed);

                        _buildingPrefab = null;
                        _toBuild = null;
                    }
                    

                }
            }
            else if (_toBuild.activeSelf)
            {
                _toBuild.SetActive(false);
            }
                
        }
    }

    public void SetBuildinfPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        PrepareBuilding();
    }

    private void PrepareBuilding()
    {
        if(_toBuild)
            Destroy(_toBuild);

        Vector3 position = new Vector3(_buildingPrefab.transform.position.x, _buildingPrefab.transform.localScale.y / 2, _buildingPrefab.transform.position.z);

        _toBuild = Instantiate(_buildingPrefab, position, Quaternion.identity);
        _toBuild.SetActive(false);

        Building building = _toBuild.GetComponent<Building>();
        building.isPlaced = false;
        building.SetPlacementMode(BuildingState.Valid);
    }

}

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
    public static BuildingPlacer Instance;

    private void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Update()
    {
        if(_buildingPrefab != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(_toBuild);
                _toBuild = null;
                _buildingPrefab = null;
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(_toBuild.activeSelf)
                    _toBuild.SetActive(false);
                return;
            }
            else if (!_toBuild.activeSelf)
            {
                _toBuild.SetActive(true);
            }

            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
            {
                //Debug.Log($"GameObject {_hit.collider.name} with LayerMask {LayerMask.LayerToName(_hit.collider.gameObject.layer)}");

                if(_toBuild.activeSelf)
                    _toBuild.SetActive(true);

                _toBuild.transform.position = new Vector3(_hit.point.x, transform.localScale.y / 2, _hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    Building building = _toBuild.GetComponent<Building>();

                    if (building.hasValidPlacement)
                    {

                        building.SetPlacementMode(BuildingState.Placed);

                        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        {
                            _toBuild = null;
                            PrepareBuilding();
                        }
                        else
                        {
                            building.GetComponent<Collider>().isTrigger = false;

                            _buildingPrefab = null;
                            _toBuild = null;
                        }
                        
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
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void PrepareBuilding()
    {
        if(_toBuild)
            Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab);
        _toBuild.SetActive(false);

        Building building = _toBuild.GetComponent<Building>();
        building.isPlaced = false;
        building.SetPlacementMode(BuildingState.Valid);
    }

}

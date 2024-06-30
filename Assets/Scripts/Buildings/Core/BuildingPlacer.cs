using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Indicators Section")]
    [SerializeField, Tooltip("GameObject under Player Prefab, where Indicator prefabs will be stored"), FormerlySerializedAs("_waypointCanvas")]
    private Transform _indicatorsParent;
    [SerializeField] private GameObject _indicatorObject;

    [Header("Building Section")]
    private GameObject _buildingPrefab;
    private GameObject _toBuild;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;
    [SerializeField] private float _buildingCooldown;


    public LayerMask groundLayerMask;
    public static BuildingPlacer Instance;

    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
                Cursor.visible = true;
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

                if(_toBuild.activeSelf)
                    _toBuild.SetActive(true);

                //_toBuild.transform.position = new Vector3(_hit.point.x, transform.localScale.y / 2, _hit.point.z);
                _toBuild.transform.position = _hit.point;

                BuildingManager buildingManager = _toBuild.GetComponent<BuildingManager>();
                Building building = _toBuild.GetComponent<Building>();

                if (building.HasCurrency())
                {
                    if (Input.GetMouseButtonDown(0))
                    {

                        if (buildingManager.hasValidPlacement)
                        {

                            buildingManager.SetPlacementMode(BuildingState.Placed);
                            GameManager.Instance.ChangeCurrencyValue(-building.BuildingCost);
                            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            {
                                _toBuild = null;
                                PrepareBuilding();
                            }
                            else
                            {
                                buildingManager.GetComponent<Collider>().isTrigger = false;
                                Cursor.visible = true;
                                _buildingPrefab = null;
                                _toBuild = null;
                            }

                        }
                        else
                        {
                            buildingManager.SetPlacementMode(BuildingState.NotValid);
                        }

                    }
                }
                else
                {
                    buildingManager.SetPlacementMode(BuildingState.NotValid);
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
        Cursor.visible = false;
    }

    private void PrepareBuilding()
    {
        if(_toBuild)
            Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab);
        _toBuild.SetActive(false);

        BuildingManager buildingManager = _toBuild.GetComponent<BuildingManager>();
        buildingManager.isPlaced = false;
        buildingManager.SetPlacementMode(BuildingState.NotValid);
    }

    public GameObject CreateWaypoint(Transform buildingTransform)
    {
        GameObject indicator = Instantiate(_indicatorObject,
            _indicatorsParent.position,
            _indicatorsParent.rotation,
            _indicatorsParent);
        indicator.GetComponent<TargetIndicator>().targetTransform = buildingTransform;
        indicator.SetActive(false);

        return indicator;
    }

}

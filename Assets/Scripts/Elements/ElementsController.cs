using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsController : MonoBehaviour
{
    public bool hasPlague;
    [SerializeField]
    private GameObject _objectWithPlague;
    public GameObject ObjectWithPlague { get { return _objectWithPlague; } }
    [SerializeField]
    private GameObject _healthyObject;
    public GameObject HealthyObject { get { return _healthyObject; } }

    void OnEnable()
    {
        hasPlague = true;
    }

}

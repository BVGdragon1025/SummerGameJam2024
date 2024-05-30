using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsController : MonoBehaviour
{
    public bool hasPlague;
    [SerializeField]
    private GameObject _objectWithPlague;
    [SerializeField]
    private GameObject _healthyObject;

    // Start is called before the first frame update
    void Start()
    {
        hasPlague = true;
    }

}

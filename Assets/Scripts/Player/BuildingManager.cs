using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public void BuildStructure(GameObject buildingToBuilt)
    {
        Camera mainCam = Camera.main;

        Vector2 origin = Vector2.zero;
        Vector2 direction = Vector2.zero;

        if (Input.GetMouseButton(0))
        {
            if (mainCam.orthographic)
            {
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                origin = ray.origin;
                direction = ray.direction;
            }

            RaycastHit2D hit = Physics2D.Raycast(origin, direction);

            if (hit.collider.CompareTag("PlayerRange"))
            {
                Instantiate(buildingToBuilt, hit.transform.position, Quaternion.identity);
            }

        }
    }

}

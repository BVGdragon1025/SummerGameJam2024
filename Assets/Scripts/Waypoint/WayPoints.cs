using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoints : MonoBehaviour
{
    public Image image;
    public Transform target;
    [SerializeField] private Vector3 _displacement;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {

        UpdateWaypoint(image, target);

    }

    private void UpdateWaypoint(Image img, Transform target)
    {
        float minX = img.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector2 pos = _camera.WorldToScreenPoint(target.position + _displacement);

        Debug.Log($"Min X: {minX}, Max X: {maxX}. Min Y: {minY}, Max Y: {maxY}");

        Debug.Log($"Raw waypoint position: {pos}");

        if (Vector3.Dot(((target.position + _displacement) - transform.position), transform.forward) < 0)
        {
            // Target is behind the player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }

            if (pos.y > maxY)
            {
                pos.y = minY;
            }
        }


        Debug.Log($"Waypoint position after if statement:  {pos}");
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        Debug.Log($"Waypoint position after Mathf.Clamp: {pos}");


        img.transform.position = pos;
    }

    public void TurnOffWaypoint()
    {
        image.gameObject.SetActive(false);
    }

    public void TurnOnWaypoint()
    {
        image.gameObject.SetActive(true);
    }

}


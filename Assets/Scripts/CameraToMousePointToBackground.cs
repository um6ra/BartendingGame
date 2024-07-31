using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToMousePointToBackground : MonoBehaviour
{
    // Singleton instance
    public static CameraToMousePointToBackground Instance { get; private set; }

    // Serialized field for the layer mask
    [SerializeField]
    private LayerMask raycastLayerMask;

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to get the point hit by the raycast from the camera through the mouse position
    public Vector3 GetMouseHitPoint()
    {
        Vector3 hitPoint = Vector3.zero;

        // Get the mouse position in the world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayerMask))
        {
            hitPoint = hit.point;
        }

        return hitPoint;
    }
}

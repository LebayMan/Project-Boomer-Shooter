using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Layer")]
    public LayerMask obstacleLayer;  // Specify the layer for obstacles

    [Header("Shooting Reference")]
    public Transform shootFrom;      // Reference for where the ray should be cast from
    public GameObject BulletImpact;  // The bullet impact prefab

    [Header("Impact Offset")]
    public float impactOffset = 0.01f; // Distance to offset the impact from the wall

    public void Shoot()
    {
        // If there's no reference to shoot from, default to Camera.main
        if (shootFrom == null)
        {
            shootFrom = Camera.main.transform;
        }

        // Create the ray from the specified point and direction
        Ray ray = new Ray(shootFrom.position, shootFrom.forward);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstacleLayer))
        {
            Debug.Log("Hit object: " + hit.collider.name);

            // Calculate an offset position by adding the hit normal multiplied by the offset
            Vector3 impactPosition = hit.point + hit.normal * impactOffset;

            // Instantiate the BulletImpact prefab at the offset position
            Instantiate(BulletImpact, impactPosition, Quaternion.LookRotation(hit.normal));
        }
    }
}

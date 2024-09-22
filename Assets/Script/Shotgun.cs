using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    [Header("Layer")]
    public LayerMask obstacleLayer;

    [Header("Shooting Reference")]
    public Transform shootFrom;
    public GameObject BulletImpact;

    [Header("Impact Offset")]
    public float impactOffset = 0.01f;
    
    [Header("Ammo")]
    public float Ammo = 6;
    public float maxAmmo = 6;

    [Header("Animation")]
    public Animator animator; // Reference to the Animator component

    public void Shoot()
    {
        shootFrom = Camera.main.transform;
        if (Ammo > 0)
        {
            Ray ray = new Ray(shootFrom.position, shootFrom.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstacleLayer))
            {
                Debug.Log("Hit object: " + hit.collider.name);
                Vector3 impactPosition = hit.point + hit.normal * impactOffset;
                Instantiate(BulletImpact, impactPosition, Quaternion.LookRotation(hit.normal));
                Ammo--;
            }
            else
            {
            Ammo--;
            }
        }
        else
        {
            Debug.Log("Out of Ammo!");
        }
    }

    public void Reload()
    {
        if (animator != null)
        {
            animator.SetTrigger("Reload"); // Trigger the reload animation
        }
        StartCoroutine(ReloadCoroutine());
    }

    private void FixedUpdate()
    {
        if (Ammo == 0)
        {
            Reload();
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        // Wait for the animation duration before reloading
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Ammo = maxAmmo;
        Debug.Log("Reloaded");
    }

    public float GetAmmo()
    {
        return Ammo;
    }
}

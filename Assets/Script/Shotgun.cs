using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour, IGun
{
    [Header("Layer")]
    public LayerMask obstacleLayer;
    public LayerMask EnemyLayer;


    [Header("Shooting Reference")]
    public Transform shootFrom;
    public GameObject BulletImpact;

    

    [Header("Impact Offset")]
    public float impactOffset = 0.01f;
    [Header("Damage")]
    public float Damage_Point = 10;
    public float maxRayDistance = 100f;
    
    [Header("Ammo")]
    public float Ammo = 6; 
    public float maxAmmo = 6;

    [Header("Animation")]
    public Animator animator; // Reference to the Animator component
    private bool isReloading = false; // Flag to track if the gun is reloading

public void Shoot()
{
    if (isReloading)
    {
        Debug.Log("Cannot shoot while reloading.");
        return;
    }

    shootFrom = Camera.main.transform;

    if (Ammo > 0)
    {
        Ray ray = new Ray(shootFrom.position, shootFrom.forward);
        RaycastHit hit;
        // Check for obstacles first
        if (Physics.Raycast(ray, out hit, maxRayDistance, obstacleLayer))
        {
            Debug.Log("Hit object: " + hit.collider.name);
            Vector3 impactPosition = hit.point + hit.normal * impactOffset;
            Instantiate(BulletImpact, impactPosition, Quaternion.LookRotation(hit.normal));
        }
        // Check for enemies
        if (Physics.Raycast(ray, out hit, maxRayDistance, EnemyLayer))
        {
            CollisonEnemy enemyScript = hit.collider.GetComponent<CollisonEnemy>();
            if (enemyScript != null)
            {
                enemyScript.Hit(Damage_Point);
            }
        }

        // Ensure ammo doesn't go below zero
        if (Ammo < 0)
        {
            Ammo = 0;
        }
        Ammo-= 1;
        
    }
    else
    {
        Debug.Log("Out of Ammo!");
    }
}

public void Reload()
{
    if (Ammo < maxAmmo && !isReloading)
    {
        isReloading = true; // Set reloading flag to true
        if (animator != null)
        {
            animator.SetTrigger("Reload"); // Trigger the reload animation
        }
        StartCoroutine(ReloadCoroutine());
    }
}


private void FixedUpdate()
{
    if (Ammo == 0 && !isReloading)
    {
        Reload();
        isReloading = true; // Set the flag to true when reloading starts
    }

    // Ensure ammo doesn't go below zero
    if (Ammo < 0)
    {
        Ammo = 0;
    }
}

    private IEnumerator ReloadCoroutine()
    {
        // Wait for the animation duration before reloading
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Ammo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded");
    }

    public float GetAmmo()
    {
        return Ammo;
    }
    public float GetMaxAmmo()
    {
        return maxAmmo;
    }
    public bool IsReloading()
    {
        return isReloading;
    }
    public void Scope()
    {
        Debug.Log("NO LOL");
    }
        public bool isScopeing1()
    {
        bool awdaw = false;
        return  awdaw;
    }
}

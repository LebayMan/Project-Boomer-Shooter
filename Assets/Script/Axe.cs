using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Axe : MonoBehaviour, IGun
{
    [Header("Layer")]
    public LayerMask obstacleLayer;
    public LayerMask EnemyLayer;

    [Header("Shooting Reference")]
    public Transform shootFrom; // Used for detecting hits
    public GameObject BulletImpact;

    [Header("Impact Offset")]
    public float impactOffset = 0.01f;

    [Header("Damage")]
    public float Damage_Point = 10;

    [Header("Animation")]
    public Animator animator; // Reference to the Animator component
    private bool isReloading = false; // Flag to track if the gun is reloading
    private bool isSwinging = false;  // Flag to track if the axe is swinging

    [Header("Swing Settings")]
    public float swingRange = 2f;  // How far the axe can reach
    public float swingCooldown = 1f; // Cooldown between swings
    private float nextSwingTime = 0f;

    public void Shoot()
    {
        if (isReloading || isSwinging || Time.time < nextSwingTime)
        {
            Debug.Log("Cannot swing now.");
            return;
        }

        Debug.Log("Swinging Axe!");
        isSwinging = true;
        animator.Play("Swing", 0, 0f); 

        // Check for enemies in range
        Ray ray = new Ray(shootFrom.position, shootFrom.forward);
        RaycastHit hit;

        // Check for obstacles first
        if (Physics.Raycast(ray, out hit, swingRange, obstacleLayer))
        {
            Debug.Log("Hit obstacle: " + hit.collider.name);
            Vector3 impactPosition = hit.point + hit.normal * impactOffset;
            Instantiate(BulletImpact, impactPosition, Quaternion.LookRotation(hit.normal));
        }

        // Check for enemies
        if (Physics.Raycast(ray, out hit, swingRange, EnemyLayer))
        {
            CollisonEnemy enemyScript = hit.collider.GetComponent<CollisonEnemy>();
            if (enemyScript != null)
            {
                enemyScript.Hit(Damage_Point);
                Debug.Log("Hit enemy: " + hit.collider.name);
            }
        }

        nextSwingTime = Time.time + swingCooldown; // Set cooldown for next swing
        isSwinging = false;
    }

    public void Reload()
    {
        Debug.Log("Reloading not applicable for axe."); // Not applicable for melee weapon
    }
    public void OnEnable()
    {
        
    }

    private void FixedUpdate()
    {
        // Any axe-specific logic for FixedUpdate can go here
    }

    public float GetAmmo()
    {
        return 0; // No ammo for the axe
    }
    public float GetMaxAmmo()
    {
        return 0;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public void Scope()
    {
        Debug.Log("NO LOL"); // Axes typically don't have a scope
    }

    public bool isScopeing1()
    {
        return false; // Axes don't scope, return false
    }
}

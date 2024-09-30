using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sniper : MonoBehaviour, IGun
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
    
    [Header("Ammo")]
    public float Ammo = 6; 
    public float maxAmmo = 6;
    [Header("Reload Settings")]
    public float ammoInsertDelay = 0.5f; // Delay between each ammo insertion

    [Header("Animation")]
    public Animator animator;
    [Header("Reference")]
    public GameObject scope;
    public Image spriteRenderer;
    private bool isReloading = false;
    public bool isScopeing = false;


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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstacleLayer))
        {
            Debug.Log("Hit object: " + hit.collider.name);
            Vector3 impactPosition = hit.point + hit.normal * impactOffset;
            Instantiate(BulletImpact, impactPosition, Quaternion.LookRotation(hit.normal));
        }
        // Check for enemies
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, EnemyLayer))
        {
            Enemy enemyScript = hit.collider.GetComponent<Enemy>();
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
        isReloading = true; 
        StartCoroutine(ReloadCoroutine());
    }
}


private void FixedUpdate()
{
    if (Ammo == 0 && !isReloading)
    {
        Reload();
        isReloading = true; 
    }
    if (Ammo < 0)
    {
        Ammo = 0;
    }
}

private IEnumerator ReloadCoroutine()
{
    while (Ammo < maxAmmo)
    {
        animator.Play("Sniper", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("Inserting ammo...");
        animator.Play("Sniper Reload", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        animator.Play("Sniper", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Ammo += 1;
    }

    // Exit reload phase
    Exitreload1();
}


    private void Exitreload1()
{
    // End of reloading, return to the default state (phase 1: default gun look)
    animator.SetTrigger("ReloadEnd");
    // Set reloading flag to false
    if (Ammo == maxAmmo)
    {
    isReloading = false;
    }
}
    public void Scope()
    {
        
        if(!isScopeing && !isReloading)
        {
            scope.gameObject.SetActive(true);
            isScopeing = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false; // Enable the scope image
            }
        }
        else if(isScopeing && !isReloading)
        {
            scope.gameObject.SetActive(false);
            isScopeing = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true; // Enable the scope image
            }
        }
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
    public bool isScopeing1()
    {
        return isScopeing;
    }
}

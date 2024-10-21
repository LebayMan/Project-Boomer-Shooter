using System.Collections;
using UnityEngine;

public class GranadeLauncher : MonoBehaviour, IGun
{
    [Header("Gun Settings")]
    public bool Auto;

    [Header("Layer")]
    public LayerMask obstacleLayer;
    public LayerMask EnemyLayer;

    [Header("Shooting Reference")]
    public Transform shootFrom;
    
    [Header("Grenade Settings")]
    public GameObject grenadePrefab; // Grenade prefab to spawn
    public float launchForce = 20f;  // Force applied when launching the grenade
    public float damage = 10f;

    [Header("Ammo")]
    public float Ammo = 6; 
    public float maxAmmo = 6;

    [Header("Reload Settings")]
    public float ammoInsertDelay = 0.5f;

    [Header("Animation")]
    public Animator animator;

    private bool isReloading = false;

    public void Shoot()
    {
        if (isReloading)
        {
            Debug.Log("Cannot shoot while reloading.");
            return;
        }

        if (Ammo > 0)
        {
            Quaternion rotation = shootFrom.rotation * Quaternion.Euler(90f, 0f, 0f);
            
            GameObject grenade = Instantiate(grenadePrefab, shootFrom.position, rotation);

            // Apply force to the grenade to launch it
            Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
            if (grenadeRb != null)
            {
                grenadeRb.AddForce(shootFrom.forward * launchForce, ForceMode.VelocityChange);
            }

            // Reduce ammo count
            Ammo -= 1;

            // Ensure ammo doesn't go below zero
            if (Ammo < 0)
            {
                Ammo = 0;
            }
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
            animator.Play("Granade Luncher", 0, 0f);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Debug.Log("Inserting ammo...");
            animator.Play("Granade Luncher IN", 0, 0f);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            animator.Play("Granade Luncher", 0, 0f);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            Ammo += 1;
        }

        Exitreload1();
    }

    private void Exitreload1()
    {
        animator.SetTrigger("ReloadEnd");
        if (Ammo == maxAmmo)
        {
            isReloading = false;
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
        return false;
    }
    public void Scope()
    {
        Debug.Log("NO LOL");
    }

    public bool auto()
    {
        return Auto;
    }
}

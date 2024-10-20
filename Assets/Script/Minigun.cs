using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Minigun : MonoBehaviour, IGun
{
    [Header("Gun Settings")]
    public bool Auto;
    [Header("Ref")]
    public GunManager gunManager;
    [Header("Layer")]
    public LayerMask obstacleLayer;
    public LayerMask enemyLayer;

    [Header("Shooting Reference")]
    public Transform shootFrom;
    public GameObject bulletImpact;

    [Header("Impact Offset")]
    public float impactOffset = 0.01f;

    [Header("Damage")]
    public float damagePoint = 10f;
    public float maxRayDistance = 100f;

    [Header("Ammo")]
    public float ammo = 6f;
    public float maxAmmo = 6f;

    [Header("Shooting")]
    public float fireRate = 0.1f; // Time between each shot
    public float cooldownTime = 2f; // Time to cool down after shooting
    private bool isCoolingDown = false; // Flag for cooldown
    private float nextTimeToFire = 0f;

    [Header("Animation")]
    public Animator animator; // Reference to the Animator component
    private bool isReloading = false; 
    private bool animato = false;

    private void Update()
    {
        animatorShooting();
        if (isCoolingDown)
        {
            Debug.Log("Minigun is cooling down...");
            return;
        }

        if (ammo == 0 && !isReloading && !animato)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        
        if (isReloading || isCoolingDown)
        {
            Debug.Log("Cannot shoot while reloading or cooling down.");
            return;
        }

        if (Time.time < nextTimeToFire)
        {
            return; // Fire rate limit
        }

        shootFrom = Camera.main.transform;

        if (ammo > 0)
        {
            Ray ray = new Ray(shootFrom.position, shootFrom.forward);
            RaycastHit hit;
            
            // Check for obstacles
            if (Physics.Raycast(ray, out hit, maxRayDistance, obstacleLayer))
            {
                Debug.Log("Hit object: " + hit.collider.name);
                Vector3 impactPosition = hit.point + hit.normal * impactOffset;
                Instantiate(bulletImpact, impactPosition, Quaternion.LookRotation(hit.normal));
            }

            // Check for enemies
            if (Physics.Raycast(ray, out hit, maxRayDistance, enemyLayer))
            {
                CollisonEnemy enemyScript = hit.collider.GetComponent<CollisonEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.Hit(damagePoint);
                }
            }

            ammo -= 1;
            nextTimeToFire = Time.time + fireRate; // Set the next fire time

            if (ammo == 0)
            {
                
                StartCooldown();
            }
        }
        else
        {
            
            Debug.Log("Out of Ammo!");
        }
        
    }

    public void animatorShooting()
    {
        if(gunManager.isShooting)
        {
            if(!animato)
            {
            animator.Play("Minigun_Tembak", 0, 0f);
            animato = true;
            }
        }
        else if(!gunManager.isShooting)
        {
            if(animato)
            {
            Debug.Log("UDH SELESAI PENCET");
            animator.SetTrigger("SelesaiPencet");
            animato = false;
            }
        }
    }
    public void Reload()
    {
        if (ammo < maxAmmo && !isReloading)
        {
            isReloading = true; // Set reloading flag to true
            if (animator != null)
            {
                animator.SetTrigger("Reload"); // Trigger the reload animation
            }
            StartCoroutine(ReloadCoroutine());
        }
    }


    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        ammo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded");
    }

    private void StartCooldown()
    {
        isCoolingDown = true;
        Invoke(nameof(EndCooldown), cooldownTime);
    }

    private void EndCooldown()
    {
        isCoolingDown = false;
        Debug.Log("Minigun cooled down and ready to fire again!");
    }

    public float GetAmmo()
    {
        return ammo;
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
        return false;
    }
    public bool auto()
    {
        return Auto;
    }
}

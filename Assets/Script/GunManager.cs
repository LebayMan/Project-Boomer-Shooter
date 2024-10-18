using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private IGun currentGun;
    public bool isShooting = false;

    public void EquipGun(IGun newGun)
    {
        currentGun = newGun;
    }

    public void ShootGun()
    {
        if (currentGun != null)
        {
            currentGun.Shoot();
        }
    }
    public void StartShooting()
    {
        isShooting = true; // Start shooting when the button is pressed
    }

    public void StopShooting()
    {
        isShooting = false; // Stop shooting when the button is released
    }
    private void Update()
    {
        if (isShooting)
        {
            ShootGun();
        }
    }
    public void ReloadGun()
    {
        if (currentGun != null)
        {
            currentGun.Reload();
        }
        
    }

    public float GetCurrentAmmo()
    {
        return currentGun != null ? currentGun.GetAmmo() : 0;
    }
    
    public float GetCurrentMaxAmmo()
    {
        return currentGun != null ? currentGun.GetMaxAmmo() : 0;
    }

    public void Scope()
    {
        if (currentGun != null)
        {
            currentGun.Scope();
        }
    }
    public bool scopbool()
    {
        return currentGun.isScopeing1();
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    private IGun currentGun;

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
}


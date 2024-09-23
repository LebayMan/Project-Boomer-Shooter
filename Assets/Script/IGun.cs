using UnityEngine;

public interface IGun
{
    void Shoot();
    void Reload(); // Add a Reload method here
    float GetAmmo(); // Optional: To access current ammo from other scripts
    
    bool IsReloading();
}
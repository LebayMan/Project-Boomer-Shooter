using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons; // Array to hold the weapon GameObjects
    private int currentWeaponIndex = 0;
    private GunManager gunManager;
    private IGun currentGun;
    private void Start()
    {
        gunManager = FindObjectOfType<GunManager>();
        // Activate the starting weapon
        ActivateWeapon(currentWeaponIndex);
    }

public void OnSwitchWeapon(InputAction.CallbackContext context)
{
    if (currentGun == null)
    {
        Debug.LogWarning("No gun equipped yet!");
        return;
    }

    // Check if the current gun is reloading
    if (currentGun.IsReloading())
    {
        Debug.Log("Cannot switch weapons while reloading.");
        return;
    }

    if (context.performed)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();
        float scrollDirection = scrollValue.y;

        if (scrollDirection > 0)
        {
            NextWeapon();
        }
        else if (scrollDirection < 0)
        {
            PreviousWeapon();
        }
    }
}


    private void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        ActivateWeapon(currentWeaponIndex);
    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        ActivateWeapon(currentWeaponIndex);
    }

    private void ActivateWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // Update the equipped gun in the GunManager
        currentGun = weapons[index].GetComponent<IGun>(); // Assign currentGun here
        if (gunManager != null && currentGun != null)
        {
            gunManager.EquipGun(currentGun);
        }
    }

}

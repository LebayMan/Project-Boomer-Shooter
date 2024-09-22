using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons; // Array to hold the weapon GameObjects
    private int currentWeaponIndex = 0;
    private GunManager gunManager;

    private void Start()
    {
        gunManager = FindObjectOfType<GunManager>();
        // Activate the starting weapon
        ActivateWeapon(currentWeaponIndex);
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 scrollValue = context.ReadValue<Vector2>();
            float scrollDirection = scrollValue.y;
            if (scrollDirection > 0)
            {
                Debug.Log("MAWFNAWF");
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
        IGun equippedGun = weapons[index].GetComponent<IGun>();
        if (gunManager != null && equippedGun != null)
        {
            gunManager.EquipGun(equippedGun);
        }
    }
}

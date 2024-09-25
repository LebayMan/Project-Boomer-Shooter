using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private SpriteRenderer theSR;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = Player.instance.transform.position - transform.position;

        // Ensure the direction is not zero to avoid division by zero
        if (directionToPlayer != Vector3.zero)
        {
            // Calculate the rotation to face the player, ensuring the sprite is always upright
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            // Lock the X rotation to 0 (or any other value you want) while keeping the Z and Y rotations
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
    }
}

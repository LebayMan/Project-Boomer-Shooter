using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCollison : MonoBehaviour
{
    private void OnEnable()
    {
        // Register collision events
        Debug.Log("BeatCollision script enabled.");
    }

    private void OnDisable()
    {
        // Unregister collision events (if needed)
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the GameObject is enabled and the collided object has an Enemy script
        if (gameObject.activeSelf)
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Hit(50);  // Call Hit(10) on the enemy
                Debug.Log("Hit enemy with 10 damage.");
            }
        }
    }
}

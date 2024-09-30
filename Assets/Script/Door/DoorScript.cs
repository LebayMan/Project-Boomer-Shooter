using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator doorAnimator; // Reference to the Animator component

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this GameObject
        doorAnimator = GetComponent<Animator>();
    }

    // Triggered when another collider enters this GameObject's collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the "In" animation when the player enters the collider
            doorAnimator.SetTrigger("In");
        }
    }

    // Triggered when another collider exits this GameObject's collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the "Out" animation when the player exits the collider
            doorAnimator.SetTrigger("Out");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonDoor : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator component
    private bool masuk;



    // Triggered when another collider enters this GameObject's collider
    private void OnTriggerEnter(Collider other)
    {
        if(!masuk)
        {
        doorAnimator.SetTrigger("IN");
        masuk = true;
        Debug.Log("pintu terbuka");
        }
    }

    // Triggered when another collider exits this GameObject's collider
    private void OnTriggerExit(Collider other)
    {
    doorAnimator.SetTrigger("Out");
    masuk = false;
    }
}

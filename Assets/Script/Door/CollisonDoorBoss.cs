using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonDoorBoss : MonoBehaviour
{
    [Header("Reference")]
    public Animator doorAnimator; 
    public GameObject UI;
    private bool masuk;



    // Triggered when another collider enters this GameObject's collider
    private void OnTriggerEnter(Collider other)
    {
        if(!masuk)
        {
        doorAnimator.SetTrigger("IN");
        masuk = true;
        Debug.Log("pintu terbuka");
        UI.SetActive(true);
        }
    }

    // Triggered when another collider exits this GameObject's collider
    private void OnTriggerExit(Collider other)
    {
    doorAnimator.SetTrigger("Out");
    masuk = false;
    }
}

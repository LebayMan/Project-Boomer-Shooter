using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets: MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if(hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
        }
        Destroy(gameObject);
    }
}

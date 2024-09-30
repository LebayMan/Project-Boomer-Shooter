using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets: MonoBehaviour
{
    private Collider bulletCollider;
    private void Start()
    {
        bulletCollider = GetComponent<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if(hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}

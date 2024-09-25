using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Info")]
    public Animator animator; // Reference to the Animator component
    private float lifetime;
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Ensure an Animator is attached and an animation is playing
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            // Get the current animation clip length
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                // Assuming you want to use the first clip's length as the lifetime
                lifetime = clips[0].length;
            }
        }

        // Destroy the bullet after the animation duration
        Destroy(gameObject, lifetime);
    }
}

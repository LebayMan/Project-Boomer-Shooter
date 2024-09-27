using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoping : MonoBehaviour
{
    [Header("Reference")]
    public Camera mainCamera;  // Reference to the main camera
    [Header("Camera Setting")]
    public float zoomFOV = 20f; // Field of view when zoomed in (adjust as needed)
    public float normalFOV = 60f; // Default field of view
    private bool isScoping = false;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically find the main camera if not set
        }
    }

    void OnEnable()
    {
        // Set the camera's FOV to zoomFOV instantly
        mainCamera.fieldOfView = zoomFOV;
        isScoping = true; // Set scoping state to true
    }

    void OnDisable()
    {
        // Set the camera's FOV back to normal instantly
        mainCamera.fieldOfView = normalFOV;
        isScoping = false; // Set scoping state to false
    }
}

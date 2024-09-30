using UnityEngine;
using System.Collections;

public class ShotgunRecoil : MonoBehaviour
{
    public float recoilAmount = 5f;    // How much the gun moves backward
    public float recoilSpeed = 10f;    // How fast the gun returns to its original position
    public float rotationRecoil = 2f;  // How much the gun rotates during recoil
    public Transform gunTransform;     // The transform of your shotgun object
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isRecoiling = false;

    private void Start()
    {
        if (gunTransform == null)
        {
            gunTransform = transform;
        }
        originalPosition = gunTransform.localPosition;
        originalRotation = gunTransform.localRotation;
    }

    public void ApplyRecoil()
    {
        if (!isRecoiling)
        {
            isRecoiling = true;
            StartCoroutine(RecoilRoutine());
        }
    }

    private IEnumerator RecoilRoutine()
    {
        // Move the gun back and rotate for the recoil effect
        Vector3 recoilPosition = originalPosition + Vector3.back * recoilAmount;
        Quaternion recoilRotation = originalRotation * Quaternion.Euler(-rotationRecoil, 0, 0);

        // Smoothly move to recoil position
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, recoilPosition, Time.deltaTime * recoilSpeed);
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, recoilRotation, Time.deltaTime * recoilSpeed);
            elapsed += Time.deltaTime * recoilSpeed;
            yield return null;
        }

        // Return gun to the original position and rotation
        elapsed = 0f;
        while (elapsed < 1f)
        {
            gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, originalRotation, Time.deltaTime * recoilSpeed);
            elapsed += Time.deltaTime * recoilSpeed;
            yield return null;
        }

        isRecoiling = false;
    }
}

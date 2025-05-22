using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtMouse : MonoBehaviour {
    [Header("Rotation Speed Settings")]
    public float maxSmoothSpeed = 7.0f;
    public float minSmoothSpeed = 0.02f;
    public float maxMouseDelta = 50f;

    [Header("Rotation Boundaries (degrees)")]
    public float minX = -3.5f;
    public float maxX = 3.5f;
    public float minY = -5.5f;
    public float maxY = 5.5f;

    [Header("Easing Settings")]
    public float slowdownDelay = 0.8f;
    public float easingPowerIn = 3f;
    public float easingPowerOut = 4f;

    [Header("Stop Threshold & Fade")]
    public float stopThreshold = 0.01f;
    public float fadeOutDuration = 3.0f;

    [Header("Ease-Out Multiplier (Not Used Here)")]
    public float maxEaseOutMultiplier = 1.0f; // No scaling beyond boundaries

    private Camera mainCamera;
    private Quaternion originalRotation;

    private Vector3 prevMousePos;
    private float lastMoveTime;

    private float fadeOutTimer = 0f;
    private bool isFadingOut = false;
    private float lerpSpeedBeforeFade = 0f;

    void Start() {
        mainCamera = Camera.main;
        originalRotation = Quaternion.Euler(58f, 0f, 0f);
        transform.rotation = originalRotation;

        prevMousePos = Input.mousePosition;
        lastMoveTime = Time.time;
    }

    void Update() {
        Vector3 currMousePos = Input.mousePosition;
        Vector3 mouseDelta = currMousePos - prevMousePos;
        float mouseMoveDist = mouseDelta.magnitude;

        bool isMoving = mouseMoveDist > 0f;
        if (isMoving) {
            lastMoveTime = Time.time;
            isFadingOut = false;
            fadeOutTimer = 0f;
        }

        float timeSinceMove = Time.time - lastMoveTime;

        float slowDownT = Mathf.Clamp01(timeSinceMove / slowdownDelay);
        float easeOutT = 1f - Mathf.Pow(1f - slowDownT, easingPowerOut);

        float speedT = Mathf.Clamp01(mouseMoveDist / maxMouseDelta);
        float easeInT = Mathf.Pow(speedT, easingPowerIn);

        float lerpSpeed = Mathf.Lerp(minSmoothSpeed, maxSmoothSpeed, easeInT) * (1f - easeOutT);

        if (!isMoving && lerpSpeed < stopThreshold) {
            if (!isFadingOut) {
                isFadingOut = true;
                fadeOutTimer = 0f;
                lerpSpeedBeforeFade = lerpSpeed;
            } else {
                fadeOutTimer += Time.deltaTime;
                float fadeT = Mathf.Clamp01(fadeOutTimer / fadeOutDuration);
                float smoothFadeT = fadeT * fadeT * (3f - 2f * fadeT);
                lerpSpeed = Mathf.Lerp(lerpSpeedBeforeFade, 0f, smoothFadeT);
            }
        } else {
            isFadingOut = false;
            fadeOutTimer = 0f;
        }

        float lookDistance = 10f;
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(currMousePos.x, currMousePos.y, lookDistance));
        Vector3 direction = worldPoint - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Quaternion offset = Quaternion.Inverse(originalRotation) * targetRotation;
        Vector3 offsetEuler = offset.eulerAngles;
        offsetEuler.x = NormalizeAngle(offsetEuler.x);
        offsetEuler.y = NormalizeAngle(offsetEuler.y);

        // Clamp rotation strictly within boundaries (no multiplier)
        offsetEuler.x = Mathf.Clamp(offsetEuler.x, minX, maxX);
        offsetEuler.y = Mathf.Clamp(offsetEuler.y, minY, maxY);
        offsetEuler.z = 0f;

        Quaternion clampedOffset = Quaternion.Euler(offsetEuler);
        Quaternion finalTargetRotation = originalRotation * clampedOffset;

        if (lerpSpeed > 0f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, finalTargetRotation, lerpSpeed * Time.deltaTime);
        }

        prevMousePos = currMousePos;
    }

    private float NormalizeAngle(float angle) {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}

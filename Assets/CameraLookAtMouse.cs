using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtMouse : MonoBehaviour {
    [Header("Panning Settings")]
    public float rotationSmoothSpeed = 8f;
    public float maxYaw = 15f;    // Left-right max angle
    public float maxPitch = 15f;  // Up-down max angle

    [Header("Rectangular Dead Zone (pixels)")]
    public float deadZoneWidth = 1200f;  // Width of dead zone rectangle
    public float deadZoneHeight = 500f; // Height of dead zone rectangle

    [Header("Axis Inversion")]
    public bool invertX = false;
    public bool invertY = false;

    private Quaternion originalRotation;
    private Vector2 currentRotationOffset;

    void Start() {
        originalRotation = Quaternion.Euler(58f, 0f, 0f);
        transform.rotation = originalRotation;
        currentRotationOffset = Vector2.zero;
    }

    void LateUpdate() {
    Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Vector2 mouseOffset = (Vector2)Input.mousePosition - screenCenter;

    bool insideDeadZone = Mathf.Abs(mouseOffset.x) < deadZoneWidth / 2f &&
                        Mathf.Abs(mouseOffset.y) < deadZoneHeight / 2f;

    if (insideDeadZone) {
        // Faster ease-out when snapping back
        float t = rotationSmoothSpeed * 2.5f * Time.deltaTime;  // 4x speed multiplier
        float smoothT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));

        currentRotationOffset = Vector2.Lerp(currentRotationOffset, Vector2.zero, smoothT);
    } else {
        float xDistOutside = 0f;
        if (Mathf.Abs(mouseOffset.x) > deadZoneWidth / 2f)
            xDistOutside = (Mathf.Abs(mouseOffset.x) - deadZoneWidth / 2f) / (screenCenter.x - deadZoneWidth / 2f);

        float yDistOutside = 0f;
        if (Mathf.Abs(mouseOffset.y) > deadZoneHeight / 2f)
            yDistOutside = (Mathf.Abs(mouseOffset.y) - deadZoneHeight / 2f) / (screenCenter.y - deadZoneHeight / 2f);

        xDistOutside = Mathf.Clamp01(xDistOutside);
        yDistOutside = Mathf.Clamp01(yDistOutside);

        Vector2 normalizedOffset = new Vector2(
            Mathf.Clamp(mouseOffset.x / screenCenter.x, -1f, 1f),
            Mathf.Clamp(mouseOffset.y / screenCenter.y, -1f, 1f)
        );

        if (invertX) normalizedOffset.x = -1f;
        if (invertY) normalizedOffset.y= -1f;

        float targetYaw = normalizedOffset.x * maxYaw;
        float targetPitch = -normalizedOffset.y * maxPitch;

        Vector2 targetRotationOffset = new Vector2(targetYaw, targetPitch);

        float easeFactor = Mathf.Max(xDistOutside, yDistOutside);

        currentRotationOffset = Vector2.Lerp(
            currentRotationOffset,
            targetRotationOffset,
            rotationSmoothSpeed * Time.deltaTime * easeFactor
        );
    }

    Quaternion offsetRotation = Quaternion.Euler(currentRotationOffset.y, currentRotationOffset.x, 0f);
    transform.rotation = originalRotation * offsetRotation;
    }

}
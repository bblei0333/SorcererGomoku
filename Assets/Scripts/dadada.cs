using UnityEngine;

[RequireComponent(typeof(Animation))]
public class LocalAnimation2 : MonoBehaviour
{
    Vector3 initialLocalPos; // Store initial LOCAL position
    bool wasPlaying;

    void Awake()
    {
        initialLocalPos = transform.localPosition; // Use LOCAL position
        wasPlaying = false;
    }

    void LateUpdate()
    {
        if (!GetComponent<Animation>().isPlaying && !wasPlaying)
            return;

        // Reset to initial local position before applying animation
        transform.localPosition = initialLocalPos; 

        wasPlaying = GetComponent<Animation>().isPlaying;
    }
}
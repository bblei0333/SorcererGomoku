using Normal.Realtime;
using UnityEngine;

public class AnimationSyncHelper : MonoBehaviour {
    private RealtimeTransform _rt;
    private Animation _animation;

    void Start() {
        _rt = GetComponent<RealtimeTransform>();
        _animation = GetComponent<Animation>();

        // Disable RealtimeTransform sync during animation
        _animation.Play();
        _rt.syncPosition = false;
        _rt.syncRotation = false;

        // Re-enable sync when animation finishes
        Invoke(nameof(ReenableSync), _animation.clip.length);
    }

    void ReenableSync() {
        _rt.syncPosition = true;
        _rt.syncRotation = true;
    }
}
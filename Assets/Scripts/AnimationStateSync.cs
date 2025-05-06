using Normal.Realtime;
using UnityEngine;

public class AnimationStateSync : RealtimeComponent<AnimationStateModel>
{
    private Animation _animation;
    
    private void Awake()
    {
        _animation = GetComponentInChildren<Animation>();
    }
    
    protected override void OnRealtimeModelReplaced(AnimationStateModel previousModel, AnimationStateModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.animationStateDidChange -= OnAnimationStateChanged;
        }
        
        if (currentModel != null)
        {
            currentModel.animationStateDidChange += OnAnimationStateChanged;
            
            // Apply the current state immediately
            UpdateAnimationState(currentModel.animationState);
        }
    }
    
    private void OnAnimationStateChanged(AnimationStateModel model, int value)
    {
        UpdateAnimationState(value);
    }
    
    private void UpdateAnimationState(int state)
    {
        if (_animation == null) return;
        
        switch (state)
        {
            case 1:
                _animation.Play("FlipUp");
                break;
            case 2:
                _animation.Play("FlipDown");
                break;
            case 3:
                _animation.Play("Piece Place");
                break;
            default:
                // No animation or stop current
                break;
        }
    }
    
    public void PlayPiecePlacement(){
        model.animationState = 3;
    }
    public void PlayFlipUp()
    {
        model.animationState = 1;
    }
    
    public void PlayFlipDown()
    {
        model.animationState = 2;
    }
}
using Normal.Realtime;

[RealtimeModel]
public partial class AnimationStateModel
{
    [RealtimeProperty(1, true, true)]
    private int _animationState;
}
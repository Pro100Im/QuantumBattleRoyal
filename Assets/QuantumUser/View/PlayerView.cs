using Quantum;
using UnityEngine;

public unsafe class PlayerView : QuantumEntityViewComponent
{
    [SerializeField] private Animator _animator;

    private readonly int _moveXHash = Animator.StringToHash("moveX");
    private readonly int _moveZHash = Animator.StringToHash("moveZ");

    public override void OnUpdateView()
    {
        base.OnUpdateView();

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        var plaerRef = PredictedFrame.Get<PlayerLink>(EntityRef).Player;
        var input = PredictedFrame.GetPlayerInput(plaerRef);
        var kcc = PredictedFrame.Get<KCC>(EntityRef);
        var velocity = kcc.Velocity;

        _animator.SetFloat(_moveXHash, velocity.X.AsFloat);
        _animator.SetFloat(_moveZHash, velocity.Y.AsFloat);
    }
}

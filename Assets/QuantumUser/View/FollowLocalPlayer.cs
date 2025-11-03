using UnityEngine;
using Quantum;

public class FollowLocalPlayer : QuantumViewComponent<CameraViewContext>
{
    public override void OnActivate(Frame frame)
    {
        base.OnActivate(frame);

        if (!frame.TryGet<PlayerLink>(_entityView.EntityRef, out var playerLink))
            return;

        if (!_game.PlayerIsLocal(playerLink.Player))
            return;

        ViewContext.VirtualCamera.Follow = _entityView.transform;
    }
}

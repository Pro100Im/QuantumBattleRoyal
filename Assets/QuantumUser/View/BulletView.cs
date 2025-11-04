using Quantum;
using UnityEngine;

public class BulletView : QuantumEntityViewComponent
{
    [SerializeField] private Transform _bulletGraphics;

    public override void OnActivate(Frame f)
    {
        base.OnActivate(f);

        var bullet = PredictedFrame.Get<Bullet>(EntityRef);
        var localPosition = _bulletGraphics.localPosition;
        
        localPosition.y = bullet.HeightOffset.AsFloat;

        _bulletGraphics.localPosition = localPosition;
    }
}

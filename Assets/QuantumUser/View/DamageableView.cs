using DG.Tweening;
using Quantum;
using UnityEngine;
using UnityEngine.UI;

public class DamageableView : QuantumEntityViewComponent
{
    [SerializeField] private Image _health;

    private Tween _tween;

    public override void OnActivate(Frame frame)
    {
        base.OnActivate(frame);

        QuantumEvent.Subscribe<EventDamageableHealthUpdate>(this, Handler);
    }

    private void Handler(EventDamageableHealthUpdate callback)
    {
        if (EntityRef != callback.entity)
            return;

        var precentage = (callback.currentHealth / callback.maxHealth).AsFloat;

        _tween?.Kill();
        _health.DOFillAmount(precentage, .1f);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        QuantumEvent.UnsubscribeListener(this);
    }
}

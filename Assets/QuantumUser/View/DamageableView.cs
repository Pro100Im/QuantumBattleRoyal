using Quantum;
using UnityEngine;
using UnityEngine.UI;

public class DamageableView : QuantumEntityViewComponent
{
    [SerializeField] private Image _health;

    public override void OnActivate(Frame frame)
    {
        base.OnActivate(frame);

        QuantumEvent.Subscribe<EventDamageableHit>(this, Handler);
    }

    private void Handler(EventDamageableHit callback)
    {
        if (EntityRef != callback.entity)
            return;

        var precentage = (callback.currentHealth / callback.maxHealth).AsFloat;

        _health.fillAmount = precentage;
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        QuantumEvent.UnsubscribeListener(this);
    }
}

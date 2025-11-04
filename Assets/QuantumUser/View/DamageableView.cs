using Photon.Deterministic;
using Quantum;
using System;
using System.Collections;
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

        float precentage = (callback.currentHealth / callback.maxHealth).AsFloat;
        _health.fillAmount = precentage;
        //StartCoroutine(ChangeHp(callback.maxHealth, callback.currentHealth));
    }

    //private IEnumerator ChangeHp(FP maxHealth, FP currentHealth)
    //{
    //    float precentage = (currentHealth / maxHealth).AsFloat;

    //    Log.Info($"precentage {precentage}");

    //    while(Mathf.Approximately(precentage, _health.fillAmount))
    //    {
    //        _health.fillAmount = Mathf.Lerp(precentage, _health.fillAmount, 0.1f);
    //        yield return null;
    //    }
    //}

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        QuantumEvent.UnsubscribeListener(this);
    }
}

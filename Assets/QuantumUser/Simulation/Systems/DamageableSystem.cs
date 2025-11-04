namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class DamageableSystem : SystemSignalsOnly, ISignalOnComponentAdded<Damageable>, ISignalDamageableHit, ISignalDamageableHealthRestored
    {
        public unsafe void OnAdded(Frame f, EntityRef entity, Damageable* component)
        {
            var damageableData = f.FindAsset(component->DamageableData);

            component->Health = damageableData.MaxHealth;
        }

        public unsafe void DamageableHit(Frame f, EntityRef vicitim, EntityRef damager, FP damage, Damageable* damageable)
        {
            var damageableBaseAsset = f.FindAsset(damageable->DamageableData);

            damageableBaseAsset.TakeDamage(f, damager, vicitim, damage, damageable);
        }

        public unsafe void DamageableHealthRestored(Frame f, EntityRef entity, Damageable* damageable)
        {
            var health = f.FindAsset(damageable->DamageableData).MaxHealth;

            damageable->Health = health;

            Log.Info($"health {health}");

            f.Events.DamageableHealthUpdate(entity, health, damageable->Health);
        }
    }
}

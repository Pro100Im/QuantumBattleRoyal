namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class DamageableSystem : SystemSignalsOnly, ISignalOnComponentAdded<Damageable>, ISignalDamageableHit
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
    }
}

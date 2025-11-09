namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class DamageableSystem : SystemMainThreadFilter<DamageableSystem.Filter>, ISignalOnComponentAdded<Damageable>, ISignalDamageableHit, ISignalDamageableHealthRestored
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Damageable* Damageable;
        }

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

            f.Events.DamageableHealthUpdate(entity, health, damageable->Health);
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var entity = filter.Entity;

            if (!f.TryGet<PlayerLink>(entity, out _))
                return;

            var shrinkingCircle = f.GetSingleton<ShrinkingCircle>();

            if (CheckPlayerIsInsideCircle(f, filter, shrinkingCircle))
            {
                var damageableAsset = f.FindAsset(filter.Damageable->DamageableData);
                var damage = f.FindAsset(shrinkingCircle.Asset).DamageDealingPerSecond;

                damageableAsset.TakeDamage(f, entity, entity, damage * f.DeltaTime, filter.Damageable);
            }
        }

        private bool CheckPlayerIsInsideCircle(Frame f, Filter filter, ShrinkingCircle shrinkingCircle)
        {
            var transform = f.Get<Transform2D>(filter.Entity);

            return FPVector2.Distance(transform.Position, shrinkingCircle.Position) >= shrinkingCircle.CurrentRadius / 2;
        }
    }
}

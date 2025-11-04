using Photon.Deterministic;

namespace Quantum
{
    public unsafe class DefaultDamageableAsset : DamageableBaseAsset
    {
        public override void TakeDamage(Frame f, EntityRef source, EntityRef victim, FP damage, Damageable* damageable)
        {
            damageable->Health -= damage;

            if (damageable->Health <= 0)
            {
                f.Destroy(victim);

                return;
            }

            f.Events.DamageableHit(victim, MaxHealth, damageable->Health);
        }
    }
}

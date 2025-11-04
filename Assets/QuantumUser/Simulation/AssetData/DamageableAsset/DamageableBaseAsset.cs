using Photon.Deterministic;

namespace Quantum
{
    public abstract unsafe class DamageableBaseAsset : AssetObject
    {
        public FP MaxHealth;

        public abstract void TakeDamage(Frame f, EntityRef source, EntityRef victim, FP damage, Damageable* damageable);
    }
}

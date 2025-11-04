namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class HealthPickUpAsset : PickUpAsset
    {
        public override void PickUpItem(Frame f, EntityRef entityBeingPickedUp, EntityRef entityPickingUp)
        {
            if (f.Unsafe.TryGetPointer<Damageable>(entityPickingUp, out var damageable))
            {
                f.Signals.DamageableHealthRestored(entityPickingUp, damageable);
            }

            f.Destroy(entityBeingPickedUp);
        }
    }
}

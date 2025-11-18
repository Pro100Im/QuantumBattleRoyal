using Photon.Deterministic;
using System;

namespace Quantum
{
    public unsafe class DefaultDamageableAsset : DamageableBaseAsset
    {
        public override void TakeDamage(Frame f, EntityRef source, EntityRef victim, FP damage, Damageable* damageable)
        {
            damageable->Health -= damage;

            if (damageable->Health <= 0)
            {
                DropLoot(f, victim);

                f.Destroy(victim);
                f.Signals.PlayerKilled();

                return;
            }

            f.Events.DamageableHealthUpdate(victim, MaxHealth, damageable->Health);
        }

        private unsafe void DropLoot(Frame f, EntityRef victim)
        {
            var transform = f.Get<Transform2D>(victim);
            var healthLoot = f.Create(f.SimulationConfig.HealthPickupItem);

            f.Unsafe.GetPointer<Transform2D>(healthLoot)->Position = transform.Position + transform.Right * 2;

            if (!f.TryGet<Weapon>(victim, out var weapon))
                return;

            var weaponAsset = f.FindAsset(weapon.WeaponData);
            var weaponLoot = f.Create(f.SimulationConfig.GetEntityPrototypeFromWeaponType(weaponAsset.Type));

            f.Unsafe.GetPointer<Transform2D>(weaponLoot)->Position = transform.Position + transform.Left * 2;
        }
    }
}

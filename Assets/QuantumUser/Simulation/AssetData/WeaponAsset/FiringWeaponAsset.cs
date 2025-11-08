using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public unsafe class FiringWeaponAsset : WeaponAsset
    {
        public BulletData BulletData;

        public byte MaxAmmo;

        public override void OnInit(Frame f, EntityRef entity, Weapon* weapon)
        {
            base.OnInit(f, entity, weapon);

            weapon->Ammo = MaxAmmo;
        }

        public override void OnUpdate(Frame f, WeaponSystem.Filter filter)
        {
            base.OnUpdate(f, filter);

            if (filter.Weapon->CoolDownTime > FP._0)
            {
                filter.Weapon->CoolDownTime -= f.DeltaTime;

                return;
            }

            var input = f.GetPlayerInput(filter.PlayerLink->Player);

            if (input->Fire.WasPressed)
            {
                OnFirePressed(f, filter);
            }
            else if(input->Fire.WasReleased)
            {
                OnFireReleased(f, filter);
            }
            else if(input->Fire.IsDown)
            {
                OnFireHeld(f, filter);
            }
        }

        protected void FireWeapon(Frame f, WeaponSystem.Filter filter)
        {
            if(filter.Weapon->Ammo <= 0)
                return;

            filter.Weapon->CoolDownTime = CoolDown;
            filter.Weapon->Ammo--;

            f.Signals.CreateBullet(filter.Entity, this);
        }
    }
}

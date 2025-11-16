using UnityEngine;

namespace Quantum
{
    public unsafe class AmmoPickUpAsset : PickUpAsset
    {
        public override void PickUpItem(Frame f, EntityRef entityBeingPickedUp, EntityRef entityPickingUp)
        {
            var weapon = f.Unsafe.GetPointer<Weapon>(entityPickingUp);
            var weaponAsset = f.FindAsset(weapon->WeaponData);

            if (weaponAsset is not FiringWeaponAsset firingWeaponAsset)
                return;

            weapon->Ammo = firingWeaponAsset.MaxAmmo;

            if (f.TryGet<PlayerLink>(entityPickingUp, out var playerLink))
                f.Events.AmmoChange(playerLink.Player, entityPickingUp, weapon->Ammo);

            f.Destroy(entityBeingPickedUp);
        }
    }
}

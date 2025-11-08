using UnityEngine;

namespace Quantum
{
    public class SemiAutomaticWeaponAsset : FiringWeaponAsset
    {
        public override void OnFirePressed(Frame f, WeaponSystem.Filter filter)
        {
            base.OnFirePressed(f, filter);

            FireWeapon(f, filter);
        }
    }
}

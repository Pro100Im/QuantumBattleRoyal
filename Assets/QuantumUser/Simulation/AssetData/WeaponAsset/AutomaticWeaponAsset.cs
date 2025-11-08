using UnityEngine;

namespace Quantum
{
    public class AutomaticWeaponAsset : FiringWeaponAsset
    {
        public override void OnFireHeld(Frame f, WeaponSystem.Filter filter)
        {
            base.OnFireHeld(f, filter);

            FireWeapon(f, filter);
        }
    }
}

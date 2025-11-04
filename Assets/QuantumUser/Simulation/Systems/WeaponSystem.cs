namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Weapon* Weapon;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if(filter.Weapon->CoolDownTime > FP._0)
            {
                filter.Weapon->CoolDownTime -= f.DeltaTime;

                return;
            }

            var input = f.GetPlayerInput(filter.PlayerLink->Player);

            if (input->Fire.WasPressed)
            {
                var weaponData = f.FindAsset<WeaponData>(filter.Weapon->WeaponData);

                filter.Weapon->CoolDownTime = weaponData.CoolDown;

                f.Signals.CreateBullet(filter.Entity, weaponData);
            } 
        }
    }
}

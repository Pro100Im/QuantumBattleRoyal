namespace Quantum
{
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>, ISignalOnComponentAdded<Weapon>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Weapon* Weapon;
        }

        public unsafe void OnAdded(Frame f, EntityRef entity, Weapon* component)
        {
            var data = f.FindAsset(component->WeaponData);

            data.OnInit(f, entity, component);
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var data = f.FindAsset(filter.Weapon->WeaponData);

            data.OnUpdate(f, filter);
        }
    }
}

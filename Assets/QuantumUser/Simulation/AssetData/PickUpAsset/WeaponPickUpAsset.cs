
namespace Quantum
{
    public unsafe class WeaponPickUpAsset : PickUpAsset
    {
        public WeaponAsset WeaponAsset;

        public override void PickUpItem(Frame f, EntityRef entityBeingPickedUp, EntityRef entityPickingUp)
        {
            var playerLink = f.Unsafe.GetPointer<PlayerLink>(entityPickingUp);
            var weapon = f.Unsafe.GetPointer<Weapon>(entityPickingUp);
             
            weapon->WeaponData = WeaponAsset;
            weapon->CoolDownTime = 0;
            weapon->Type = WeaponAsset.Type;

            WeaponAsset.OnInit(f, entityPickingUp, weapon);

            f.Events.WeaponChange(playerLink->Player, WeaponAsset.Type);

            f.Destroy(entityBeingPickedUp);
        }
    }
}

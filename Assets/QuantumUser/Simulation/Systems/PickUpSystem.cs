namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PickUpSystem : SystemMainThreadFilter<PickUpSystem.Filter>, ISignalOnTriggerEnter2D, ISignalOnTriggerExit2D, ISignalOnComponentAdded<PickUpItem>
    {
        public unsafe void OnAdded(Frame f, EntityRef entity, PickUpItem* component)
        {
            var baseAsset = f.FindAsset<PickUpAsset>(component->PickUpAsset);

            component->PickUpTime = baseAsset.PickUpTime;
        }

        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (!f.TryGet<PlayerLink>(info.Entity, out _))      
                return;

            if (!f.Unsafe.TryGetPointer<PickUpItem>(info.Other, out var pickUpItem))
                return;

            if (pickUpItem->PickingUpEntity != EntityRef.None)
                return;

            pickUpItem->PickingUpEntity = info.Entity;
        }

        public void OnTriggerExit2D(Frame f, ExitInfo2D info)
        {
            if (!f.TryGet<PlayerLink>(info.Entity, out _))
                return;

            if (!f.Unsafe.TryGetPointer<PickUpItem>(info.Other, out var pickUpItem))
                return;

            if (pickUpItem->PickingUpEntity != info.Entity)
                return;

            pickUpItem->PickingUpEntity = EntityRef.None;
            pickUpItem->CurrentPickUpTime = 0;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.PickUpItem->PickingUpEntity == EntityRef.None)
                return;

            filter.PickUpItem->CurrentPickUpTime += f.DeltaTime;

            if(filter.PickUpItem->CurrentPickUpTime >= filter.PickUpItem->PickUpTime)
            {
                var baseAsset = f.FindAsset(filter.PickUpItem->PickUpAsset);

                baseAsset.PickUpItem(f, filter.Entity, filter.PickUpItem->PickingUpEntity);
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PickUpItem* PickUpItem;
        }
    }
}

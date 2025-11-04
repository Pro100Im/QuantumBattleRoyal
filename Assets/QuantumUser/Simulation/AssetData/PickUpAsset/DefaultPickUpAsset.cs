namespace Quantum
{
    using Photon.Deterministic;

    public class DefaultPickUpAsset : PickUpAsset
    {
        public override void PickUpItem(Frame f, EntityRef entityBeingPickedUp, EntityRef entityPickingUp)
        {
            f.Destroy(entityBeingPickedUp);
        }
    }
}

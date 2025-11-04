using Photon.Deterministic;

namespace Quantum
{
    public abstract class PickUpAsset : AssetObject
    {
        public FP PickUpTime;

        public abstract void PickUpItem(Frame f, EntityRef entityBeingPickedUp, EntityRef entityPickingUp);
    }
}

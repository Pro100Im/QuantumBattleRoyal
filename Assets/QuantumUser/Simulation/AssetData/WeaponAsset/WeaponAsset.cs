namespace Quantum
{
    using Photon.Deterministic;

    public abstract unsafe class WeaponAsset : AssetObject
    {
        public WeaponType Type;
        public FP CoolDown;
        public FPVector3 Offset;

        public virtual void OnInit(Frame f, EntityRef entity, Weapon* weapon) 
        {

        }

        public virtual void OnUpdate(Frame f, WeaponSystem.Filter filter) 
        {

        }

        public virtual void OnFirePressed(Frame f, WeaponSystem.Filter filter) 
        {

        }

        public virtual void OnFireReleased   (Frame f, WeaponSystem.Filter filter)
        {

        }

        public virtual void OnFireHeld(Frame f, WeaponSystem.Filter filter)
        {

        }
    }
}

namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class BulletAsset : AssetObject
    {
        public FP Duration;
        public EntityPrototype Bullet;
        public FP Damage;
        public FP Speed;

        public virtual void CreateBullet(Frame f, WeaponAsset weaponAsset, EntityRef owner)
        {
            var bulletEntity = f.Create(Bullet);
            var bulletTransform = f.Unsafe.GetPointer<Transform2D>(bulletEntity);
            var ownerTransform = f.Get<Transform2D>(owner);
            var bullet = f.Unsafe.GetPointer<Bullet>(bulletEntity);

            bulletTransform->Position = ownerTransform.Position + weaponAsset.Offset.XZ.Rotate(ownerTransform.Rotation);
            bulletTransform->Rotation = ownerTransform.Rotation;

            bullet->Speed = Speed;
            bullet->Damage = Damage;
            bullet->Time = Duration;
            bullet->Owner = owner;
            bullet->HeightOffset = weaponAsset.Offset.Y;
            bullet->Direction = ownerTransform.Up;
        }
    }
}

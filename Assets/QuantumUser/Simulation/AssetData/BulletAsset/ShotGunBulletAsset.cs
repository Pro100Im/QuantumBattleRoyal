using Photon.Deterministic;

namespace Quantum
{
    public unsafe class ShotGunBulletAsset : BulletAsset
    {
        public int NumberOfBullets;
        public FP SpreadAngle;

        public override void CreateBullet(Frame f, WeaponAsset weaponAsset, EntityRef owner)
        {
            var ownerTransform = f.Get<Transform2D>(owner);
            var spreadAngleRad = SpreadAngle * FP.Deg2Rad;

            for(int i = 0; i < NumberOfBullets; i++)
            {
                var bulletEntity = f.Create(Bullet);
                var bullet = f.Unsafe.GetPointer<Bullet>(bulletEntity);
                var bulletTransform = f.Unsafe.GetPointer<Transform2D>(bulletEntity);

                bulletTransform->Position = ownerTransform.Position + weaponAsset.Offset.XZ.Rotate(ownerTransform.Rotation);
                bulletTransform->Rotation = ownerTransform.Rotation + FPMath.Lerp(-spreadAngleRad, spreadAngleRad, (FP)i / (NumberOfBullets - 1));

                bullet->Speed = Speed;
                bullet->Damage = Damage;
                bullet->Time = Duration;
                bullet->Owner = owner;
                bullet->HeightOffset = weaponAsset.Offset.Y;
                bullet->Direction = bulletTransform->Up;
            }
        }
    }
}

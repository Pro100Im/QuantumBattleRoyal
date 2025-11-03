namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class CharacterMovableSystem : SystemMainThreadFilter<CharacterMovableSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public KCC* KCC;
            public Transform2D* Transform;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerLink->Player);

            MovePlayer(f, ref filter, input);
            RotatePlayer(f, ref filter, input);
        }

        private void MovePlayer(Frame f, ref Filter filter, Input* input)
        {
            var dir = input->Direction;

            if (dir.Magnitude > 1)
                dir = dir.Normalized;

            var kccSettings = f.FindAsset<KCCSettings>(filter.KCC->Settings);
            kccSettings.Move(f, filter.Entity, dir);
        }

        private void RotatePlayer(Frame f, ref Filter filter, Input* input)
        {
            var dir = input->MousePosition - filter.Transform->Position;

            filter.Transform->Rotation = FPVector2.RadiansSigned(FPVector2.Up, dir);
        }
    }
}

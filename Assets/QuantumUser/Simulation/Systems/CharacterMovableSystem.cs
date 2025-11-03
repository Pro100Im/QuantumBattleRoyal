namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class CharacterMovableSystem : SystemMainThreadFilter<CharacterMovableSystem.Filter>, ISignalOnPlayerAdded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public KCC* KCC;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerLink->Player);
            var dir = input->Direction;

            if (dir.Magnitude > 1)
                dir = dir.Normalized;

            var kccSettings = f.FindAsset<KCCSettings>(filter.KCC->Settings);
            kccSettings.Move(f, filter.Entity, dir);
        }

        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var playerData = f.GetPlayerData(player);
            var playerEntity = f.Create(playerData.PlayerAvatar);
            var kcc = f.Unsafe.GetPointer<KCC>(playerEntity);
            var kccSettings = f.FindAsset<KCCSettings>(kcc->Settings);

            kcc->Acceleration = kccSettings.Acceleration;
            kcc->MaxSpeed = kccSettings.BaseSpeed;

            var playerLink = new PlayerLink
            {
                Player = player
            };

            f.Add(playerEntity, playerLink);
        }
    }
}

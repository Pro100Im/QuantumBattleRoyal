namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class SpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            if (!firstTime)
                return;

            var playerEntity = CreatePlayer(f, player);

            PlacePlayerOnSpawnPosition(f, playerEntity);

            f.Events.PlayerSpawned(player, playerEntity);
        }

        private EntityRef CreatePlayer(Frame f, PlayerRef player)
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

            return playerEntity;
        }

        private void PlacePlayerOnSpawnPosition(Frame f, EntityRef player)
        {
            var spawnPointManager = f.Unsafe.GetPointerSingleton<SpawnPointManager>();
            var availableSpawnPoints = f.ResolveList(spawnPointManager->AvailableSpawnPoints);
            var usedSpawnPoints = f.ResolveList(spawnPointManager->UsedSpawnPoints);

            if(availableSpawnPoints.Count == 0 && usedSpawnPoints.Count == 0)
            {
                foreach(var spawnPointPair in f.GetComponentIterator<SpawnPoint>())
                {
                    availableSpawnPoints.Add(spawnPointPair.Entity); 
                }
            }

            var randomIndex = f.RNG->Next(0, availableSpawnPoints.Count);
            var spawnPoint = availableSpawnPoints[randomIndex]; 
            var spawnTransform = f.Get<Transform2D>(spawnPoint);
            var playerTransfrom = f.Unsafe.GetPointer<Transform2D>(player);

            playerTransfrom->Position = spawnTransform.Position;

            availableSpawnPoints.RemoveAt(randomIndex);
            usedSpawnPoints.Add(spawnPoint);

            if (availableSpawnPoints.Count == 0)
            {
                spawnPointManager->AvailableSpawnPoints = usedSpawnPoints;
                spawnPointManager->UsedSpawnPoints = new Collections.QListPtr<EntityRef>();
            }
        }
    }
}

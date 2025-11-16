namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GameManagerSystem : SystemMainThread, ISignalOnComponentAdded<GameManager>, ISignalPlayerKilled
    {
        public unsafe void OnAdded(Frame f, EntityRef entity, GameManager* component)
        {
            var asset = f.FindAsset(component->GameManagerAsset);
            component->TimeToWaitForPlayers = asset.TimeToWaitForPlayers;
        }

        public void PlayerKilled(Frame f)
        {
            
        }

        public override void Update(Frame f)
        {
            var gameManager = f.Unsafe.GetPointerSingleton<GameManager>();

            if (gameManager->currentState != GameState.WaitingForPlayers)
                return;

            gameManager->TimeToWaitForPlayers -= f.DeltaTime;

            if (gameManager->TimeToWaitForPlayers <= FP._0)
            {
                gameManager->currentState = f.ComponentCount<PlayerLink>() > 1 ? GameState.Playing : GameState.GameOver;

                if (gameManager->currentState == GameState.GameOver)
                {
                    var winner = GetWinner(f);

                    if (winner == EntityRef.None)
                        return;

                    f.Events.GameOver(winner);
                }
            }
        }

        private EntityRef GetWinner(Frame f)
        {
            var entityRef = EntityRef.None;
            var playerLinks = f.GetComponentIterator<PlayerLink>();

            foreach (var playerLink in playerLinks)
            {
                entityRef = playerLink.Entity;
            }

            return entityRef;
        }
    }
}

namespace Quantum
{
    using Photon.Deterministic;
    using System.Reflection;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GameManagerSystem : SystemMainThread, ISignalOnComponentAdded<GameManager>, ISignalPlayerKilled, ISignalOnPlayerDisconnected
    {
        public unsafe void OnAdded(Frame f, EntityRef entity, GameManager* component)
        {
            var asset = f.FindAsset(component->GameManagerAsset);

            component->TimeToWaitForPlayers = asset.TimeToWaitForPlayers;
        }

        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            foreach (var pair in f.GetComponentIterator<PlayerLink>())
            {
                if (pair.Component.Player == player)
                {
                    f.Destroy(pair.Entity);

                    var count = f.ComponentCount<PlayerLink>();

                    if (count > 1)
                        return;

                    var gameManager = f.Unsafe.GetPointerSingleton<GameManager>();

                    if (GetWinner(f, out var entityRef))
                    {
                        f.Events.GameOver(entityRef);

                        gameManager->currentState = GameState.GameOver;
                    }

                    break;
                }
            }
        }

        public void PlayerKilled(Frame f)
        {
            var gameManager = f.Unsafe.GetPointerSingleton<GameManager>();

            if(gameManager->currentState != GameState.Playing)
                return;

            var count = f.ComponentCount<PlayerLink>();

            if (count > 1)
                return;

            if (GetWinner(f, out var entityRef))
            {
                f.Events.GameOver(entityRef);

                gameManager->currentState = GameState.GameOver;
            }
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
                    if (GetWinner(f, out var entityRef))
                        f.Events.GameOver(entityRef);
                }
            }
        }

        private bool GetWinner(Frame f, out EntityRef entityRef)
        {
            var playerLinks = f.GetComponentIterator<PlayerLink>();

            entityRef = EntityRef.None;

            foreach (var playerLink in playerLinks)
            {
                entityRef = playerLink.Entity;
            }

            return entityRef != EntityRef.None;
        }
    }
}

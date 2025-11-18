using Quantum;
using TMPro;
using UnityEngine;

namespace QuantumUser.View
{
    public class WinnerUI : QuantumSceneViewComponent
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TextMeshProUGUI _winnerText;

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe<EventGameOver>(this, GameOver);
        }

        private void GameOver(EventGameOver callback)
        {
            var f = callback.Game.Frames.Predicted;
            var playerRef = f.Get<PlayerLink>(callback.Winner).Player;
            var playerData = f.GetPlayerData(playerRef);

            _winnerText.text = $"Winner: {playerData.PlayerNickname}";
            _winPanel.SetActive(true);
        }
    }
}
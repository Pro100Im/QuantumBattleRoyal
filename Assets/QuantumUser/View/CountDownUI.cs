using Quantum;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : QuantumSceneViewComponent
{
    [SerializeField] private TextMeshProUGUI _timeRemainingText;
    [SerializeField] private Image _timeProgressImage;

    public override void OnActivate(Frame frame)
    {
        QuantumEvent.Subscribe<EventGameOver>(this, GameOver);
    }

    private void GameOver(EventGameOver callback)
    {
        var f = callback.Game.Frames.Predicted;
        var playerRef = f.Get<PlayerLink>(callback.Winner).Player;
        var playerData = f.GetPlayerData(playerRef);

        Debug.Log($"Game Over! Winner: {playerData.PlayerNickname}");
    }

    public override void OnUpdateView()
    {
        base.OnUpdateView();

        var f = PredictedFrame;
        var shrinkingCircle = f.GetSingleton<ShrinkingCircle>();
        var time = shrinkingCircle.CurrentTimeToNextState.AsFloat;
        var currentState = shrinkingCircle.CurrentState;

        _timeRemainingText.text = time < 0 ? "0" : time.ToString("F2", CultureInfo.InvariantCulture);
        _timeProgressImage.fillAmount = time / currentState.TimeToNextState.AsFloat;
    }

    public override void OnDeactivate()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}

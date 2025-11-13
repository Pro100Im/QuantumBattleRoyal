using Quantum;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : QuantumSceneViewComponent
{
    [SerializeField] private TextMeshProUGUI _timeRemainingText;
    [SerializeField] private Image _timeProgressImage;

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
}

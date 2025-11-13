namespace Quantum
{
    using DG.Tweening;
    using System;
    using UnityEngine;

    public class ShrinkingCircleView : QuantumEntityViewComponent
    {
        [SerializeField] private Transform _redCircle, _whiteCircle;

        public override void OnActivate(Frame frame)
        {
            base.OnActivate(frame);

            var shrinkingCircle = frame.GetSingleton<ShrinkingCircle>();

            _whiteCircle.localScale = _redCircle.localScale = new Vector3(shrinkingCircle.CurrentRadius.AsFloat, shrinkingCircle.CurrentRadius.AsFloat);
            _redCircle.gameObject.SetActive(false);

            QuantumEvent.Subscribe<EventShrinkingCircleChangeState>(this, ShrinkingCircleChangedState);
        }

        private void ShrinkingCircleChangedState(EventShrinkingCircleChangeState callback)
        {
            var shrinkingCircle = VerifiedFrame.GetSingleton<ShrinkingCircle>();
            var currentState = shrinkingCircle.CurrentState.CircleStateUnion.Field;

            _redCircle.gameObject.SetActive(currentState is CircleStateUnion.PRESHRINKSTATE || currentState is CircleStateUnion.SHRINKSTATE);

            if (currentState is CircleStateUnion.PRESHRINKSTATE)
            {
                _whiteCircle.DOScale(new Vector3 (shrinkingCircle.TargetRadius.AsFloat, shrinkingCircle.TargetRadius.AsFloat), 1f);
            }
        }

        public override void OnLateUpdateView()
        {
            base.OnLateUpdateView();

            var shrinkingCircle = PredictedFrame.GetSingleton<ShrinkingCircle>();
            var currentState = shrinkingCircle.CurrentState.CircleStateUnion.Field;

            if (currentState != CircleStateUnion.SHRINKSTATE)
                return;

            _redCircle.localScale = new Vector3(shrinkingCircle.CurrentRadius.AsFloat, shrinkingCircle.CurrentRadius.AsFloat);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            QuantumEvent.UnsubscribeListener(this);
        }
    }
}

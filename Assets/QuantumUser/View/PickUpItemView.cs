namespace Quantum
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PickUpItemView : QuantumEntityViewComponent
    {
        [SerializeField] private Image _progressbarFill;

        public override void OnUpdateView()
        {
            base.OnUpdateView();

            if(!PredictedFrame.Exists(EntityRef))
                return;

            var pickedUpItem = PredictedFrame.Get<PickUpItem>(EntityRef);
            var percentage = (pickedUpItem.CurrentPickUpTime / pickedUpItem.PickUpTime).AsFloat;

            _progressbarFill.fillAmount = percentage;
        }
    }
}

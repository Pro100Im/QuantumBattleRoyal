namespace Quantum {
  using Photon.Deterministic;
  using UnityEngine;

  /// <summary>
  /// A Unity script that creates empty input for any Quantum game.
  /// </summary>
  public class QuantumDebugInput : MonoBehaviour {

    private Vector3 _mousePosition;

    private void OnEnable() {
      QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    /// <summary>
    /// Set an empty input when polled by the simulation.
    /// </summary>
    /// <param name="callback"></param>
    public void PollInput(CallbackPollInput callback) {
#if DEBUG
      if (callback.IsInputSet) {
        Debug.LogWarning($"{nameof(QuantumDebugInput)}.{nameof(PollInput)}: Input was already set by another user script, unsubscribing from the poll input callback. Please delete this component.", this);
        QuantumCallback.UnsubscribeListener(this);
        return;
      }
#endif

      var i = new Quantum.Input();
      var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

      if (Physics.Raycast(ray, out var hit, 100, 1 << UnityEngine.LayerMask.NameToLayer("Ground")))
        _mousePosition = hit.point;

      i.MousePosition = _mousePosition.ToFPVector3().XZ;
      i.Direction = new FPVector2(UnityEngine.Input.GetAxis("Horizontal").ToFP(), UnityEngine.Input.GetAxis("Vertical").ToFP());
      i.Fire = UnityEngine.Input.GetMouseButton(0);

      callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
  }
}

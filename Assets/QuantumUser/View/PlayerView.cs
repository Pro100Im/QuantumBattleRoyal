using Quantum;
using UnityEngine;

public unsafe class PlayerView : QuantumEntityViewComponent
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _overHeadUI;

    private readonly int _moveXHash = Animator.StringToHash("moveX");
    private readonly int _moveZHash = Animator.StringToHash("moveZ");

    private bool _isLocalPlayer;
    private Renderer[] _renderers;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
    }

    public override void OnActivate(Frame f)
    {
        base.OnActivate(f);

        _isLocalPlayer = _game.PlayerIsLocal(f.Get<PlayerLink>(EntityRef).Player);

        var layer = UnityEngine.LayerMask.NameToLayer(_isLocalPlayer ? "Player_Local" : "Player_Remote");

        foreach (var renderer in _renderers)
        {
            renderer.gameObject.layer = layer;
            renderer.enabled = true;
        }

        _overHeadUI.SetActive(true);

        QuantumEvent.Subscribe<EventOnPlayerEnteredGrass>(this, OnPlayerEnteredGrass);
        QuantumEvent.Subscribe<EventOnPlayerExitGrass>(this, OnPlayerExitGrass);
    }

    private void OnPlayerEnteredGrass(EventOnPlayerEnteredGrass callback)
    {
        if (callback.Player != PredictedFrame.Get<PlayerLink>(EntityRef).Player)
            return;

        if (_isLocalPlayer)
            return;

        TogglePlayerVisible(false);
    }

    private void OnPlayerExitGrass(EventOnPlayerExitGrass callback)
    {
        if (callback.Player != PredictedFrame.Get<PlayerLink>(EntityRef).Player)
            return;

        if (_isLocalPlayer)
            return;

        TogglePlayerVisible(true);
    }

    private void TogglePlayerVisible(bool value)
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = value;
        }

        _overHeadUI.SetActive(value);
    }

    public override void OnUpdateView()
    {
        base.OnUpdateView();

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        var plaerRef = PredictedFrame.Get<PlayerLink>(EntityRef).Player;
        var input = PredictedFrame.GetPlayerInput(plaerRef);
        var kcc = PredictedFrame.Get<KCC>(EntityRef);
        var velocity = kcc.Velocity;

        _animator.SetFloat(_moveXHash, velocity.X.AsFloat);
        _animator.SetFloat(_moveZHash, velocity.Y.AsFloat);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        QuantumEvent.UnsubscribeListener(this);
    }
}

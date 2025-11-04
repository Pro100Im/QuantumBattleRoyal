using Quantum;
using TMPro;
using UnityEngine;

public unsafe class PlayerView : QuantumEntityViewComponent
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _overHeadUI;
    [SerializeField] private TextMeshProUGUI _nickName;
    [SerializeField]
    [Range(1, 2)] private float _speedAnimMultiplier = 1.5f;

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

        var playerLink = f.Get<PlayerLink>(EntityRef);
        var playerData = f.GetPlayerData(playerLink.Player);

        _nickName.text = playerData.PlayerNickname;
        _isLocalPlayer = _game.PlayerIsLocal(playerLink.Player);

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
        if (!PredictedFrame.Exists(EntityRef))
            return;

        base.OnUpdateView();

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        var playerRef = PredictedFrame.Get<PlayerLink>(EntityRef).Player;
        var input = PredictedFrame.GetPlayerInput(playerRef);
        var transform = PredictedFrame.Get<Transform2D>(EntityRef);
        var rotationDirection = input->Direction.Rotate(-transform.Rotation).ToUnityVector2() * _speedAnimMultiplier;

        _animator.SetFloat(_moveXHash, rotationDirection.x);
        _animator.SetFloat(_moveZHash, rotationDirection.y);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();

        QuantumEvent.UnsubscribeListener(this);
    }
}

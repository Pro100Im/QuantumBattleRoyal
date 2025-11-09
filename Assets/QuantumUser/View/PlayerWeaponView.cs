namespace Quantum
{
    using QuantumUser.View;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PlayerWeaponView : QuantumEntityViewComponent
    {
        [SerializeField] private PlayerWeapon _currentWeapon;
        [SerializeField] private Dictionary<WeaponType, PlayerWeapon> _weapons;

        private void Awake()
        {
            _weapons = GetComponentsInChildren<PlayerWeapon>(true).ToDictionary(w => w.Type, w => w);
        }

        public override void OnActivate(Frame frame)
        {
            base.OnActivate(frame);

            foreach (var weapon in _weapons.Values)
            {
                weapon.gameObject.SetActive(false);
            }

            _currentWeapon = _weapons[WeaponType.Pistol];
            _currentWeapon.gameObject.SetActive(true);

            QuantumEvent.Subscribe<EventWeaponChange>(this, WeaponChanged);
        }

        private void WeaponChanged(EventWeaponChange callback)
        {
            if (!_game.PlayerIsLocal(callback.Player))
                return;

            if (_currentWeapon.Type == callback.Type)
                return;

            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon.Rig.weight = 0f;
            _currentWeapon = _weapons[callback.Type];
            _currentWeapon.Rig.weight = 1f;
            _currentWeapon.gameObject.SetActive(true);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            QuantumEvent.UnsubscribeListener(this);
        }
    }
}

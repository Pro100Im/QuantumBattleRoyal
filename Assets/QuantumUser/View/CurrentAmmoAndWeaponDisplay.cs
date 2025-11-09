using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuantumUser.View
{
    public class CurrentAmmoAndWeaponDisplay : MonoBehaviour
    {
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private TextMeshProUGUI _ammoCount;

        private void Awake()
        {
            QuantumEvent.Subscribe<EventWeaponChange>(this, WeaponChanged);
            QuantumEvent.Subscribe<EventPlayerSpawned>(this, PlayerSpawned);
            QuantumEvent.Subscribe<EventAmmoChange>(this, AmmoChanged);
        }

        private void AmmoChanged(EventAmmoChange callback)
        {
            if (!callback.Game.PlayerIsLocal(callback.Player))
                return;

            _ammoCount.text = callback.NewAmmo.ToString(); 
        }

        private void PlayerSpawned(EventPlayerSpawned callback)
        {
            if (!callback.Game.PlayerIsLocal(callback.Player))
                return;

            var f = callback.Game.Frames.Verified;

            FillImageAndText(f, callback.Entity);
        }

        private void WeaponChanged(EventWeaponChange callback)
        {
            if (!callback.Game.PlayerIsLocal(callback.Player))
                return;

            var f = callback.Game.Frames.Verified;

            FillImageAndText(f, callback.Entity);
        }

        private void FillImageAndText(Frame f, EntityRef entityRef)
        {
            var weapon = f.Get<Weapon>(entityRef);

            _weaponIcon.sprite = f.FindAsset(weapon.WeaponData).Sprite;
            _ammoCount.text = weapon.Ammo.ToString();
        }

        private void OnDestroy()
        {
            QuantumEvent.UnsubscribeListener(this);
        }
    }
}
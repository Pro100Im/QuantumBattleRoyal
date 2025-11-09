using Quantum;
using UnityEngine;

namespace QuantumUser.View
{
    public class PlayerWeapon : MonoBehaviour
    {
        [field: SerializeField] public WeaponType Type { get; private set; }
    }
}
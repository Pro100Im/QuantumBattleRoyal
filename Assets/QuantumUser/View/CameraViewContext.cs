using UnityEngine;
using Quantum;
using Unity.Cinemachine;

public class CameraViewContext : MonoBehaviour, IQuantumViewContext
{
    [field: SerializeField] public CinemachineCamera VirtualCamera { get; private set; }
}

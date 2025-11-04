using Quantum;

public class LookAtCamera : QuantumViewComponent<CameraViewContext>
{
    private void Update()
    {
        transform.LookAt(ViewContext.VirtualCamera.transform);
    }
}

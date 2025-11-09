namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class ShrinkingCircleSystem : SystemMainThread, ISignalOnComponentAdded<ShrinkingCircle>
    {
        public unsafe void OnAdded(Frame f, EntityRef entity, ShrinkingCircle* component)
        {
            var asset = f.FindAsset(component->Asset);
            var transform = f.Unsafe.GetPointer<Transform2D>(entity);
            var randomX = f.RNG->Next(asset.MinBounds.X, asset.MaxBounds.X);
            var randomY = f.RNG->Next(asset.MinBounds.Y, asset.MaxBounds.Y);
            var pos = new FPVector2(randomX, randomY);

            asset.States[0].Materialize(f, ref component->CurrentState);

            component->CurrentStateIndex = 0;
            component->CurrentState.EnterState(component);
            component->Position = pos;

            transform->Position = pos;
        }

        public override void Update(Frame f)
        {
            var shrinkingCircle = f.Unsafe.GetPointerSingleton<ShrinkingCircle>();
            var asset = f.FindAsset(shrinkingCircle->Asset);

            shrinkingCircle->CurrentState.UpdateState(f, shrinkingCircle);

            if (shrinkingCircle->CurrentTimeToNextState > 0)
                return;

            if (shrinkingCircle->CurrentStateIndex >= asset.States.Length - 1)
                return;

            shrinkingCircle->CurrentStateIndex++;
            asset.States[shrinkingCircle->CurrentStateIndex].Materialize(f, ref shrinkingCircle->CurrentState);
            shrinkingCircle->CurrentState.EnterState(shrinkingCircle);
        }
    }
}

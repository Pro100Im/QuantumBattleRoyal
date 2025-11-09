namespace Quantum
{
    using Photon.Deterministic;
    using Quantum.Prototypes;

    public class ShrinkingCircleAsset : AssetObject
    {
        public FP DamageDealingPerSecond;

        public FPVector2 MinBounds, MaxBounds;

        public ShrinkingCircleStatePrototype[] States;
    }
}

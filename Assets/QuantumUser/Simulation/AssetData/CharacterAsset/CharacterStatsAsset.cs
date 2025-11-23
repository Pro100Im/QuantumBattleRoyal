namespace Quantum
{
    using Photon.Deterministic;

    public class CharacterStatsAsset : AssetObject
    {
        public FP HealthMultiplier;
        public FP FireRateMultiplier;

        private void OnValidate()
        {
            HealthMultiplier = FPMath.Clamp(HealthMultiplier, 1, 2);
            FireRateMultiplier = FPMath.Clamp(FireRateMultiplier, FP._0_10, 1);
        }
    }
}

namespace GasDistributionOptimizer.Models
{
    public class BlastFurnace
    {
        public int Id { get; set; }

        // Базовые параметры
        public double BaseNaturalGas { get; set; }
        public double MinNaturalGas { get; set; }
        public double MaxNaturalGas { get; set; }
        public double BaseCoke { get; set; }
        public double CokeReplacementRatio { get; set; }
        public double BaseIronProduction { get; set; }
        public double BaseSulfur { get; set; }

        public double MinIronProduction { get; set; }
        public double MaxCoke { get; set; }
        public double MaxSulfur { get; set; }

        // Коэффициенты влияния на производство чугуна (dP/dV, dP/dK)
        public double DeltaIronPerGas { get; set; }
        public double DeltaIronPerCoke { get; set; }

        // Коэффициенты влияния на серу (dS/dV, dS/dK)
        public double DeltaSulfurPerGas { get; set; }
        public double DeltaSulfurPerCoke { get; set; }
    }
}

using GasDistributionOptimizer.Models;
using System.Collections.ObjectModel;

namespace GasDistributionOptimizer.Services
{
    public static class TestData
    {
        public static ObservableCollection<BlastFurnace> GetVariant4Furnaces()
        {
            return new ObservableCollection<BlastFurnace>
            {
                new BlastFurnace { Id = 1, BaseNaturalGas = 15000, MinNaturalGas = 10000, MaxNaturalGas = 20000, BaseCoke = 64.25, CokeReplacementRatio = 0.59, BaseIronProduction = 146.4, MinIronProduction = 140, MaxCoke = 66, DeltaIronPerGas = -0.0007295, DeltaIronPerCoke = -0.002970, DeltaSulfurPerGas = -0.00000301, DeltaSulfurPerCoke = -0.00000301 },
                new BlastFurnace { Id = 2, BaseNaturalGas = 17000, MinNaturalGas = 10000, MaxNaturalGas = 20000, BaseCoke = 66.76, CokeReplacementRatio = 0.53, BaseIronProduction = 136.4, MinIronProduction = 130, MaxCoke = 68, DeltaIronPerGas = -0.0006695, DeltaIronPerCoke = -0.002970, DeltaSulfurPerGas = -0.00000287, DeltaSulfurPerCoke = -0.00000287 },
                new BlastFurnace { Id = 3, BaseNaturalGas = 11000, MinNaturalGas = 10000, MaxNaturalGas = 20000, BaseCoke = 56.08, CokeReplacementRatio = 0.85, BaseIronProduction = 134.3, MinIronProduction = 130, MaxCoke = 56, DeltaIronPerGas = 0, DeltaIronPerCoke = -0.002928, DeltaSulfurPerGas = -0.00000284, DeltaSulfurPerCoke = -0.00000284 },
                new BlastFurnace { Id = 4, BaseNaturalGas = 13000, MinNaturalGas = 10000, MaxNaturalGas = 20000, BaseCoke = 49.78, CokeReplacementRatio = 0.59, BaseIronProduction = 122.3, MinIronProduction = 120, MaxCoke = 52, DeltaIronPerGas = -0.0007237, DeltaIronPerCoke = -0.002897, DeltaSulfurPerGas = -0.00000292, DeltaSulfurPerCoke = -0.00000292 },
                new BlastFurnace { Id = 5, BaseNaturalGas = 12000, MinNaturalGas = 10000, MaxNaturalGas = 20000, BaseCoke = 62.92, CokeReplacementRatio = 0.75, BaseIronProduction = 138.2, MinIronProduction = 130, MaxCoke = 64, DeltaIronPerGas = -0.0007724, DeltaIronPerCoke = -0.002970, DeltaSulfurPerGas = -0.00000288, DeltaSulfurPerCoke = -0.00000288 }
            };
        }
    }
}

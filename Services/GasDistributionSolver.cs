using GasDistributionOptimizer.Models;
using Google.OrTools.LinearSolver;

namespace GasDistributionOptimizer.Services
{
    public class GasDistributionSolver
    {
        private const double TonsToKgMultiplier = 1000.0;
        public OptimizationResult Solve(IEnumerable<BlastFurnace> furnaces, double valB29, double valB30)
        {
            var result = new OptimizationResult();
            Solver solver = Solver.CreateSolver("GLOP");
            if (solver == null) return result;

            var gasVars = new Dictionary<int, Variable>();
            foreach (var f in furnaces)
            {
                gasVars[f.Id] = solver.MakeNumVar(f.MinNaturalGas, f.MaxNaturalGas, $"Gas_F{f.Id}");
            }

            foreach (var f in furnaces)
            {
                Variable v = gasVars[f.Id];

                double coeffIron = f.DeltaIronPerGas - (f.CokeReplacementRatio * f.DeltaIronPerCoke);
                double ironRightSide = f.MinIronProduction - f.BaseIronProduction + (coeffIron * f.BaseNaturalGas);

                Constraint ironConst = solver.MakeConstraint(ironRightSide, double.PositiveInfinity, $"Iron_F{f.Id}");
                ironConst.SetCoefficient(v, coeffIron);

                double coeffCoke = -0.001 * f.CokeReplacementRatio;
                double cokeRightSide = f.MaxCoke - f.BaseCoke - (0.001 * f.CokeReplacementRatio * f.BaseNaturalGas);

                Constraint cokeConst = solver.MakeConstraint(double.NegativeInfinity, cokeRightSide, $"Coke_F{f.Id}");
                cokeConst.SetCoefficient(v, coeffCoke);
            }

            Objective objective = solver.Objective();
            foreach (var f in furnaces)
            {
                double objCoeff = (0.5 * (f.CokeReplacementRatio * valB29 - valB30)) +
                                  (0.5 * (f.DeltaSulfurPerGas - f.CokeReplacementRatio * f.DeltaSulfurPerCoke));

                objective.SetCoefficient(gasVars[f.Id], objCoeff);
            }

            objective.SetMinimization();

            Solver.ResultStatus status = solver.Solve();

            if (status == Solver.ResultStatus.OPTIMAL || status == Solver.ResultStatus.FEASIBLE)
            {
                result.IsSuccess = true;
                double totalGas = 0;
                double totalCoke = 0;
                double totalIron = 0;
                double totalSavings = 0;

                result.FurnaceDetailResults.Clear();

                foreach (var f in furnaces)
                {
                    double optimalGas = gasVars[f.Id].SolutionValue();
                    totalGas += optimalGas;

                    double deltaV = optimalGas - f.BaseNaturalGas;
                    double finalCoke = f.BaseCoke - (0.001 * f.CokeReplacementRatio * deltaV);

                    double coeffIron = f.DeltaIronPerGas - (f.CokeReplacementRatio * f.DeltaIronPerCoke);
                    double finalIron = f.BaseIronProduction + (coeffIron * deltaV);

                    totalCoke += finalCoke;
                    totalIron += finalIron;

                    double gasCost = optimalGas * valB30;
                    double cokeCost = finalCoke * TonsToKgMultiplier * valB29;

                    totalSavings += (gasCost + cokeCost);

                    result.FurnaceDetailResults.Add(new FurnaceResult
                    {
                        FurnaceId = f.Id,
                        OptimalGas = optimalGas,
                        FinalCoke = finalCoke,
                        FinalIron = finalIron
                    });
                }

                result.TotalNaturalGas = totalGas;
                result.TotalCoke = totalCoke;
                result.TotalIronProduction = totalIron;
                result.TotalSavings = totalSavings;
            }
            else
            {
                result.IsSuccess = false;
                result.StatusMessage = "Решение не найдено. Проверьте ограничения.";
            }

            return result;
        }
    }
}

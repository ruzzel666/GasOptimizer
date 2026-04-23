using GasDistributionOptimizer.Models;
using Google.OrTools.LinearSolver;
using System.Collections.Generic;

namespace GasDistributionOptimizer
{
    public class GasDistributionSolver
    {
        public OptimizationResult Solve(IEnumerable<BlastFurnace> furnaces, double valB29, double valB30)
        {
            var result = new OptimizationResult();
            Solver solver = Solver.CreateSolver("GLOP");
            if (solver == null) return result;

            var gasVars = new Dictionary<int, Variable>();
            foreach (var f in furnaces)
            {
                // Задаем жесткие границы газа (10000 - 20000)
                gasVars[f.Id] = solver.MakeNumVar(f.MinNaturalGas, f.MaxNaturalGas, $"Gas_F{f.Id}");
            }

            foreach (var f in furnaces)
            {
                Variable v = gasVars[f.Id];

                // 1. ОГРАНИЧЕНИЕ ПО ЧУГУНУ (Без ошибочного 0.001 из Экселя, т.к. dP/dK задан в кг)
                // P_base + (dP/dV - E * dP/dK) * (V - V_base) >= P_min
                double coeffIron = f.DeltaIronPerGas - (f.CokeReplacementRatio * f.DeltaIronPerCoke);
                double ironRightSide = f.MinIronProduction - f.BaseIronProduction + (coeffIron * f.BaseNaturalGas);

                Constraint ironConst = solver.MakeConstraint(ironRightSide, double.PositiveInfinity, $"Iron_F{f.Id}");
                ironConst.SetCoefficient(v, coeffIron);

                // 2. ОГРАНИЧЕНИЕ ПО КОКСУ (То самое из 12 строки Экселя)
                // K_base + 0.001 * (-E) * (V - V_base) <= K_max
                double coeffCoke = -0.001 * f.CokeReplacementRatio;
                double cokeRightSide = f.MaxCoke - f.BaseCoke - (0.001 * f.CokeReplacementRatio * f.BaseNaturalGas);

                Constraint cokeConst = solver.MakeConstraint(double.NegativeInfinity, cokeRightSide, $"Coke_F{f.Id}");
                cokeConst.SetCoefficient(v, coeffCoke);
            }

            // 3. ЦЕЛЕВАЯ ФУНКЦИЯ
            Objective objective = solver.Objective();
            foreach (var f in furnaces)
            {
                // Точная копия формулы из ячейки B24
                double objCoeff = (0.5 * (f.CokeReplacementRatio * valB29 - valB30)) +
                                  (0.5 * (f.DeltaSulfurPerGas - f.CokeReplacementRatio * f.DeltaSulfurPerCoke));

                objective.SetCoefficient(gasVars[f.Id], objCoeff);
            }

            // СЕКРЕТ ЭКСЕЛЯ: Из-за структуры цен эта формула уходит в минус. 
            // Чтобы заставить газ снижаться (как в твоем эталоне), мы должны МИНИМИЗИРОВАТЬ эту разницу.
            // Замени на SetMaximization(), если вдруг суммы опять улетят в 96000.
            objective.SetMinimization();

            Solver.ResultStatus status = solver.Solve();

            if (status == Solver.ResultStatus.OPTIMAL || status == Solver.ResultStatus.FEASIBLE)
            {
                result.IsSuccess = true;
                double totalGas = 0;
                double totalCoke = 0;
                double totalIron = 0;
                double totalSavings = 0; // Переменная для нашей экономики

                // Очищаем список на всякий случай, чтобы таблица точно обновилась
                result.FurnaceDetailResults.Clear();

                foreach (var f in furnaces)
                {
                    // Получаем оптимальный газ для печи
                    double optimalGas = gasVars[f.Id].SolutionValue();
                    totalGas += optimalGas;

                    // Считаем изменения
                    double deltaV = optimalGas - f.BaseNaturalGas;
                    double finalCoke = f.BaseCoke - (0.001 * f.CokeReplacementRatio * deltaV);

                    double coeffIron = f.DeltaIronPerGas - (f.CokeReplacementRatio * f.DeltaIronPerCoke);
                    double finalIron = f.BaseIronProduction + (coeffIron * deltaV);

                    // Плюсуем общие расходы цеха
                    totalCoke += finalCoke;
                    totalIron += finalIron;

                    // --- ЧЕСТНЫЙ РАСЧЕТ ОБЩИХ ЗАТРАТ ---
                    // Затраты на газ = Оптимальный газ * Цена газа (valB30)
                    double gasCost = optimalGas * valB30;

                    // Затраты на кокс = Итоговый кокс (в тоннах) * 1000 кг * Цена кокса (valB29)
                    double cokeCost = finalCoke * 1000 * valB29;

                    // Суммируем затраты (используем ту же переменную totalSavings, чтобы не менять модель)
                    totalSavings += (gasCost + cokeCost);

                    // ОБЯЗАТЕЛЬНО: Возвращаем данные печи в список, чтобы появилась таблица!
                    result.FurnaceDetailResults.Add(new FurnaceResult
                    {
                        FurnaceId = f.Id,
                        OptimalGas = optimalGas,
                        FinalCoke = finalCoke,
                        FinalIron = finalIron
                    });
                }

                // Записываем финальные итоги в результат
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

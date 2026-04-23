namespace GasDistributionOptimizer.Models
{
    public class OptimizationResult
    {
        /// <summary>
        /// Флаг успешности расчета.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Сообщение о статусе расчета (например, "Оптимальное решение найдено" или текст ошибки).
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// Суммарный оптимальный расход природного газа по всем печам цеха (м³/ч).
        /// </summary>
        public double TotalNaturalGas { get; set; }
        /// <summary>
        /// Суммарный итоговый расход кокса по всем печам цеха (т/ч).
        /// </summary>
        public double TotalCoke { get; set; }
        /// <summary>
        /// Суммарное итоговое производство чугуна по всем печам цеха (т/ч).
        /// </summary>
        public double TotalIronProduction { get; set; }
        /// <summary>
        /// Общие финансовые затраты на топливо (газ + кокс) для всего цеха (руб/ч).
        /// </summary>
        public double TotalSavings { get; set; }
        /// <summary>
        /// Детализированный список результатов расчета для каждой отдельной доменной печи.
        /// </summary>
        public List<FurnaceResult> FurnaceDetailResults { get; set; } = new List<FurnaceResult>();
    }

    public class FurnaceResult
    {
        /// <summary>
        /// Идентификатор (номер) доменной печи.
        /// </summary>
        public int FurnaceId { get; set; }
        /// <summary>
        /// Оптимальный расход природного газа для данной печи, подобранный алгоритмом (м³/ч).
        /// </summary>
        public double OptimalGas { get; set; }
        /// <summary>
        /// Итоговый расход кокса для данной печи с учетом нового расхода газа (т/ч).
        /// </summary>
        public double FinalCoke { get; set; }
        /// <summary>
        /// Итоговое производство чугуна на данной печи с учетом измененного теплового баланса (т/ч).
        /// </summary>
        public double FinalIron { get; set; }
    }
}

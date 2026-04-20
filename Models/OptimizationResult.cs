namespace GasDistributionOptimizer.Models
{
    // Общие итоги по цеху
    public class OptimizationResult
    {
        public bool IsSuccess { get; set; }
        public string StatusMessage { get; set; }

        public double TotalNaturalGas { get; set; }     // Общий расход ПГ
        public double TotalCoke { get; set; }           // Общий расход кокса
        public double TotalIronProduction { get; set; } // Общее производство чугуна
        public double TotalSavings { get; set; }        // Общая экономия по ресурсам

        // Детализация по каждой печи
        public List<FurnaceResult> FurnaceDetailResults { get; set; } = new List<FurnaceResult>();
    }

    // Итоги по одной конкретной печи
    public class FurnaceResult
    {
        public int FurnaceId { get; set; }
        public double OptimalGas { get; set; }
        public double FinalCoke { get; set; }
        public double FinalIron { get; set; }
    }
}

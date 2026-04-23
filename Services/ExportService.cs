using GasDistributionOptimizer.Models;
using Microsoft.Win32;
using System.IO;
using System.Text;


namespace GasDistributionOptimizer.Services
{
    public class ExportService
    {
        public bool ExportData(OptimizationResult result)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файл (*.csv)|*.csv",
                FileName = $"Отчет_Цех_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Сохранение результатов оптимизации"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();

                csvContent.AppendLine("Общие показатели по цеху");
                csvContent.AppendLine("Параметр;Значение;Ед. изм.");
                csvContent.AppendLine($"Общий расход природного газа;{result.TotalNaturalGas:F2};м3/ч");
                csvContent.AppendLine($"Общий расход кокса;{result.TotalCoke:F2};т/ч");
                csvContent.AppendLine($"Общее производство чугуна;{result.TotalIronProduction:F2};т/ч");
                csvContent.AppendLine($"Экономические затраты на топливо;{Math.Abs(result.TotalSavings):F2};руб/ч");

                csvContent.AppendLine();

                csvContent.AppendLine("Детализация по каждой печи");
                csvContent.AppendLine("Номер печи;Оптимальный ПГ (м3/ч);Итоговый кокс (т/ч);Итоговое производство (т/ч)");

                foreach (var f in result.FurnaceDetailResults)
                {
                    csvContent.AppendLine($"{f.FurnaceId};{f.OptimalGas:F2};{f.FinalCoke:F2};{f.FinalIron:F2}");
                }

                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);
                return true;
            }
            return false;
        }
    }
}

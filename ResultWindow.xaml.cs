using GasDistributionOptimizer.Models;
using System.IO;
using Microsoft.Win32;
using System.Text;
using System.Windows;

namespace GasDistributionOptimizer
{
    public partial class ResultWindow : Window
    {
        public ResultWindow(OptimizationResult result)
        {
            InitializeComponent();

            txtTotalGas.Text = $"{result.TotalNaturalGas:F2} м³/ч";
            txtTotalCoke.Text = $"{result.TotalCoke:F2} т/ч";
            txtTotalIron.Text = $"{result.TotalIronProduction:F2} т/ч";

            // Math.Abs гарантирует отсутствие минуса
            txtTotalSavings.Text = $"{Math.Abs(result.TotalSavings):F2} руб/ч";

            // Строка, которая рисует таблицу!
            dgvDetails.ItemsSource = result.FurnaceDetailResults;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрываем окно при нажатии на кнопку
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файл (*.csv)|*.csv",
                FileName = $"Отчет_Цех_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Сохранение результатов оптимизации"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StringBuilder csvContent = new StringBuilder();

                    // 1. СЕКЦИЯ: ОБЩИЕ ПОКАЗАТЕЛИ ПО ЦЕХУ
                    csvContent.AppendLine("Общие показатели по цеху");
                    csvContent.AppendLine($"Параметр;Значение;Ед. изм.");
                    csvContent.AppendLine($"Общий расход природного газа;{txtTotalGas.Text.Replace(" м³/ч", "")};м3/ч");
                    csvContent.AppendLine($"Общий расход кокса;{txtTotalCoke.Text.Replace(" т/ч", "")};т/ч");
                    csvContent.AppendLine($"Общее производство чугуна;{txtTotalIron.Text.Replace(" т/ч", "")};т/ч");
                    csvContent.AppendLine($"Общая экономия;{txtTotalSavings.Text.Replace(" руб/ч", "")};руб/ч");

                    // Пустая строка для разделения секций
                    csvContent.AppendLine();

                    // 2. СЕКЦИЯ: ДЕТАЛИЗАЦИЯ ПО ПЕЧАМ
                    csvContent.AppendLine("Детализация по каждой печи");
                    csvContent.AppendLine("Номер печи;Оптимальный ПГ (м3/ч);Итоговый кокс (т/ч);Итоговый чугун (т/ч)");

                    // Берем данные из ItemsSource таблицы
                    if (dgvDetails.ItemsSource is IEnumerable<FurnaceResult> details)
                    {
                        foreach (var f in details)
                        {
                            csvContent.AppendLine($"{f.FurnaceId};{f.OptimalGas:F2};{f.FinalCoke:F2};{f.FinalIron:F2}");
                        }
                    }

                    // Записываем файл (Encoding.UTF8 с BOM важен для кириллицы в Excel)
                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);

                    MessageBox.Show("Отчет успешно сохранен!", "Экспорт", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

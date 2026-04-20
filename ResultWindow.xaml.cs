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

            // Заполняем красивые карточки наверху
            txtTotalGas.Text = $"{result.TotalNaturalGas:F2} м³/ч";
            txtStatus.Text = result.IsSuccess ? "Оптимально" : "Ошибка";

            // Привязываем список результатов к таблице
            dgvDetails.ItemsSource = result.FurnaceDetailResults;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрываем окно при нажатии на кнопку
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            // 1. Настраиваем окно сохранения
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файл (*.csv)|*.csv",
                FileName = $"Отчет_ПГ_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Сохранение результатов расчета"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    StringBuilder csvContent = new StringBuilder();

                    // 2. Добавляем заголовки (используем точку с запятой, чтобы Excel в РФ открыл сразу по столбцам)
                    csvContent.AppendLine("Номер печи;Оптимальный газ (м3/ч);Итоговый кокс (т/ч);Итоговый чугун (т/ч)");

                    // 3. Добавляем данные по каждой печи
                    foreach (var furnace in dgvDetails.ItemsSource as List<FurnaceResult>)
                    {
                        csvContent.AppendLine($"{furnace.FurnaceId};{furnace.OptimalGas:F2};{furnace.FinalCoke:F2};{furnace.FinalIron:F2}");
                    }

                    // 4. Добавляем итоговую строку
                    // Мы можем достать общее значение из текстового блока или передать его отдельно
                    csvContent.AppendLine($";;;");
                    csvContent.AppendLine($"Итог по цеху;{txtTotalGas.Text.Replace(" м³/ч", "")};;");

                    // 5. Записываем в файл в кодировке UTF-8 с BOM (чтобы Excel понимал кириллицу)
                    File.WriteAllText(saveFileDialog.FileName, csvContent.ToString(), Encoding.UTF8);

                    MessageBox.Show("Отчет успешно сохранен!", "Экспорт", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

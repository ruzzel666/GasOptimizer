using GasDistributionOptimizer.Models;
using System.IO;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using GasDistributionOptimizer.Services;

namespace GasDistributionOptimizer
{
    public partial class ResultWindow : Window
    {
        private OptimizationResult _currentResult;

        public ResultWindow(OptimizationResult result)
        {
            InitializeComponent();
            _currentResult = result;

            txtTotalGas.Text = $"{result.TotalNaturalGas:F2} м³/ч";
            txtTotalCoke.Text = $"{result.TotalCoke:F2} т/ч";
            txtTotalIron.Text = $"{result.TotalIronProduction:F2} т/ч";
            txtTotalSavings.Text = $"{Math.Abs(result.TotalSavings):F2} руб/ч";

            dgvDetails.ItemsSource = result.FurnaceDetailResults;
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var exportService = new ExportService();
            bool success = exportService.ExportData(_currentResult);

            if (success)
            {
                MessageBox.Show("Отчет успешно сохранен!", "Экспорт", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

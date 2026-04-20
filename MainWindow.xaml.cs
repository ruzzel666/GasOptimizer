using GasDistributionOptimizer.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace GasDistributionOptimizer
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<BlastFurnace> Furnaces { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Furnaces = new ObservableCollection<BlastFurnace>();
            dgvFurnaces.ItemsSource = Furnaces;
        }

        private void btnLoadTestData_Click(object sender, RoutedEventArgs e)
        {
            Furnaces.Clear();

            Furnaces.Add(new BlastFurnace
            {
                Id = 1,
                BaseNaturalGas = 15000,
                MinNaturalGas = 10000,
                MaxNaturalGas = 20000,
                BaseCoke = 64.25,
                CokeReplacementRatio = 0.59,
                BaseIronProduction = 146.4,
                BaseSulfur = 0.015,
                MinIronProduction = 140,
                MaxCoke = 66,
                MaxSulfur = 0.015,
                DeltaIronPerGas = -0.0007295,
                DeltaIronPerCoke = -0.00297,
                DeltaSulfurPerGas = -0.0000034,
                DeltaSulfurPerCoke = -0.0000030
            });

            Furnaces.Add(new BlastFurnace
            {
                Id = 2,
                BaseNaturalGas = 17000,
                MinNaturalGas = 10000,
                MaxNaturalGas = 20000,
                BaseCoke = 66.76,
                CokeReplacementRatio = 0.53,
                BaseIronProduction = 136.4,
                BaseSulfur = 0.014,
                MinIronProduction = 130,
                MaxCoke = 68,
                MaxSulfur = 0.014,
                DeltaIronPerGas = -0.0006695,
                DeltaIronPerCoke = -0.00297,
                DeltaSulfurPerGas = -0.0000034,
                DeltaSulfurPerCoke = -0.0000029
            });

            Furnaces.Add(new BlastFurnace
            {
                Id = 3,
                BaseNaturalGas = 11000,
                MinNaturalGas = 10000,
                MaxNaturalGas = 20000,
                BaseCoke = 56.08,
                CokeReplacementRatio = 0.85,
                BaseIronProduction = 134.3,
                BaseSulfur = 0.013,
                MinIronProduction = 130,
                MaxCoke = 57,
                MaxSulfur = 0.013,
                DeltaIronPerGas = 0,
                DeltaIronPerCoke = -0.002928,
                DeltaSulfurPerGas = -0.0000035,
                DeltaSulfurPerCoke = -0.0000032
            });

            Furnaces.Add(new BlastFurnace
            {
                Id = 4,
                BaseNaturalGas = 13000,
                MinNaturalGas = 10000,
                MaxNaturalGas = 20000,
                BaseCoke = 49.78,
                CokeReplacementRatio = 0.59,
                BaseIronProduction = 122.3,
                BaseSulfur = 0.014,
                MinIronProduction = 120,
                MaxCoke = 51,
                MaxSulfur = 0.014,
                DeltaIronPerGas = -0.00072373,
                DeltaIronPerCoke = -0.002897,
                DeltaSulfurPerGas = -0.0000033,
                DeltaSulfurPerCoke = -0.0000029
            });

            Furnaces.Add(new BlastFurnace
            {
                Id = 5,
                BaseNaturalGas = 12000,
                MinNaturalGas = 10000,
                MaxNaturalGas = 20000,
                BaseCoke = 62.92,
                CokeReplacementRatio = 0.75,
                BaseIronProduction = 138.2,
                BaseSulfur = 0.017,
                MinIronProduction = 130,
                MaxCoke = 64,
                MaxSulfur = 0.017,
                DeltaIronPerGas = -0.0007724,
                DeltaIronPerCoke = -0.00297,
                DeltaSulfurPerGas = -0.0000034,
                DeltaSulfurPerCoke = -0.0000031
            });

            MessageBox.Show("Данные обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (Furnaces == null || Furnaces.Count == 0) return;

            double valB29 = 1.8;
            double valB30 = 0.6;

            var solver = new GasDistributionSolver();
            OptimizationResult result = solver.Solve(Furnaces, valB29, valB30);

            if (result.IsSuccess)
            {
                lblTotalResult.Text = result.TotalNaturalGas.ToString("F2") + " м³/ч";

                var resultWin = new ResultWindow(result);
                resultWin.Owner = this;
                resultWin.ShowDialog();
            }
            else
            {
                MessageBox.Show(result.StatusMessage, "Ошибка");
            }
        }
    }
}
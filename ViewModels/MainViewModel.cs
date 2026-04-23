using GasDistributionOptimizer.Models;
using GasDistributionOptimizer.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace GasDistributionOptimizer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BlastFurnace> _furnaces;
        public ObservableCollection<BlastFurnace> Furnaces
        {
            get => _furnaces;
            set { _furnaces = value; OnPropertyChanged(); }
        }

        private BlastFurnace _selectedFurnace;
        public BlastFurnace SelectedFurnace
        {
            get => _selectedFurnace;
            set { _selectedFurnace = value; OnPropertyChanged(); }
        }

        private string _gasPrice = "0.6";
        public string GasPrice
        {
            get => _gasPrice;
            set { _gasPrice = value; OnPropertyChanged(); }
        }

        private string _cokePrice = "1.8";
        public string CokePrice
        {
            get => _cokePrice;
            set { _cokePrice = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand AddFurnaceCommand { get; }
        public ICommand DeleteFurnaceCommand { get; }
        public ICommand CalculateCommand { get; }

        public MainViewModel()
        {
            LoadDataCommand = new RelayCommand(_ => LoadTestData());
            AddFurnaceCommand = new RelayCommand(_ => AddFurnace());

            DeleteFurnaceCommand = new RelayCommand(_ => DeleteFurnace(), _ => SelectedFurnace != null);

            CalculateCommand = new RelayCommand(_ => Calculate());
        }

        private void LoadTestData()
        {
            Furnaces = TestData.GetVariant4Furnaces();
        }

        private void AddFurnace()
        {
            if (Furnaces == null) Furnaces = new ObservableCollection<BlastFurnace>();

            int newId = Furnaces.Count > 0 ? Furnaces.Max(f => f.Id) + 1 : 1;
            Furnaces.Add(new BlastFurnace { Id = newId, BaseNaturalGas = 15000, MinNaturalGas = 10000, MaxNaturalGas = 20000, BaseCoke = 60, CokeReplacementRatio = 0.6, BaseIronProduction = 140, MinIronProduction = 135, MaxCoke = 65, DeltaIronPerGas = -0.0007, DeltaIronPerCoke = -0.0029, DeltaSulfurPerGas = -0.000003, DeltaSulfurPerCoke = -0.000003 });
        }

        private void DeleteFurnace()
        {
            var result = MessageBox.Show($"Удалить печь №{SelectedFurnace.Id}?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Furnaces.Remove(SelectedFurnace);
                for (int i = 0; i < Furnaces.Count; i++) Furnaces[i].Id = i + 1;
            }
        }
        #region-- Рассчет оптимизации --
        private void Calculate()
        {
            if (Furnaces == null || Furnaces.Count == 0) return;

            string normGas = GasPrice?.Replace(",", ".") ?? "0";
            string normCoke = CokePrice?.Replace(",", ".") ?? "0";

            if (!double.TryParse(normGas, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedGasPrice) ||
                !double.TryParse(normCoke, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedCokePrice))
            {
                MessageBox.Show("Пожалуйста, введите корректные числа для цен.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var solver = new GasDistributionSolver();
            var result = solver.Solve(Furnaces, parsedCokePrice, parsedGasPrice);

            if (result.IsSuccess)
            {
                var resultWin = new ResultWindow(result);
                resultWin.ShowDialog();
            }
            else
            {
                MessageBox.Show(result.StatusMessage, "Ошибка");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

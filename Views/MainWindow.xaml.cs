using GasDistributionOptimizer.ViewModels;
using System.Windows;

namespace GasDistributionOptimizer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }
    }
}
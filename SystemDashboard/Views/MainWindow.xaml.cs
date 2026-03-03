using System.Windows;
using SystemDashboard.ViewModels;

namespace SystemDashboard.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        public int Property
        {
            get => default;
            set
            {
            }
        }
    }
}
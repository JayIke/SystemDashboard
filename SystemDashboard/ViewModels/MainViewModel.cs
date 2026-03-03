using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using SystemDashboard.Services;

namespace SystemDashboard.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly SystemMonitorService _monitor;
        private readonly DispatcherTimer _timer;

        private double _cpuUsage;
        public double CpuUsage
        {
            get => _cpuUsage;
            set { _cpuUsage = value; OnPropertyChanged(); }
        }

        private double _ramUsage;
        public double RamUsage
        {
            get => _ramUsage;
            set { _ramUsage = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _monitor = new SystemMonitorService();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += UpdateStats;
            _timer.Start();
        }

        private void UpdateStats(object? sender, EventArgs e)
        {
            CpuUsage = _monitor.GetCpuUsage();
            RamUsage = _monitor.GetRamUsage();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
using ControllerSimulation.App.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ControllerSimulation.App.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            PIDController.Regelgrosse = 50;
            refreshTimer.Elapsed += (async (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    Refresh();
                });
            });
        }

        private System.Timers.Timer refreshTimer = new System.Timers.Timer()
        {
            Interval = 100,
            Enabled = true,
            AutoReset = true,
        };

        public PIDController PIDController { get; set; } = new PIDController();


        public double OutputLevel
        {
            get => _outputLevel;
            set { Set(ref _outputLevel, value); }
        }
        private double _outputLevel = 0;

        public double IOQuanity = 1; // [l/s]

        private void Refresh()
        {
            double reservoir = PIDController.Regelgrosse;
            reservoir += (PIDController.Stellgrosse - OutputLevel) * IOQuanity / 10;
            if (reservoir < 0) reservoir = 0;
            else if (reservoir > 100) reservoir = 100;
            PIDController.Regelgrosse = reservoir;
        }
    }
}

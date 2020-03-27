using ControllerSimulation.App.Controllers;
using ControllerSimulation.App.Elements;
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

        public double OutputQuantity
        {
            get => _outputQuantity;
            set { Set(ref _outputQuantity, value); }
        }
        private double _outputQuantity = 0;


        public double InputQunatity
        {
            get => _inputQuantity;
            set { Set(ref _inputQuantity, value); }
        }
        private double _inputQuantity = 0;




        public double OutputLevel
        {
            get => _outputLevel;
            set { Set(ref _outputLevel, value); }
        }
        private double _outputLevel = 0;


        public double ReservoirLevel
        {
            get => _reservoirLevel;
            set { Set(ref _reservoirLevel, value); }
        }
        private double _reservoirLevel;

        public double reservoirCapacity = 1000; // [l]

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

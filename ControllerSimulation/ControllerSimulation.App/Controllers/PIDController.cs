﻿using ControllerSimulation.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ControllerSimulation.App.Controllers
{
    public class PIDController : BindableBase
    {
        public PIDController()
        {
            refreshTimer.Elapsed += (async (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    StellgrosseAnpassen();
                });
            });
        }


        private System.Timers.Timer refreshTimer = new System.Timers.Timer()
        {
            Enabled = false,
            AutoReset = true,
        };

        
        public bool PAnteilAktiv
        {
            get => _pAnteilAktiv;
            set { Set(ref _pAnteilAktiv, value); }
        }
        private bool _pAnteilAktiv = true;


        public bool IAnteilAktiv
        {
            get => _iAnteilAktiv;
            set { Set(ref _iAnteilAktiv, value); }
        }
        private bool _iAnteilAktiv = true;


        public bool DAnteilAktiv
        {
            get => _dAnteilAktiv;
            set { Set(ref _dAnteilAktiv, value); }
        }
        private bool _dAnteilAktiv = true;


        /// <summary>
        /// Der Soll-Wert der Regelstrecke. <para>Abkurzung: SW</para>
        /// </summary>
        public double Fuhrungsgrosse
        {
            get => _fuhrungsgrosse;
            set { Set(ref _fuhrungsgrosse, value); }
        }
        private double _fuhrungsgrosse = 50;


        /// <summary>
        /// Der momentane Ist-Wert der Regelstrecke. <para>Abkurzung: IW</para>
        /// </summary>
        public double Regelgrosse
        {
            get => _regelgrosse;
            set { Set(ref _regelgrosse, value); }
        }
        private double _regelgrosse;


        public double Proportionalbeiwert
        {
            get => _proportionalbeiwert;
            set { Set(ref _proportionalbeiwert, value); }
        }
        private double _proportionalbeiwert = 1;


        public double Nachstellzeit
        {
            get => _nachstellzeit;
            set { Set(ref _nachstellzeit, value); }
        }
        private double _nachstellzeit = 1;


        public double Vorhaltzeit
        {
            get => _vorhaltzeit;
            set { Set(ref _vorhaltzeit, value); }
        }
        private double _vorhaltzeit = 1;


        
        public double Abtastzeit
        {
            get => _abtastzeit;
            set
            {
                if(_abtastzeit != value && value >= 0.1)
                {
                    _abtastzeit = value;
                    refreshTimer.Interval = value * 1000;
                    OnPropertyChanged();
                }
            }
        }
        private double _abtastzeit = 0.1;


        /// <summary>
        /// 
        /// </summary>
        public double Stellgrosse
        {
            get => _stellgrosse;
            private set { Set(ref _stellgrosse, value); }
        }
        private double _stellgrosse;


        /// <summary>
        /// Wert der vorherigen Regeldifferenz <para>Abkurzung: EK1</para>
        /// </summary>
        private double regeldifferenzVorher;


        /// <summary>
        /// Summe aller Regeldiffernezen <para>Abkurzung: ESUM</para>
        /// </summary>
        private double regeldifferenzSumme;

        /// <summary>
        /// Startet die asynchrone Aktualisierung der Stellgrosse anhand der eingestellten Eigenschaften.
        /// </summary>
        public void Start()
        {
            refreshTimer.Enabled = true;
        }


        /// <summary>
        /// Stoppt die asynchrone Aktualisierung der Stellgrosse.
        /// </summary>
        public void Stop()
        {
            refreshTimer.Enabled = false;
            Reset();
        }


        /// <summary>
        /// Setzt den internen Cache zuruck.
        /// </summary>
        public void Reset()
        {
            Stellgrosse = 0;
            regeldifferenzSumme = 0;
            regeldifferenzVorher = 0;
        }


        private void StellgrosseAnpassen()
        {
            double stellgrosse = 0;
            double regeldifferenz = Proportionalbeiwert * (Fuhrungsgrosse - Regelgrosse);
            regeldifferenzSumme += regeldifferenz;
            regeldifferenzVorher = regeldifferenz;
            if (PAnteilAktiv)
            {
                stellgrosse += regeldifferenz;
            }
            if (IAnteilAktiv)
            {
                double IAnteil = regeldifferenzSumme * Abtastzeit / Nachstellzeit;
                stellgrosse += IAnteil;
            }
            if (DAnteilAktiv)
            {
                double DAnteil = Vorhaltzeit * (regeldifferenz - regeldifferenzVorher) / Abtastzeit;
                stellgrosse += DAnteil;
            }
            if (stellgrosse < 0) Stellgrosse = 0;
            else if (stellgrosse > 100) Stellgrosse = 100;
            else Stellgrosse = stellgrosse;
        }

    }
}

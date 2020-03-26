using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerSimulation.App.Controllers
{
    public class PIDController : BindableBase
    {
        public PIDController()
        {
            timer.Elapsed += ((object sender, System.Timers.ElapsedEventArgs e) =>
            {
                StellgrösseAnpassen();
            });
        }


        private System.Timers.Timer timer = new System.Timers.Timer()
        {
            Enabled = false,
            AutoReset = true,
        };

        
        public bool PAnteilAktiv
        {
            get => _pAnteilAktiv;
            set { Set(ref _pAnteilAktiv, value); }
        }
        private bool _pAnteilAktiv;


        public bool IAnteilAktiv
        {
            get => _iAnteilAktiv;
            set { Set(ref _iAnteilAktiv, value); }
        }
        private bool _iAnteilAktiv;


        public bool DAnteilAktiv
        {
            get => _dAnteilAktiv;
            set { Set(ref _dAnteilAktiv, value); }
        }
        private bool _dAnteilAktiv;


        /// <summary>
        /// Der Soll-Wert der Regelstrecke. <para>Abkürzung: SW</para>
        /// </summary>
        public double Führungsgrösse
        {
            get => _führungsgrösse;
            set { Set(ref _führungsgrösse, value); }
        }
        private double _führungsgrösse;


        /// <summary>
        /// Der momentane Ist-Wert der Regelstrecke. <para>Abkürzung: IW</para>
        /// </summary>
        public double Regelgrösse
        {
            get => _regelgrösse;
            set { Set(ref _regelgrösse, value); }
        }
        private double _regelgrösse;


        public double Proportionalbeiwert
        {
            get => _proportionalbeiwert;
            set { Set(ref _proportionalbeiwert, value); }
        }
        private double _proportionalbeiwert;


        public double Nachstellzeit
        {
            get => _nachstellzeit;
            set { Set(ref _nachstellzeit, value); }
        }
        private double _nachstellzeit;


        public double Vorhaltezeit
        {
            get => _vorhaltezeit;
            set { Set(ref _vorhaltezeit, value); }
        }
        private double _vorhaltezeit;


        
        public double Abtastzeit
        {
            get => _abtastzeit;
            set
            {
                if(_abtastzeit != value && value >= 100)
                {
                    _abtastzeit = value;
                    timer.Interval = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _abtastzeit { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public double Stellgrösse
        {
            get => _stellgrösse;
            private set { Set(ref _stellgrösse, value); }
        }
        private double _stellgrösse;


        /// <summary>
        /// Wert der vorherigen Regeldifferenz <para>Abkürzung: EK1</para>
        /// </summary>
        private double regeldifferenzVorher;


        /// <summary>
        /// Summe aller Regeldiffernezen <para>Abkürzung: ESUM</para>
        /// </summary>
        private double regeldifferenzSumme;

        /// <summary>
        /// Startet die asynchrone Aktualisierung der Stellgrösse anhand der eingestellten Eigenschaften.
        /// </summary>
        public void Start()
        {

        }


        /// <summary>
        /// Stoppt die asynchrone Aktualisierung der Stellgrösse.
        /// </summary>
        public void Stop()
        {

        }


        /// <summary>
        /// Setzt den internen Cache zurück.
        /// </summary>
        public void Reset()
        {
            Stellgrösse = 0;
            regeldifferenzSumme = 0;
            regeldifferenzVorher = 0;
        }


        private void StellgrösseAnpassen()
        {
            double stellgrösse = 0;
            double regeldifferenz = Proportionalbeiwert * (Führungsgrösse - Regelgrösse);
            regeldifferenzSumme += regeldifferenz;
            regeldifferenzVorher = regeldifferenz;
            if (PAnteilAktiv)
            {
                stellgrösse += regeldifferenz;
            }
            if (IAnteilAktiv)
            {
                double IAnteil = regeldifferenzSumme * Abtastzeit / Nachstellzeit;
                stellgrösse += IAnteil;
            }
            if (DAnteilAktiv)
            {
                double DAnteil = Vorhaltezeit * (regeldifferenz - regeldifferenzVorher) / Abtastzeit;
                stellgrösse += DAnteil;
            }
            Stellgrösse = stellgrösse;
        }

    }
}

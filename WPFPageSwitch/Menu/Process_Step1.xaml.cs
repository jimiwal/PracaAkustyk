using MeasurementDomain.Services.DomainLayer;
using NHibernate.Util;
using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.Entities;
using SoundDomain.Model.ValueObjects;
using SoundDomain.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UserDomain.Services.DomainLayer;

namespace WPFPageSwitch
{
	public partial class Process_Step1 : UserControl, ISwitchable, INotifyPropertyChanged, System.ComponentModel.IDataErrorInfo
    {
        private string measurementName;

        public Process_Step1()
		{
			InitializeComponent();

            AutoPlayDelay = "3";

            SoundsInSequence = new ObservableCollection<Sound>();
            Sequences = new ObservableCollection<SoundSequence>();

            this.Loaded += Process_Step1_Loaded;
            nextBtn.IsEnabled = false;
            this.DataContext = this;
        }

        public System.Collections.ObjectModel.ObservableCollection<SoundSequence> Sequences { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Sound> SoundsInSequence { get; set; }

        public string MeasurementName
        {
            get
            {
                return measurementName;
            }

            set
            {
                measurementName = value;
                OnPropertyChanged("MeasurementName");             
            }
        }

        private SoundSequence selectedSequence;
        public SoundSequence SelectedSequence
        {
            get
            {
                return selectedSequence;
            }
            set
            {
                selectedSequence = value;
                OnPropertyChanged("SelectedSequence");

                SetSquenceInListView();
            }
        }

        private string autoPlayDelay;
        public string AutoPlayDelay
        {
            get
            {
                return autoPlayDelay;
            }
            set
            {
                autoPlayDelay = value;
                OnPropertyChanged("AutoPlayDelay");
            }
        }
        
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {            
            get
            {
                string error = string.Empty;
                bool isValid = true;
                switch (columnName)
                {
                    case "AutoPlayDelay":
                        var validation = new StringToIntValidationRule().Validate(AutoPlayDelay, System.Globalization.CultureInfo.CurrentCulture);
                        isValid = isValid && validation.IsValid;
                        error = (string)validation.ErrorContent;
                    break;
                }

                nextBtn.IsEnabled = isValid;
                return error;
            }
        }

        void OnPropertyChanged(String prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	Switcher.Switch(new MainMenu());
        }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(SoundsInSequence != null && SoundsInSequence.Count > 0)
            {
                MeasurementService.LastMeasurementName = MeasurementName;

                int delayAutoPlay = 5;
                delayAutoPlay = int.Parse(AutoPlayDelay);

                Switcher.Switch(new Process_Step2(SoundsInSequence, delayAutoPlay));
            }   
        }

        private void Process_Step1_Loaded(object sender, RoutedEventArgs e)
        {
            if(UserService.SelectedUser == null)
            {
                SoundsInSequence.Clear();
            }
            else
            {
                LoadSequences();

                SoundsInSequence.Clear();
                var sounds = SoundServiceSingleton.Instance.GetSoundsForUser(UserService.SelectedUser);                
                sounds.ForEach(x => SoundsInSequence.Add(x));

                if(sounds.Count > 0)
                {
                    nextBtn.IsEnabled = true;
                }                
            }            
        }

        void LoadSequences()
        {
            Sequences.Clear();
            var soundSequences = SoundServiceSingleton.Instance.GetAllSoundSequences();
            soundSequences.ForEach(x => Sequences.Add(x));
        }

        void SetSquenceInListView()
        {
            if (selectedSequence == null) return;

            SoundsInSequence.Clear();
            SelectedSequence.Sounds.ForEach(x => SoundsInSequence.Add(x.Sound));

            MeasurementName = SelectedSequence.Name + " " + DateTime.Now.ToString();
        }
    }
}
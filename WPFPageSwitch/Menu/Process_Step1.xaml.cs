using MeasurementDomain.Services.DomainLayer;
using NHibernate.Util;
using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.Entities;
using SoundDomain.Model.ValueObjects;
using SoundDomain.Services;
using System;
using System.Collections.ObjectModel;
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
	public partial class Process_Step1 : UserControl, ISwitchable
	{
        private string measurementName;

        public Process_Step1()
		{
			InitializeComponent();

            AvailableSounds = new ObservableCollection<Sound>();

            this.Loaded += Process_Step1_Loaded;
            nextBtn.IsEnabled = false;

            this.DataContext = this;
        }        

        public System.Collections.ObjectModel.ObservableCollection<Sound> AvailableSounds { get; set; }

        public string MeasurementName
        {
            get
            {
                return measurementName;
            }

            set
            {
                measurementName = value;                
            }
        }

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
            MeasurementService.LastMeasurementName = MeasurementName;
            Switcher.Switch(new Process_Step2());
        }

        private void Process_Step1_Loaded(object sender, RoutedEventArgs e)
        {
            if(UserService.SelectedUser == null)
            {
                AvailableSounds.Clear();
            }
            else
            {
                AvailableSounds.Clear();
                var sounds = SoundServiceSingleton.Instance.GetSoundsForUser(UserService.SelectedUser);                
                sounds.ForEach(x => AvailableSounds.Add(x));

                if(sounds.Count > 0)
                {
                    nextBtn.IsEnabled = true;
                }
            }            
        }
    }
}
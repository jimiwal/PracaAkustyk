using MeasurementDomain.Infrastructure.Repositories;
using MeasurementDomain.Model.Entities;
using MeasurementDomain.Services.DomainLayer;
using NHibernate.Util;
using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.Entities;
using SoundDomain.Model.ValueObjects;
using SoundDomain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
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
	public partial class MyProcesses : UserControl, ISwitchable, INotifyPropertyChanged
    {
        private string measurementName;

        public MyProcesses()
		{
			InitializeComponent();

            MyMeasurements = new ObservableCollection<Measurement>();
            this.Loaded += MyProcesses_Loaded;         
            this.DataContext = this;
        }

        private void MyProcesses_Loaded(object sender, RoutedEventArgs e)
        {
            var currentUser = UserService.SelectedUser;
            var measurements = MeasurementServiceSingleton.Instance.GetMeasurementsForUser(currentUser);

            MyMeasurements.Clear();
            measurements.ForEach(x => MyMeasurements.Add(x));
        }
        public System.Collections.ObjectModel.ObservableCollection<Measurement> MyMeasurements { get; set; }

        public Measurement SelectedMeasurement { get; set; }

        public IList SelectedItems { get; set; }

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


        private void button_Click_2(object sender, RoutedEventArgs e)
        {//Back to menu
            Switcher.Switch(new MainMenu());
        }
        
        private void button1_Click(object sender, RoutedEventArgs e)
        {//Usuń badanie
            if(SelectedMeasurement != null)
            {
                IList< Measurement > listOfMeasurements = SelectedItems as IList<Measurement>;
                foreach (var item in SelectedItems.Cast<Measurement>().ToList())
                {
                    MeasurementServiceSingleton.Instance.RemoveMeasuremant(item as Measurement);
                    MyMeasurements.Remove(SelectedMeasurement);
                }
                
            }            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
using MeasurementDomain.Infrastructure.Repositories;
using MeasurementDomain.Model.Entities;
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
	public partial class MyProcesses : UserControl, ISwitchable
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
    }
}
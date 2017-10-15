using MeasurementDomain.Infrastructure.Repositories;
using MeasurementDomain.Model.Entities;
using MeasurementDomain.Services.DomainLayer;
using Remotion.Linq.Collections;
using SoundDomain.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserDomain.Services.DomainLayer;

namespace WPFPageSwitch.Menu
{
    /// <summary>
    /// Interaction logic for AfterSpundHear.xaml
    /// </summary>
    public partial class AfterSpundHear : UserControl, ISwitchable, INotifyPropertyChanged
    {
        Measurement measurement;
        private int idOfMeasurement = 0;
        public AfterSpundHear(int id)
        {
            SoundHeard = new ObservableCollection<SoundDomain.Model.Entities.SoundHeard>();

            InitializeComponent();
            this.DataContext = this;
            idOfMeasurement = id;

            this.Loaded += AfterSpundHear_Loaded;            
        }

        public string UserName
        {
            get
            {
                return string.Format("{0} - {1}", UserService.SelectedUser.Name, UserService.SelectedUser.Email);
            }
        }

        public string MeasurementName
        {
            get
            {
                return MeasurementService.LastMeasurementName;
            }
        }

        public ObservableCollection<SoundHeard> SoundHeard { get; set; }

        private void AfterSpundHear_Loaded(object sender, RoutedEventArgs e)
        {
            measurement = MeasurementRepositorySingleton.Instance.Get(idOfMeasurement);
            
            measurement.Sounds.ToList().ForEach(x => SoundHeard.Add(x));
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        //start again
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
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

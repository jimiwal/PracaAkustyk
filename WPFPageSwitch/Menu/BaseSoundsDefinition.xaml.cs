using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.ValueObjects;
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
    /// Interaction logic for BaseSoundsDefinition.xaml
    /// </summary>
    public partial class BaseSoundsDefinition : UserControl, ISwitchable, INotifyPropertyChanged
    {
        public BaseSoundsDefinition()
        {
            InitializeComponent();


            AvailableSounds = new System.Collections.ObjectModel.ObservableCollection<Sound>();
            this.Loaded += BaseSoundsDefinition_Loaded;
        }

        private void BaseSoundsDefinition_Loaded(object sender, RoutedEventArgs e)
        {
            var sounds = SoundRepositorySingleton.Instance.FindAll().ToList();
            sounds.ForEach(x => AvailableSounds.Add(x));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(String prop)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private Sound selectedSoundGeneral;
        public Sound SelectedSoundGeneral
        {
            get
            {
                return selectedSoundGeneral;
            }
            set
            {
                selectedSoundGeneral = value;
                OnPropertyChanged("SelectedSoundGeneral");
            }
        }

        public string SoundInProcessForUser
        {
            get
            {
                if (UserService.SelectedUser == null)
                {
                    return string.Format("Nie wybrano użytkownika w oknie głównym");
                }
                else
                {
                    return string.Format("Dźwięki użytkownika : {0} - {1} ", UserService.SelectedUser.Name, UserService.SelectedUser.Email);
                }
            }
        }

        public System.Collections.ObjectModel.ObservableCollection<Sound> AvailableSounds { get; set; }

        //Add Sound to list new
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            double frequency = Convert.ToDouble(textBox.Text);
            double volume = Convert.ToDouble(textBox_Copy.Text);
            string name = textBox_Copy1.Text;
            var sound = new Sound()
            {
                Frequency = frequency,
                Volume = volume,
                Name = name
            };

            SoundRepositorySingleton.Instance.Save(sound);
            SoundRepositorySingleton.Instance.Flush();

            AvailableSounds.Add(sound);
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
                return;

            var sound = listView.SelectedItem as Sound;

            SoundRepositorySingleton.Instance.Remove(sound);
            SoundRepositorySingleton.Instance.Flush();

            AvailableSounds.Remove(sound);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }

    }
}

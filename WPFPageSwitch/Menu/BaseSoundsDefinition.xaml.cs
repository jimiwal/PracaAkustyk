using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            this.DataContext = this;
        }

        private void BaseSoundsDefinition_Loaded(object sender, RoutedEventArgs e)
        {
            SoundsCount = 1;
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

        private int soundsCount;
        public int SoundsCount
        {
            get
            {
                return soundsCount;
            }
            set
            {
                soundsCount = value;
                OnPropertyChanged("SoundsCount");
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
            if(SoundsCount == 1)
            {
                try
                {
                    AddSound(textBox_Copy1.Text, Convert.ToDouble(textBox.Text), Convert.ToDouble(textBox_Copy.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd w trakcie dodawania dźwięku");
                    return;
                }
            }
            else if( SoundsCount > 1)
            {
                int step = 100 / SoundsCount;
                for (int i = 1; i <= SoundsCount; i++)
                {
                    AddSound(textBox_Copy1.Text, Convert.ToDouble(textBox.Text), step * i);
                }
            }            
        }

        private void AddSound(string nameOfSound, double freq, double vol)
        {
            Sound sound = null;
            double frequency = freq;
            double volume = vol;
            string name = nameOfSound;
            sound = new Sound()
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
            List<Sound> selectedSoundsCopy = new List<Sound>();
            foreach (var selectedSound in listView.SelectedItems)
            {
                selectedSoundsCopy.Add(selectedSound as Sound);
            }

            foreach (var selectedSound in selectedSoundsCopy)
            {
                var sound = selectedSound;

                SoundRepositorySingleton.Instance.Remove(sound);
                SoundRepositorySingleton.Instance.Flush();

                AvailableSounds.Remove(sound);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
    }
}

using SoundDomain.Model.Entities;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using SoundDomain.Model.ValueObjects;
using SoundDomain.Infrastructure.Repositories;
using UserDomain.Services.DomainLayer;
using System.ComponentModel;
using System.Collections.Generic;
using NHibernate.Transform;
using NHibernate.Util;
using SoundDomain.Services;

namespace WPFPageSwitch
{
	public partial class Option : UserControl, ISwitchable, INotifyPropertyChanged
    {
        private Sound selectedSoundGeneral;

        public Option()
		{
			// Required to initialize variables
			InitializeComponent();
            AvailableSounds = new ObservableCollection<Sound>();
            SoundsInProcess = new ObservableCollection<Sound>();

            this.Loaded += Option_Loaded;
            this.DataContext = this;
        }

        public bool IsUserSelected
        {
            get
            {
                return UserService.SelectedUser != null && SelectedSoundGeneral != null;
            }
        }

        public Sound SelectedSoundGeneral
        {
            get
            {
                return selectedSoundGeneral;
            }
            set
            {
                selectedSoundGeneral = value;
                OnPropertyChanged("IsUserSelected");
            }
        }

        public string SoundInProcessForUser
        {
            get
            {
                if(UserService.SelectedUser == null)
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

        public System.Collections.ObjectModel.ObservableCollection<Sound> SoundsInProcess { get; set; }

        private void Option_Loaded(object sender, RoutedEventArgs e)
        {
            var sounds = SoundRepositorySingleton.Instance.FindAll().ToList();
            sounds.ForEach(x => AvailableSounds.Add(x));

            var userSoundList = SoundServiceSingleton.Instance.GetSoundsForUser(UserService.SelectedUser);
            userSoundList.ForEach(x => SoundsInProcess.Add(x));
        }

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        
        //back, save changes
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(UserService.SelectedUser != null)
            {
                SoundServiceSingleton.Instance.RemoveAllSoundsForUser(UserService.SelectedUser);

                foreach (var sound in SoundsInProcess)
                {
                    SoundSetting soundSetting = new SoundSetting() { Sound = sound, User = UserService.SelectedUser };

                    SoundServiceSingleton.Instance.SaveSoundSettings(soundSetting);                    
                }
            }            
            Switcher.Switch(new MainMenu());
        }
        #endregion

        //move to sound in process
        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null) return;

            var selectedSound = listView.SelectedItem as Sound;
            int indexOf = SoundsInProcess.IndexOf(selectedSound);

            if(indexOf == -1)
            {
                SoundsInProcess.Add(selectedSound);
            }

        }

        //remove sound from process
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (listViewInProcess.SelectedItem == null) return;

            var selectedSound = listViewInProcess.SelectedItem as Sound;
            SoundsInProcess.Remove(selectedSound);
        }

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
            AvailableSounds.Add(sound);
            SoundRepositorySingleton.Instance.Flush();
        }

        //remove sound from list general
        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
                return;

            var sound = listView.SelectedItem as Sound;

            AvailableSounds.Remove(sound);

            SoundRepositorySingleton.Instance.Remove(sound);
            SoundRepositorySingleton.Instance.Flush();
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
    }
}
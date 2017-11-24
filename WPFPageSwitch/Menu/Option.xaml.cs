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
            SoundsInSequence = new ObservableCollection<Sound>();
            Sequences = new ObservableCollection<SoundSequence>();

            this.Loaded += Option_Loaded;
            this.DataContext = this;
        }

        private ICommand _clickCommand;
        public ICommand DeleteSoundFromSequence
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => DeleteSelectedSoundFromSequenceAction(), true));
            }
        }
        public void DeleteSelectedSoundFromSequenceAction()
        {
            if(SelectedSoundInSequence != null)
            {
                SoundsInSequence.Remove(SelectedSoundInSequence);
                SelectedSoundInSequence = null;
            }
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

        private string sequenceName;
        public string SequenceName
        {
            get
            {
                return sequenceName;
            }
            set
            {
                sequenceName = value;
                OnPropertyChanged("SequenceName");
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

        private Sound selectedSoundInSequence;
        public Sound SelectedSoundInSequence
        {
            get
            {
                return selectedSoundInSequence;
            }
            set
            {
                selectedSoundInSequence = value;
                OnPropertyChanged("SelectedSoundInSequence");                
            }
        }

        bool? isUserSequence;
        public bool? IsUserSequence
        {
            get
            {
                return isUserSequence;
            }
            set
            {
                isUserSequence = value;
                OnPropertyChanged("IsUserSequence");
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
        public System.Collections.ObjectModel.ObservableCollection<Sound>  SoundsInSequence { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<SoundSequence> Sequences { get;set;}

    private void Option_Loaded(object sender, RoutedEventArgs e)
        {
            var sounds = SoundRepositorySingleton.Instance.FindAll().ToList();
            sounds.ForEach(x => AvailableSounds.Add(x));

            LoadSequences();
        }

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        
        //back, save changes
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {            
            Switcher.Switch(new MainMenu());
        }
        #endregion

        //move sound to sequence
        private void button_Click_1(object sender, RoutedEventArgs e)
        {

            if (listView.SelectedItem == null) return;

            var selectedSound = listView.SelectedItem as Sound;
            int indexOf = SoundsInSequence.IndexOf(selectedSound);

            if(indexOf == -1)
            {
                SoundsInSequence.Add(selectedSound);
            }

        }

        //remove sequence
        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }


        //remove sound from list general
        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null)
                return;

            var sound = listView.SelectedItem as Sound;            

            SoundRepositorySingleton.Instance.Remove(sound);
            SoundRepositorySingleton.Instance.Flush();

            AvailableSounds.Remove(sound);
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

        //save sequence
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSequence == null)
            {
                //SoundServiceSingleton.Instance.RemoveAllSoundsForUser(UserService.SelectedUser);
                SoundSequence soundSequence = new SoundSequence();
                foreach (var sound in SoundsInSequence)
                {
                    SoundSetting soundSetting = new SoundSetting() { Sound = sound };
                    soundSequence.Sounds.Add(soundSetting);
                }

                if(IsUserSequence.HasValue && IsUserSequence.Value == true && UserService.SelectedUser != null)
                {
                    soundSequence.User = UserService.SelectedUser;
                }
                else
                {
                    soundSequence.User = null;
                }

                soundSequence.Name = SequenceName;

                SoundServiceSingleton.Instance.SaveSoundSequence(soundSequence);                
            }
            else
            {
                SelectedSequence.Name = SequenceName;
                SelectedSequence.Sounds.Clear();
                SoundsInSequence.ForEach(x => SelectedSequence.Sounds.Add(new SoundSetting() { Sound = x }));

                if (IsUserSequence.HasValue && IsUserSequence.Value == true && UserService.SelectedUser != null)
                {
                    SelectedSequence.User = UserService.SelectedUser;
                }
                else
                {
                    SelectedSequence.User = null;
                }

                SoundServiceSingleton.Instance.SaveSoundSequence(SelectedSequence);
            }
        }

        //
        void SetSquenceInListView()
        {
            SoundsInSequence.Clear();
            SelectedSequence.Sounds.ForEach(x => SoundsInSequence.Add(x.Sound));

            SequenceName = SelectedSequence.Name;
        }

        void LoadSequences()
        {
            Sequences.Clear();
            var soundSequences = SoundServiceSingleton.Instance.GetAllSoundSequences();
            soundSequences.ForEach(x => Sequences.Add(x));
        }
    }
}
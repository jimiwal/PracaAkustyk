﻿using NHibernate.Util;
using SoundDomain.Infrastructure.Repositories;
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
using WPFPageSwitch.Menu;

namespace WPFPageSwitch
{
	public partial class Process_Step2 : UserControl, ISwitchable
	{
        private int index = 0;
        private float frequency = 0;
        private float volume = 0;
        ISoundService soundServiceSingleton;
        
        RadioButton selectedCheckbox;
        public Process_Step2()
		{
			InitializeComponent();
            AvailableSounds = new ObservableCollection<Sound>();            

            this.DataContext = this;

            soundServiceSingleton = SoundServiceSingleton.Instance;

            this.Loaded += Process_Step2_Loaded; ;

        }

        public System.Collections.ObjectModel.ObservableCollection<Sound> AvailableSounds { get; set; }

        private void Process_Step2_Loaded(object sender, RoutedEventArgs e)
        {
            nextBtn.IsEnabled = false;
            radioStackPanel.IsEnabled = false;
            playBtn.IsEnabled = true;
            index = 0;
            var sounds = soundServiceSingleton.GetSoundsForUser(UserService.SelectedUser);//SoundRepositorySingleton.Instance.FindAll().ToList();
            sounds.ForEach(x => AvailableSounds.Add(x));

            freqTxt.Text = AvailableSounds[index].Frequency.ToString();
            frequency = (float)AvailableSounds[index].Frequency;

            VolumeTxt.Text = AvailableSounds[index].Volume.ToString();
            volume = (float)(AvailableSounds[index].Volume / 100);

            NameTxt.Text = AvailableSounds[index].Name.ToString();
        }

        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	Switcher.Switch(new Process_Step1());
        }
        #endregion

        //next btn
        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            soundServiceSingleton.Stop();
            if(selectedCheckbox != null)
            {
                selectedCheckbox.IsChecked = false;
            }

            index++;
            if (AvailableSounds.Count <= index)
            {
                Switcher.Switch(new AfterSpundHear());
                return;
            }

            freqTxt.Text = AvailableSounds[index].Frequency.ToString();
            frequency = (float)AvailableSounds[index].Frequency;

            VolumeTxt.Text = AvailableSounds[index].Volume.ToString();
            volume = (float)(AvailableSounds[index].Volume / 100);

            NameTxt.Text = AvailableSounds[index].Name.ToString();

            playBtn.IsEnabled = true;
            radioStackPanel.IsEnabled = false;
            nextBtn.IsEnabled = false;
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            radioStackPanel.IsEnabled = true;
            playBtn.IsEnabled = false;
            soundServiceSingleton.Play(frequency, volume);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            nextBtn.IsEnabled = true;
            selectedCheckbox = sender as RadioButton;
        }
    }
}
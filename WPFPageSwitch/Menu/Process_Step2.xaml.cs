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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UserDomain.Model.Entities;
using UserDomain.Services.DomainLayer;
using WPFPageSwitch.Menu;

namespace WPFPageSwitch
{
    //Measurement process
	public partial class Process_Step2 : UserControl, ISwitchable
	{
        private readonly int _delayAutoPlay = 5;
        private int index = 0;
        private float frequency = 0;
        private float volume = 0;
        ISoundService soundServiceSingleton;
        private CancellationTokenSource source;        

        Measurement myCurrentMeasurement;
        public Process_Step2(ObservableCollection<Sound> availableSounds, int delayAutoPlay)
		{

            InitializeComponent();
            AvailableSounds = availableSounds;

            this.DataContext = this;

            soundServiceSingleton = SoundServiceSingleton.Instance;

            _delayAutoPlay = delayAutoPlay;

            this.Loaded += Process_Step2_Loaded;
            this.Unloaded += Process_Step2_Unloaded;      
        }

        private void Process_Step2_Unloaded(object sender, RoutedEventArgs e)
        {
            if(source != null)
            {
                source.Dispose();
            }
            
            this.Loaded -= Process_Step2_Loaded;
            this.Unloaded -= Process_Step2_Unloaded;            
        }

        public System.Collections.ObjectModel.ObservableCollection<Sound> AvailableSounds { get; set; }

        private void Process_Step2_Loaded(object sender, RoutedEventArgs e)
        {            
            nextBtn.IsEnabled = false;
            radioStackPanel.IsEnabled = false;
            playBtn.IsEnabled = true;
            index = 0;
            freqTxt.Text = AvailableSounds[index].Frequency.ToString();
            frequency = (float)AvailableSounds[index].Frequency;

            //VolumeTxt.Text = AvailableSounds[index].Volume.ToString();
            volume = (float)(AvailableSounds[index].Volume / 100);

            NameTxt.Text = AvailableSounds[index].Name.ToString();

            CreateMeasurementObject();
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
        private async void button_Click_1(object sender, RoutedEventArgs e)
        {
            soundServiceSingleton.Stop();
            SaveAnswer();

            index++;
            if (AvailableSounds.Count <= index)
            {
                int dbId = SaveMeasurement();
                Switcher.Switch(new AfterSpundHear(dbId));
                return;
            }

            //clear answear
            answerSlider.Value = 0;

            freqTxt.Text = AvailableSounds[index].Frequency.ToString();
            frequency = (float)AvailableSounds[index].Frequency;

            //VolumeTxt.Text = AvailableSounds[index].Volume.ToString();
            volume = (float)(AvailableSounds[index].Volume / 100);

            NameTxt.Text = AvailableSounds[index].Name.ToString();

            playBtn.IsEnabled = true;
            radioStackPanel.IsEnabled = false;
            nextBtn.IsEnabled = false;

            source = new CancellationTokenSource();
            var cancellationToken = source.Token;
            await Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(_delayAutoPlay * 1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                                               
            });

            if(playBtn.IsEnabled)
            {
                PlayNextSound();
            }
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            if(source !=null && source.Token!= null && source.Token.CanBeCanceled)
            {
                source.Cancel();
            }

            PlayNextSound();
        }

        private void PlayNextSound()
        {
            radioStackPanel.IsEnabled = true;
            playBtn.IsEnabled = false;
            soundServiceSingleton.Play(frequency, volume);
        }

        
        void CreateMeasurementObject()
        {
            myCurrentMeasurement = new Measurement();
            myCurrentMeasurement.User = UserService.SelectedUser;
            myCurrentMeasurement.Name = MeasurementService.LastMeasurementName;

        }

        private void SaveAnswer()
        {
            Sound currentSound = AvailableSounds[index];            
            SoundHeard soundHeard = new SoundHeard();
            soundHeard.Sound = currentSound;
            soundHeard.Answer = (int)answerSlider.Value;

            myCurrentMeasurement.Sounds.Add(soundHeard);
        }

        private int SaveMeasurement()
        {
            myCurrentMeasurement.DateTime = DateTime.Now;
            MeasurementRepositorySingleton.Instance.Save(myCurrentMeasurement);
            MeasurementRepositorySingleton.Instance.Flush();

            return myCurrentMeasurement.Id;
        }

        private void CustomSlider_SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            nextBtn.IsEnabled = true;
        }

    }
}
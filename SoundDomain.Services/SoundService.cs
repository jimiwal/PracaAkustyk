using System;
using System.Collections.Generic;
using NAudio.Wave;
using SoundDomain.Model.ValueObjects;
using UserDomain.Model.Entities;
using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.Entities;
using UserDomain.Services.DomainLayer;
using NHibernate.Transform;
using NHibernate.Util;
using RepositoryComponents.Patternts;
using NHibernate.Linq;
using System.Linq;

namespace SoundDomain.Services
{
    public class SoundService : ISoundService
    {
        private SoundService() { }

        private WaveOut waveOut;
        public void Play(float frequency, float vloume)
        {            
            var sineWaveProvider = new SineWaveProvider32();
            sineWaveProvider.SetWaveFormat(16000, 1); // 16kHz mono
            sineWaveProvider.Frequency = frequency;
            sineWaveProvider.Amplitude = vloume;
            waveOut = new WaveOut();
            waveOut.Init(sineWaveProvider);
            waveOut.Play();
        }

        public void Stop()
        {
            waveOut.Stop();
        }

        public IList<Sound> GetSoundsForUser(User user)
        {
            IList<Sound> userSoundList;
            var query = SoundSettingRepositorySingleton.Instance.Session.QueryOver<SoundSetting>();            

            userSoundList = query
                                .SelectList(list => list
                                .Select(pr => pr.Sound))
                                .TransformUsing(Transformers.DistinctRootEntity)
                                .List<Sound>();

            return userSoundList;
        }

        public void SaveSoundSettings(SoundSetting soundSetting)
        {
            SoundSettingRepositorySingleton.Instance.Save(soundSetting);
            SoundSettingRepositorySingleton.Instance.Flush();
        }

        public void RemoveAllSoundsForUser(User user)
        {
            var soundSettings = GetSoundsSettingForUser(user);

            soundSettings.ForEach(x =>
            {
                SoundSettingRepositorySingleton.Instance.Remove(x);
                SoundSettingRepositorySingleton.Instance.Flush();
            });            
        }

        private IList<SoundSetting> GetSoundsSettingForUser(User user)
        {
            IList<SoundSetting> userSoundList;
            var query = SoundSettingRepositorySingleton.Instance.Session.QueryOver<SoundSetting>();

            userSoundList = query.TransformUsing(Transformers.DistinctRootEntity)
                                .List<SoundSetting>();

            return userSoundList;
        }

        public void SaveSoundSequence(SoundSequence soundSequence)
        {
            SoundSequenceRepositorySingleton.Instance.Save(soundSequence);
            SoundSequenceRepositorySingleton.Instance.Flush();
        }

        public IList<SoundSequence> GetAllSoundSequences()
        {
            var query = SoundSequenceRepositorySingleton.Instance.Session.QueryOver<SoundSequence>();
            return query.TransformUsing(Transformers.DistinctRootEntity).List<SoundSequence>();
        }

        public SoundSequence GetSoundSequenceByName(string name)
        {
            var query = SoundSequenceRepositorySingleton.Instance.Session.QueryOver<SoundSequence>();
            return query.Where(x => x.Name.Equals(name)).Select(x => x).SingleOrDefault<SoundSequence>();
        }

        public IList<double> GetAllFrequencesFromBaseSounds()
        {
            var query = SoundRepositorySingleton.Instance.Session.Query<Sound>();
            var returnValue = query.Select(x => x.Frequency).Distinct().ToList();

            return returnValue;
        }

        public IList<Sound> GetSoundForFrequency(double frequency)
        {
            Sound sound = null;
            var query = SoundRepositorySingleton.Instance.Session.QueryOver<Sound>(() => sound);
            var selected = query.Where(() => sound.Frequency == frequency).OrderBy(() => sound.Volume).Asc.List();
            return selected;
        }
    }

    public sealed class SoundServiceSingleton
    {
        private static ISoundService _instance;

        public static ISoundService Instance
        {
            get { return _instance ?? SingletonBase<SoundService>.Instance; }

            set { _instance = value; }
        }
    }

    public class SineWaveProvider32 : WaveProvider32
    {
        int sample;

        public SineWaveProvider32()
        {
            Frequency = 1000;
            Amplitude = 0.25f; // let's not hurt our ears            
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
                sample++;
                if (sample >= sampleRate) sample = 0;
            }
            return sampleCount;
        }
    }
}
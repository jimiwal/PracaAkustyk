using SoundDomain.Model.Entities;
using SoundDomain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Model.Entities;

namespace SoundDomain.Services
{
    public interface ISoundService
    {
        void Play(float frequency, float vloume);
        void Stop();
        IList<Sound> GetSoundsForUser(User user);
        void SaveSoundSettings(SoundSetting soundSetting);
        void RemoveAllSoundsForUser(User user); //add

        void SaveSoundSequence(SoundSequence soundSequence);
        IList<SoundSequence> GetAllSoundSequences();
        SoundSequence GetSoundSequenceByName(string name);


        IList<Sound> GetSoundForFrequency(double frequency);
        IList<double> GetAllFrequencesFromBaseSounds();
    }
}

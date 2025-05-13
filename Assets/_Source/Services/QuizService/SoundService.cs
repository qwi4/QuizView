using UnityEngine;
using YG;

namespace Quiz.Services
{
    public class SoundService : ISound
    {
        private readonly AudioSource _audio;

        public SoundService(AudioSource audio)
        {
            _audio = audio;
            _audio.mute = YG2.saves.IsMuted;
        }

        public void Toggle()
        {
            if (YG2.saves.IsMuted)
                OnSound();
            else
                OffSound();
        }

        public void OnSound()
        {
            YG2.saves.IsMuted = false;
            _audio.mute = false;
            YG2.SaveProgress();
        }

        public void OffSound()
        {
            YG2.saves.IsMuted = true;
            _audio.mute = true;
            
            YG2.SaveProgress();
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public bool IsMuted = true;
    }
}
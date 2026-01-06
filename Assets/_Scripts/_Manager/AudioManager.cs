using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class AudioManager : MMSingleton<AudioManager>
{
     [SerializeField] SoundData soundData;
     public AudioSource bgmSource;
     public AudioSource sfxSource;
     
     private Dictionary<SoundId, SoundItem> soundMap;
     
     protected override void Awake()
     {
         base.Awake();
         soundMap = new Dictionary<SoundId, SoundItem>();
         foreach (var sound in soundData.sounds)
         {
             soundMap[sound.id] = sound;
         }
     }

     private void Start()
     {
         PlayBGM(SoundId.BGM);
     }

     public void PlaySFX(SoundId id)
        {
            if (!soundMap.TryGetValue(id, out var sound)) return;
            sfxSource.PlayOneShot(sound.clip, sound.volume);
        }
    
        public void PlayBGM(SoundId id, float volume = 1f)
        {
            if (!soundMap.TryGetValue(id, out var sound)) return;

            // Avoid restarting same BGM
            if (bgmSource.clip == sound.clip && bgmSource.isPlaying)
                return;

            bgmSource.clip = sound.clip;
            bgmSource.volume = volume;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        public void MuteBGM(bool mute)
        {
            if (bgmSource == null) return;
            bgmSource.mute = mute;
        }

        public void MuteSFX(bool mute)
        {
            if (sfxSource == null) return;
            sfxSource.mute = mute;
        }

        public void PlayClickButton()
        {
            PlaySFX(SoundId.ButtonClick);
        }

        
        public void PlayDrillSfx()
        {
            PlaySFX(SoundId.Drill);
        }
        
        public void PlayHammerSfx()
        {
            PlaySFX(SoundId.Hammer);
        }
}

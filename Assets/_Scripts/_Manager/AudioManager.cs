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
            
            bgmSource.clip = soundMap[id].clip;
            bgmSource.volume = volume;
            bgmSource.Play();
        }
    
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        public void StopSFX()
        {
            sfxSource.Stop();
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

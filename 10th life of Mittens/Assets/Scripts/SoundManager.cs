using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundFX { HitEnemy };
public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class MusicSources
    {
        public AudioSource Source;
        public bool ShouldBeMute;
    }

    [System.Serializable]
    public class SoundFXClip
    {
        public AudioClip Clip;
        public SoundFX FX;
    }

    [Header("Music")]
    [SerializeField] 
    private MusicSources[] musicSources;

    [SerializeField]
    private float musicFadeRate;

    [Header("SoundFX")]
    [SerializeField]
    private SoundFXClip[] soundFXList;

    public AudioSource soundFXSource;

    public void Toggle(bool isOn, params int[] indexes)
    {
        foreach(int i in indexes)
        {
            if(i >= 0 && i < indexes.Length)
            {
                musicSources[i].ShouldBeMute = isOn;
            }
        }

        UpdateAudioSourceState();
    }

    public void PlayOnce(SoundFX fx)
    {
        AudioClip clip = null;
        foreach(var entry in soundFXList)
        {
            if(entry.FX == fx)
            {
                clip = entry.Clip;
            }
        }

        if(clip != null)
        {
            soundFXSource.PlayOneShot(clip);
        }
    }

    private void Update()
    {
        UpdateAudioSourceState();
    }

    private void UpdateAudioSourceState()
    {
        // made this as a separated function so that if we want
        // to fade the audio source in/out, we can call this on Update
        // and increment/decrement the audio source volume (instead of mutting)
        for (int i = 0; i < musicSources.Length; i++)
        {
            var source = musicSources[i].Source;
            var shouldBeMuted = musicSources[i].ShouldBeMute;

            if (shouldBeMuted && source.volume > 0)
            {
                if (source.volume <= 0)
                {
                    source.volume = 0;
                }
            }
            else if (!shouldBeMuted && source.volume < 1)
            {
                source.volume += musicFadeRate;
                if (source.volume >= 1)
                {
                    source.volume = 1;
                }
            }
        }
    }
}

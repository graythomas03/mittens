using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum SoundFX { 
        HitEnemy,
        TennisBallLaunch,
        SprinlerSpray,
        BombBlast,
        CatMove,
        CatSwipe,
        CatDrag,
        GameOver,
        ReadySetGo,
        SFXButton
    };

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
        public AudioClip[] Clips;
        public SoundFX FX;
    }

    [Header("Music")]
    [SerializeField]
    private AudioSource title;

    [SerializeField]
    private AudioSource credits;

    [SerializeField] 
    private MusicSources[] musicSources;

    [SerializeField]
    private float musicFadeRate;

    [SerializeField]
    [Range(0f,1f)]
    private float maxVolume;

    [SerializeField]
    private bool muteInsteadOfFade;

    [Header("SoundFX")]
    [SerializeField]
    private SoundFXClip[] soundFXList;



    public AudioSource soundFXSource;
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

    void Awake(){
        if (Instance != this)
        {
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }
        _instance = this;
        UpdateAudioSourceState();
    }

    public void ToggleTitle(bool isOn)
    {
        title.volume = isOn ? 1f : 0f;
    }

    public void ToggleCredit(bool isOn)
    {
        credits.volume = isOn ? 1f : 0f;
    }

    public void Toggle(bool isOn, params int[] indexes)
    {
        foreach(int i in indexes)
        {
            Debug.Log("music: " + i + " set to mute? " + !isOn);
            if(i >= 0 && i < musicSources.Length)
            {
                musicSources[i].ShouldBeMute = !isOn;
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
                if(entry.Clips != null && entry.Clips.Length > 0)
                {
                    int size = entry.Clips.Length;
                    clip = entry.Clips[Random.Range(0, size)];
                }
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

            if(muteInsteadOfFade)
            {
                if(source.mute != shouldBeMuted)
                {
                    source.mute = shouldBeMuted;
                }
            }
            else
            {
                if (shouldBeMuted && source.volume > 0)
                {
                    source.volume -= musicFadeRate * Time.deltaTime;
                    if (source.volume <= 0)
                    {
                        source.volume = 0;
                    }
                }
                else if (!shouldBeMuted && source.volume < maxVolume)
                {
                    source.volume += musicFadeRate * Time.deltaTime;
                    if (source.volume >= maxVolume)
                    {
                        source.volume = maxVolume;
                    }
                }
            }
        }
    }
}

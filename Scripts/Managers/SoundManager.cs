using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _bgmPlayer;
    [SerializeField] private AudioSource[] _sfxPlayers;
    private int currentChannel = 0;
    private int maxChannels = 15;
    private bool _isSFXMuted;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            InitSoundChannels();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitSoundChannels()
    {
        _sfxPlayers = new AudioSource[maxChannels];

        for (int i = 0; i < maxChannels; i++)
        {
            GameObject channelObj = new GameObject($"SFXChannel_{i}");
            channelObj.transform.SetParent(transform);
            _sfxPlayers[i] = channelObj.AddComponent<AudioSource>();

            // 사운드 기본 세팅
            _sfxPlayers[i].playOnAwake = false;
            _sfxPlayers[i].spatialBlend = 0f;
        }
    }

    public void SetSFXMuted(bool isSFXmuted)
    {
        if (isSFXmuted == _isSFXMuted) return;
        _isSFXMuted = isSFXmuted;
        if (isSFXmuted == true)
        {
            foreach (AudioSource sfxPlayer in _sfxPlayers)
            {
                sfxPlayer.Stop();
            }
        }
    }
    private void PlayBGM(AudioClip bgmClip, float volume = 0.3f)
    {
        if (bgmClip == null) return;
        
        if (_bgmPlayer.clip == bgmClip) return;

        _bgmPlayer.clip = bgmClip;
        _bgmPlayer.volume = volume;
        _bgmPlayer.Play();
    }
    public void PauseBGM()
    {
        _bgmPlayer.Pause();
    }
    public void PlayBGM(string clipName, float volume = 0.3f)
    {
        AudioClip audioClip = Managers.Resource.Load<AudioClip>(clipName);
        PlayBGM(audioClip, volume);
    }
    public void ResumeBGM()
    {
        _bgmPlayer.UnPause();
    }
    public void PlaySFX(string clipName, float volume = 0.3f, float pitch = 1, double duration = 0)
    {
        if(_isSFXMuted == true) return;
        AudioClip audioClip = Managers.Resource.Load<AudioClip>(clipName);
 
        _sfxPlayers[currentChannel].Stop(); 
        _sfxPlayers[currentChannel].pitch = pitch;
        _sfxPlayers[currentChannel].clip = audioClip;
        _sfxPlayers[currentChannel].Play();
        _sfxPlayers[currentChannel].volume = volume;

        if (duration > 0)
        {
            double stopTime = AudioSettings.dspTime + duration;
            _sfxPlayers[currentChannel].SetScheduledEndTime(stopTime);
        }
        currentChannel = (currentChannel + 1) % _sfxPlayers.Length;
    }
}
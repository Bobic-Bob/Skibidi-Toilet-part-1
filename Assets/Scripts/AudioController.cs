using System;
using UnityEngine;
using UnityEngine.Audio;
using YG;

public class AudioController : MonoBehaviour
{

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _sound;

    public const string SOUND_GROUP = "SoundGroup";
    public const string MUSIC_GROUP = "MusicGroup";

    private void Start()
    {
        LoadData();

        if (!_mixer)
        {
            throw new NullReferenceException(nameof(_mixer));
        }

        if (!_music)
        {
            throw new NullReferenceException(nameof(_music));
        }

        if (!_sound)
        {
            throw new NullReferenceException(nameof(_sound));
        }
    }

    private void LoadData()
    {
        if (!YandexGame.savesData.Sound)
        {
            _sound.mute = true;
            _sound.Pause();
            _mixer.SetFloat(SOUND_GROUP, -80f);
        }

        if (!YandexGame.savesData.Music)
        {
            _music.mute = true;
            _music.Pause();
            _mixer.SetFloat(MUSIC_GROUP, -80f);
        }
    }

    public void SwitchSound()
    {
        if (_sound.mute)
        {
            _sound.mute = false;
            _sound.UnPause();
            _mixer.SetFloat(SOUND_GROUP, 0f);
        }
        else
        {
            _sound.mute = true;
            _sound.Pause();
            _mixer.SetFloat(SOUND_GROUP, -80f);
        }

        YandexGame.savesData.Sound = !_sound.mute;
        YandexGame.savesData.Music = !_music.mute;
        YandexGame.SaveProgress();
    }

    public void SwitchMusic()
    {
        if (_music.mute)
        {
            _music.mute = false;
            _music.UnPause();
            _mixer.SetFloat(MUSIC_GROUP, 0f);
        }
        else
        {
            _music.mute = true;
            _music.Pause();
            _mixer.SetFloat(MUSIC_GROUP, -80f);
        }

        YandexGame.savesData.Sound = !_sound.mute;
        YandexGame.savesData.Music = !_music.mute;
        YandexGame.SaveProgress();
    }
}

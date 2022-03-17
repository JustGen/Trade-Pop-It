using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
    public static GameAudioSource S;

    [SerializeField] private AudioSource _audioSourcePopIt;
    [SerializeField] private AudioSource _audioSourceClicks;
    [SerializeField] private AudioSource _audioSourceBG;
    [SerializeField] private AudioClip[] _audioClipsPopIt;
    [SerializeField] private AudioClip[] _audioClipsSnapperz;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
            PlayBGMusic();
    }

    public void AudioPlayPopItOrSD()
    {
        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            int idAudioClip = Random.Range(0, _audioClipsPopIt.Length);
            _audioSourcePopIt.clip = _audioClipsPopIt[idAudioClip];
            _audioSourcePopIt.Play();
        }
    }

    public void AudioPlaySnapperz()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            int idAudioClip = Random.Range(0, _audioClipsSnapperz.Length);
            _audioSourcePopIt.clip = _audioClipsSnapperz[idAudioClip];
            _audioSourcePopIt.Play();

        }
    }

    public void Click()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            _audioSourceClicks.Stop();
            _audioSourceClicks.Play();
        }
    }

    public void PlayBGMusic()
    {
        StartCoroutine(CoroutinePlayBGMusic());
    }
    public void PauseBGMusic()
    {
        StartCoroutine(CoroutinePauseBGMusic());
    }

    private IEnumerator CoroutinePlayBGMusic()
    {
        yield return null;
        _audioSourceBG.Play();
    }

    private IEnumerator CoroutinePauseBGMusic()
    {
        yield return null;
        _audioSourceBG.Pause();
    }

    public bool CheckAudioBGByPause()
    {
        return _audioSourceBG.isPlaying;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager S;

    [SerializeField] private GameObject itemMusic;
    [SerializeField] private GameObject itemSound;
    [SerializeField] private GameObject itemVibro;
    [SerializeField] private AnimationClip _animationOn;
    [SerializeField] private AnimationClip _animationOff;
    [SerializeField] private Color _colorActive;
    [SerializeField] private Color _colorNoActive;
    [SerializeField] private Image _imageStatusRU;
    [SerializeField] private Image _imageStatusEN;


    private bool _firstRun;
    private int _statusMusic;
    private int _statusSound;
    private int _statusVibro;
    private int _statusLang;

    private Animation _animationOfMusic;
    private Animation _animationOfSound;
    private Animation _animationOfVibro;


    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        _firstRun = true;

        _animationOfMusic = itemMusic.GetComponent<Animation>();
        _animationOfSound = itemSound.GetComponent<Animation>();
        _animationOfVibro = itemVibro.GetComponent<Animation>();

        if (PlayerPrefs.HasKey("Music"))
        {
            _statusMusic = PlayerPrefs.GetInt("Music");
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        if (PlayerPrefs.HasKey("Sound"))
        {
            _statusSound = PlayerPrefs.GetInt("Sound");
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }

        if (PlayerPrefs.HasKey("Vibro"))
        {
            _statusVibro = PlayerPrefs.GetInt("Vibro");
        }
        else
        {
            PlayerPrefs.SetInt("Vibro", 1);
        }

        MusicControl();
        SoundControl();
        VibroControl();
        

        if (PlayerPrefs.HasKey("Lang"))
        {
            _statusLang = PlayerPrefs.GetInt("Lang");
            ChangeLang(_statusLang);
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                ChangeLang(1);
            }
            else
            {
                ChangeLang(2);
            }      
        }

        _firstRun = false;
    }

    public void MusicControl()
    {
        if (StatusMusic == 1)
        {
            if (!_firstRun)
            {
                GameAudioSource.S.PauseBGMusic();
                StatusMusic = 0;
                _animationOfMusic.clip = _animationOff;
            }
            else
            {
                _animationOfMusic.clip = _animationOn;
            }

        } 
        else if (StatusMusic == 0)
        {
            if (!_firstRun)
            {
                GameAudioSource.S.PlayBGMusic();
                StatusMusic = 1;
                _animationOfMusic.clip = _animationOn;
            }
            else
            {
                _animationOfMusic.clip = _animationOff;
            }
        }

        _animationOfMusic.Play();
    }

    public void SoundControl()
    {
        if (StatusSound == 1)
        {
            if (!_firstRun)
            {
                StatusSound = 0;
                _animationOfSound.clip = _animationOff;
            }
            else
            {
                _animationOfSound.clip = _animationOn;
            }
        }
        else if (StatusSound == 0)
        {
            if (!_firstRun)
            {
                StatusSound = 1;
                _animationOfSound.clip = _animationOn;
            }
            else
            {
                _animationOfSound.clip = _animationOff;
            }
        }
        
        _animationOfSound.Play();
    }

    public void VibroControl()
    {
        if (StatusVibro == 1)
        {
            if (!_firstRun)
            {
                StatusVibro = 0;
                _animationOfVibro.clip = _animationOff;
            }
            else
            {
                _animationOfVibro.clip = _animationOn;
            }

        }
        else if (StatusVibro == 0)
        {
            if (!_firstRun)
            {
                StatusVibro = 1;
                _animationOfVibro.clip = _animationOn;
            }
            else
            {
                _animationOfVibro.clip = _animationOff;
            }
        }

        _animationOfVibro.Play();
    }

    public void ChangeLang(int idLang)
    {
        if (idLang == 1)
        {
            _imageStatusRU.color = _colorActive;
            _imageStatusEN.color = _colorNoActive;
            PlayerPrefs.SetInt("Lang", idLang);
            LangManager.S.ChangeGlobalLang(idLang);
        }
        else if (idLang == 2)
        {
            _imageStatusEN.color = _colorActive;
            _imageStatusRU.color = _colorNoActive;
            PlayerPrefs.SetInt("Lang", idLang);
            LangManager.S.ChangeGlobalLang(idLang);
        }
        else
        {
            Debug.Log("Lang no found!");
        }
    }

    private int StatusMusic
    {
        get
        {
            _statusMusic = PlayerPrefs.GetInt("Music");
            return _statusMusic;
        }

        set
        {
            _statusMusic = value;
            PlayerPrefs.SetInt("Music", value);
        }
    }

    private int StatusSound
    {
        get
        {
            _statusSound = PlayerPrefs.GetInt("Sound");
            return _statusSound;
        }

        set
        {
            _statusSound = value;
            PlayerPrefs.SetInt("Sound", value);
        }
    }

    private int StatusVibro
    {
        get
        {
            _statusVibro = PlayerPrefs.GetInt("Vibro");
            return _statusVibro;
        }

        set
        {
            _statusVibro = value;
            PlayerPrefs.SetInt("Vibro", value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreUI : MonoBehaviour
{
    public static CoreUI S;

    [Header("MENU")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _btnOpenMenu;
    [SerializeField] private GameObject _subMenu;
    [SerializeField] private GameObject _subMenuChooseHero;
    [SerializeField] private GameObject _btnSkip;

    [Header("Menu Palels")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _shopPanel;

    [Header("Panels")]
    [SerializeField] private GameObject _startMenuGame;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _winPanelWithAds;
    [SerializeField] private Text _coinsLevel;
    [SerializeField] private Text _coinsAddLevel;

    [Header("Elements for Main Level")]
    [SerializeField] private GameObject _gameSceneModels;
    [SerializeField] private GameObject _room;
    [SerializeField] private GameObject _hand;
    [SerializeField] private GameObject _sceneUI;
    [SerializeField] private GameObject _scrollViewModels;

    [Header("Elements for Add Level")]
    [SerializeField] private GameObject _texturePattern;
    [SerializeField] private List<Sprite> _listOfSpritesForPattern;
    [SerializeField] private GameObject _slider;
    [SerializeField] private Text _txtCountClickers;
    [SerializeField] private Text _txtCurrClickers;
    [SerializeField] private GameObject _sliderRightPart;

    [Header("Positions Camera")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _posStart;
    [SerializeField] private Transform _pos1;
    [SerializeField] private Transform _pos2;

    private int _currValueSlider;
    private Slider _sliderComponent;

    public int coinsAfterLevelGlobal;

    private bool _blocking3DModels;

    private bool _startGameMunuStatus;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        //StartGame(true);
        StartMenuGame(true);

        _sliderComponent = _slider.GetComponent<Slider>();

        coinsAfterLevelGlobal = 0;

        Blocking3DModelsBool = false;
    }

    public void StartMenuGame(bool status)
    {
        if (!status)
        {
            if (HeroesManager.S.CheckBuyHeroBeforeStart())
            {
                _startMenuGame.SetActive(false);
                _gamePanel.SetActive(true);

                HeroesManager.S.LoadHeroForGame();
                CoreGame.S.LoadLevel();

                _startGameMunuStatus = false;
            }
        }
        else
        {
            _startMenuGame.SetActive(true);
            _gamePanel.SetActive(false);

            GameMode(0);

            _startGameMunuStatus = true;
        }
    }

    public void StartGame(bool status)
    {
        _gamePanel.SetActive(status);
        _losePanel.SetActive(!status);
        _winPanel.SetActive(!status);
    }

    public void LoseGame(bool status)
    {
        _gamePanel.SetActive(!status);
        _losePanel.SetActive(status);
        _scrollViewModels.SetActive(!status);

        Blocking3DModels(status);
    }

    public void WinGame(bool status)
    {
        _gamePanel.SetActive(!status);
        _scrollViewModels.SetActive(!status);

        Blocking3DModels(status);

        if (CoreGame.S.Level < 8)
        {
            _winPanel.SetActive(status);
        }
        else
        {
            StartCoroutine(FinishLevelPanelWithAds_Corotinue(status));
        }
    }

    public IEnumerator FinishLevelPanelWithAds_Corotinue(bool status)
    {
        _winPanelWithAds.SetActive(status);

        if (status)
        {
            _btnSkip.SetActive(false);

            yield return new WaitForSeconds(2f);

            _btnSkip.SetActive(true);
        }
        else
        {
            yield return null;
        }
    }

    public void FinishLevel()
    {
        if (CoreGame.S.LevelIndicator > 50)
        {
            WinGame(true);
            int coinsLevel = Random.Range(10, 15);
            _coinsLevel.text = coinsLevel.ToString();
            _coinsAddLevel.text = coinsLevel.ToString();
            //CoreGame.S.Coins += coinsLevel;
            coinsAfterLevelGlobal = coinsLevel;
            //GameMode(0);
        }
        else
        {
            LoseGame(true);
        }

        StartCoroutine(CoreGame.S.ReloadTimer(false));
    }

    public void FinishLevelAddLevel(int gameMode)
    {
        //GameMode(0);
        WinGame(true);
        SliderActivator(false, 0, true);

        int coinsLevel = Random.Range(20, 29);
        _coinsLevel.text = coinsLevel.ToString();
        _coinsAddLevel.text = coinsLevel.ToString();
        //CoreGame.S.Coins += coinsLevel;
        coinsAfterLevelGlobal = coinsLevel;

        switch (gameMode) //1 - PopIt, 2 - SD, 3 - Sz
        {
            case 1:
                CoreGame.S.LevelPopIt++;
                break;

            case 2:
                CoreGame.S.LevelSD++;
                break;

            case 3:
                CoreGame.S.LevelSnapperz++;
                break;
        }
    }

    public void Restart()
    {
        LoseGame(false);
        CoreGame.S.ClearScene(true);
    }

    public void NextLevel()
    {
        //StartMenuGame(true);
        WinGame(false);
        CoreGame.S.NextLevel();
    }

    public void NextLevelWithAds()
    {
        //StartMenuGame(true);
        WinGame(false);
    }

    public void ChooseHero()
    {
        CoreGame.S.BackToChooseHeroFromLevel();
        _subMenuChooseHero.SetActive(false);
        MenuActivator(false);
    }

    public void GameMode(int id) //1 - main scene, 2 - add level Pop It, 3 - Add Level Other, 0 - Start Menu
    {
        if (id == 0)
        {
            _camera.transform.position = _posStart.position;
            _camera.GetComponent<Camera>().fieldOfView = 100;
            _camera.transform.rotation = _posStart.rotation;
            _gameSceneModels.SetActive(true);
            _room.SetActive(true);
            _hand.SetActive(false);
            _sceneUI.SetActive(false);
            _scrollViewModels.SetActive(false);
            _texturePattern.SetActive(false);
            _slider.SetActive(false);
            HeroesManager.S.HeroActivatorForGameMode(true);
        }
        else if (id == 1)
        {
            _camera.transform.position = _pos1.position;
            _camera.transform.rotation = _pos1.rotation;
            _camera.GetComponent<Camera>().fieldOfView = 42;
            _gameSceneModels.SetActive(true);
            _room.SetActive(true);
            _hand.SetActive(true);
            _sceneUI.SetActive(true);
            _scrollViewModels.SetActive(true);
            _texturePattern.SetActive(false);
            _slider.SetActive(false);
            HeroesManager.S.HeroActivatorForGameMode(true);
        }
        else if (id == 2)
        {
            _camera.transform.position = _pos2.position;
            _camera.transform.rotation = _pos2.rotation;
            _camera.GetComponent<Camera>().fieldOfView = 42;
            _gameSceneModels.SetActive(false);
            _room.SetActive(false);
            _hand.SetActive(false);
            _sceneUI.SetActive(false);
            _scrollViewModels.SetActive(false);
            _texturePattern.SetActive(true);
            _slider.SetActive(false);
            HeroesManager.S.HeroActivatorForGameMode(false);
        }
        else if (id == 3)
        {
            _camera.transform.position = _pos2.position;
            _camera.transform.rotation = _pos2.rotation;
            _camera.GetComponent<Camera>().fieldOfView = 42;
            _gameSceneModels.SetActive(false);
            _room.SetActive(false);
            _hand.SetActive(false);
            _sceneUI.SetActive(false);
            _scrollViewModels.SetActive(false);
            _texturePattern.SetActive(false);
            _slider.SetActive(true);
            HeroesManager.S.HeroActivatorForGameMode(false);
        }
    }

    public void ChangeTexturePattern(int id)
    {
        _texturePattern.GetComponent<Image>().sprite = _listOfSpritesForPattern[id];
    }

    public void SliderActivator(bool status, int maxCount, bool reset)
    {
        _slider.SetActive(status);
        _sliderComponent.maxValue = maxCount;
        _txtCountClickers.text = maxCount.ToString();
        CurrValueSlider = 0;

        if (reset)
        {
            _sliderRightPart.SetActive(false);
        }
    }

    public void ProgressBarUpdate()
    {
        CurrValueSlider++;

        if (_sliderComponent.value == _sliderComponent.maxValue)
        {
            _sliderRightPart.SetActive(true);
        }
    }

    public void MenuActivator(bool status)
    {
        _menu.SetActive(status);
        Blocking3DModels(status);

        if (!_startMenuGame.activeInHierarchy)
        {
            if (status)
                StartCoroutine(CoreGame.S.ReloadTimer(false));
            else
            {
                CoreGame.S.GetTimerCorotinue();
                StartCoroutine(CoreGame.S.ReloadTimer(true));
            }
        }

        if (status)
            _btnOpenMenu.SetActive(false);
        else
            _btnOpenMenu.SetActive(true);

        if (_startGameMunuStatus && status)
            _startMenuGame.SetActive(false);
        else if (_startGameMunuStatus && !status)
            _startMenuGame.SetActive(true);
    }

    public void SettingsActivator(bool status)
    {
        _menu.SetActive(!status);
        _settingsPanel.SetActive(status);
    }

    public void ShopActivator(bool status)
    {
        _menu.SetActive(!status);
        _shopPanel.SetActive(status);
    }

    public void SubMenuActivator(bool status)
    {
        _subMenu.SetActive(status);
    }

    public void SubMenuChooseHeroActivator(bool status)
    {
        if (!_startGameMunuStatus)
        {
            _subMenuChooseHero.SetActive(status);
        }
        else
        {
            MenuActivator(false);
        }
            
    }

    public void AppQuit()
    {
        Application.Quit();
    }

    private void Blocking3DModels(bool status)
    {
        if (status)
            Blocking3DModelsBool = true;
        else
            Blocking3DModelsBool = false;
    }

    private int CurrValueSlider
    {
        get
        {
            return _currValueSlider;
        }

        set
        {
            _currValueSlider = value;
            _sliderComponent.value = _currValueSlider;
            _txtCurrClickers.text = _currValueSlider.ToString();
        }
    }

    public bool Blocking3DModelsBool
    {
        get
        {
            return _blocking3DModels;
        }

        set
        {
            _blocking3DModels = value;
            CoreGame.S.Block3DButtons = _blocking3DModels;
        }
    }

}

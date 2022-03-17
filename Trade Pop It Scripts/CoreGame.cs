using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreGame : MonoBehaviour
{
    public static CoreGame S;

    [SerializeField] private int _startlevel_ADMIN;
    [SerializeField] private float _timeReactionBot;
    [SerializeField] private float _timeReactionHand;
    [SerializeField] private float _timeWaitBot;
    [SerializeField] private float _timeBlocking;
    [SerializeField] private List<GameObject> _listOfItems;
    [SerializeField] private List<GameObject> _listOfSpawnPoints;
    [SerializeField] private Slider _sliderLevelIndicator;
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Text _coinsWallet;
    public float scaleFactorForBubble;

    [Header("ModelsAddLevel")]
    [SerializeField] private List<GameObject> _listOfModelsForAddLevelsPopIt;
    [SerializeField] private List<GameObject> _listOfModelsForAddLevelsSD;
    [SerializeField] private List<GameObject> _listOfModelsForAddLevelsSnapperz;
    [SerializeField] private int[] _listOfLevelPopIt;
    [SerializeField] private int[] _listOfLevelSD;
    [SerializeField] private int[] _listOfLevelSnapperz;

    [Header("Logic Persentes")]
    [Header("Red Zone")]
    [SerializeField] private float _perRedZonePutItem;
    [SerializeField] private float _perRedZoneOk;
    [Header("Yellow Zone")]
    [SerializeField] private float _perYellowZoneDownOk;
    [SerializeField] private float _perYellowZoneDownAdd;
    [SerializeField] private float _perYellowZoneDownPutItem;
    [SerializeField] private float _perYellowZoneTopOk;
    [SerializeField] private float _perYellowZoneTopAdd;
    [SerializeField] private float _perYellowZoneTopPutItem;
    [Header("Yellow Zone")]
    [SerializeField] private float _perGreenZoneDownOk;
    [SerializeField] private float _perGreenZoneDownAdd;
    [SerializeField] private float _perGreenZoneDownCancel;
    [SerializeField] private float _perGreenZoneDownPutItem;
    [SerializeField] private float _perGreenZoneTopOk;
    [SerializeField] private float _perGreenZoneTopAdd;
    [SerializeField] private float _perGreenZoneTopCancel;

    [Header("Status OK")]
    public bool playerOk;
    public bool aiOk;

    private int _balancePlayer;
    private int _balanceBot;
    private int _levelIndicator;

    private IEnumerator _timerCoroutine;

    private List<GameObject> _listOfModelInScene;

    private int _countWaitStep;
    private int _idLastItemPutbyPlayer;

    private GameObject modelAddLevel;

    private int _level;
    private int _levelPopIt;
    private int _levelSD;
    private int _levelSnapperz;

    private int _choosenHero;

    private int _levelP1;
    private int _levelPopItP1;
    private int _levelSDP1;
    private int _levelSnapperzP1;

    private int _levelP2;
    private int _levelPopItP2;
    private int _levelSDP2;
    private int _levelSnapperzP2;

    private int _levelP3;
    private int _levelPopItP3;
    private int _levelSDP3;
    private int _levelSnapperzP3;

    private bool _foundAddLevel;

    private bool _block3DButtons;

    private int _coins;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        LoadSavesOther();

        LoadSavesP1();
        LoadSavesP2();
        LoadSavesP3();

        //Level = _startlevel_ADMIN;
        //LevelPopIt = 1;
        //Coins = 8000;

        _listOfModelInScene = new List<GameObject>();

        UpdateLevelTxt();
    }

    private void LoadSavesOther()
    {
        if (PlayerPrefs.HasKey("Coins"))
            Coins = PlayerPrefs.GetInt("Coins");
        else
            Coins = 0;
    }

    public void LoadSavesP1()
    {
        if (PlayerPrefs.HasKey("LevelP1"))
            LevelP1 = PlayerPrefs.GetInt("LevelP1");
        else
            LevelP1 = 1;

        if (PlayerPrefs.HasKey("LevelPopItP1"))
            LevelPopItP1 = PlayerPrefs.GetInt("LevelPopItP1");
        else
            LevelPopItP1 = 0;

        if (PlayerPrefs.HasKey("LevelSDP1"))
            LevelSDP1 = PlayerPrefs.GetInt("LevelSDP1");
        else
            LevelSDP1 = 0;

        if (PlayerPrefs.HasKey("LevelSnapperzP1"))
            LevelSnapperzP1 = PlayerPrefs.GetInt("LevelSnapperzP1");
        else
            LevelSnapperzP1 = 0;
    }

    public void LoadSavesP2()
    {
        if (PlayerPrefs.HasKey("LevelP2"))
            LevelP2 = PlayerPrefs.GetInt("LevelP2");
        else
            LevelP2 = 1;

        if (PlayerPrefs.HasKey("LevelPopItP2"))
            LevelPopItP2 = PlayerPrefs.GetInt("LevelPopItP2");
        else
            LevelPopItP2 = 0;

        if (PlayerPrefs.HasKey("LevelSDP2"))
            LevelSDP2 = PlayerPrefs.GetInt("LevelSDP2");
        else
            LevelSDP2 = 0;

        if (PlayerPrefs.HasKey("LevelSnapperzP2"))
            LevelSnapperzP2 = PlayerPrefs.GetInt("LevelSnapperzP2");
        else
            LevelSnapperzP2 = 0;
    }

    public void LoadSavesP3()
    {
        if (PlayerPrefs.HasKey("LevelP3"))
            LevelP3 = PlayerPrefs.GetInt("LevelP3");
        else
            LevelP3 = 1;

        if (PlayerPrefs.HasKey("LevelPopItP3"))
            LevelPopItP3 = PlayerPrefs.GetInt("LevelPopItP3");
        else
            LevelPopItP3 = 0;

        if (PlayerPrefs.HasKey("LevelSDP3"))
            LevelSDP3 = PlayerPrefs.GetInt("LevelSDP3");
        else
            LevelSDP3 = 0;

        if (PlayerPrefs.HasKey("LevelSnapperzP3"))
            LevelSnapperzP3 = PlayerPrefs.GetInt("LevelSnapperzP3");
        else
            LevelSnapperzP3 = 0;
    }

    public void PutItem(int idItem)
    {
        if (!Block3DButtons)
        {
            BlockButtons();
            idItem--;
            _idLastItemPutbyPlayer = idItem;
            HandMoving.S.MoveHandPlayer(0);
            StartCoroutine(PutItemProcess());
        }
    }

    private IEnumerator PutItemProcess()
    {
        //StopCorotinueTimer();
        StartCoroutine(ReloadTimer(true));

        yield return new WaitForSeconds(_timeReactionHand);

        InstantiateModel(false, _idLastItemPutbyPlayer);
        CalculateIndicator();

        yield return new WaitForSeconds(_timeReactionBot);

        BrainPowerAI();
    }

    public void BlockButtons()
    {
        StartCoroutine(Blocking());
    }

    private IEnumerator Blocking()
    {
        Block3DButtons = true;

        yield return new WaitForSeconds(_timeBlocking);

        Block3DButtons = false;
    }

    //####################################### FUCKING LOGIC BY BOT #######################################
    private void BrainPowerAI()
    {
        //if (block3DButtons)
        //    return;

        if (LevelIndicator == 0) //First Step
        {
            StartCoroutine(PutItemAI());
        }
        else if (LevelIndicator < 33) //Red Zone
        {
            if (Random.Range(0f, 1.0f) <= _perRedZoneOk) 
            {
                HandMoving.S.MoveHandBot(1); //AI click OK
            }
            else
            {
                StartCoroutine(PutItemAI()); //AI put Item
            }
        } 
        else if (LevelIndicator >= 33 && LevelIndicator < 49) // Down Yellow
        {
            if (Random.Range(0f, 1.0f) <= _perYellowZoneDownOk)
            {
                HandMoving.S.MoveHandBot(1); //AI click OK
            }
            else if (Random.Range(0f, 1.0f) <= _perYellowZoneDownAdd)
            {
                HandMoving.S.MoveHandBot(2); //AI click Add
            }
            else
            {
                StartCoroutine(PutItemAI()); //AI put Item
            }
        } 
        else if (LevelIndicator >= 49 && LevelIndicator < 66) // Top Yellow
        {
            if (Random.Range(0f, 1.0f) <= _perYellowZoneTopOk)
            {
                HandMoving.S.MoveHandBot(1); //AI click OK 
            }
            else if (Random.Range(0f, 1.0f) <= _perYellowZoneTopAdd)
            {
                HandMoving.S.MoveHandBot(2); //AI click Add
            }
            else
            {
                StartCoroutine(PutItemAI()); //AI put Item
            }
        } 
        else if (LevelIndicator >= 66 && LevelIndicator < 83) // Down Green
        {
            if (Random.Range(0f, 1.0f) <= _perGreenZoneDownOk)
            {
                HandMoving.S.MoveHandBot(1); //AI click OK
            }
            else if (Random.Range(0f, 1.0f) <= _perGreenZoneDownAdd)
            {
                HandMoving.S.MoveHandBot(2); //AI click Add
            }
            else if (Random.Range(0f, 1.0f) <= _perGreenZoneDownCancel)
            {
                HandMoving.S.MoveHandBot(3); //AI click Cancel
                CoreUI.S.LoseGame(true);
            }
            else
            {
                StartCoroutine(PutItemAI()); //AI put Item
            }
        }
        else if (LevelIndicator >= 83 && LevelIndicator < 100) // Top Green
        {
            if (Random.Range(0f, 1.0f) <= _perGreenZoneTopOk)
            {
                HandMoving.S.MoveHandBot(1); //AI click OK
            }
            else if (Random.Range(0f, 1.0f) <= _perGreenZoneTopAdd)
            {
                HandMoving.S.MoveHandBot(2); //AI click Add
            }
            else if (Random.Range(0f, 1.0f) <= _perGreenZoneTopCancel)
            {
                HandMoving.S.MoveHandBot(3); //AI click Cancel
                CoreUI.S.LoseGame(true);
            }
        }
    }
    //======================================= FUCKING LOGIC BY BOT =======================================

    private IEnumerator PutItemAI()
    {
        HandMoving.S.MoveHandBot(0);

        yield return new WaitForSeconds(_timeReactionHand);

        InstantiateModel(true, 0);
        CalculateIndicator();
    }

    private void CalculateIndicator()
    {
        LevelIndicator = 100 - Mathf.RoundToInt((BalancePlayer * 100) / (BalancePlayer + BalanceBot));
    }

    private void InstantiateModel(bool isAI, int id)
    {
        int spawnPointId = Random.Range(0, _listOfSpawnPoints.Count);
        int randomItemId = 0;

        if (isAI)
        {
            randomItemId = Random.Range(0, _listOfItems.Count);
            GameObject newItemBot = Instantiate(_listOfItems[randomItemId], _listOfSpawnPoints[spawnPointId].transform);
            BalanceBot += newItemBot.GetComponent<ItemTrade>().InsidePrice;
            _listOfModelInScene.Add(newItemBot);
        }
        else
        {
            GameObject newItemPlayer = Instantiate(_listOfItems[id], _listOfSpawnPoints[spawnPointId].transform);
            BalancePlayer += newItemPlayer.GetComponent<ItemTrade>().InsidePrice;
            _listOfModelInScene.Add(newItemPlayer);
        }
    }

    public void DoneClick()
    {
        if (!Block3DButtons)
            StartCoroutine(DoneClickProcess());
    }

    private IEnumerator DoneClickProcess()
    {
        BlockButtons();
        StartCoroutine(ReloadTimer(true));
        HandMoving.S.MoveHandPlayer(1);

        yield return new WaitForSeconds(_timeReactionBot);

        if (!aiOk)
            BrainPowerAI();
    }

    public void AddClick()
    {
        if (!Block3DButtons)
            StartCoroutine(AddClickProcess());
    }

    private IEnumerator AddClickProcess()
    {
        BlockButtons();
        StartCoroutine(ReloadTimer(true));
        HandMoving.S.MoveHandPlayer(2);

        yield return new WaitForSeconds(_timeReactionBot);
        
        BrainPowerAI();
    }

    public void CancelClick()
    {
        if (!Block3DButtons)
        {
            BlockButtons();
            StartCoroutine(ReloadTimer(false));
            HandMoving.S.MoveHandPlayer(3);
            CoreUI.S.LoseGame(true);
        }     
    }

    private IEnumerator TimerWaitStep()
    {
        _countWaitStep = 0;
        //Debug.Log("Start " + _countWaitStep);
        while (_countWaitStep != 5)
        {
            yield return new WaitForSeconds(_timeWaitBot);
            HandMoving.S.MoveHandBot(2);
            _countWaitStep++;
            //Debug.Log("Work " + _countWaitStep);
            if (_countWaitStep == 5)
            {
                yield return new WaitForSeconds(_timeWaitBot);
                HandMoving.S.MoveHandBot(3);
                CoreUI.S.LoseGame(true);
            }
        }        
    }

    public IEnumerator ReloadTimer(bool start)
    {
        StopCoroutine(_timerCoroutine);

        yield return null;

        _timerCoroutine = null;

        yield return null;

        if (start)
        {
            GetTimerCorotinue();
            StartCoroutine(_timerCoroutine);
        } 
    }

    public void GetTimerCorotinue()
    {
        _timerCoroutine = TimerWaitStep();
    }


    public void ClearScene(bool restart)
    {
        StopAllCoroutines();

        foreach(GameObject temp in _listOfModelInScene)
        {
            Destroy(temp);
        }

        _listOfModelInScene.Clear();

        BalanceBot = 5;
        BalancePlayer = 5;
        _countWaitStep = 0;
        CalculateIndicator();
        playerOk = false;
        aiOk = false;
        Block3DButtons = true;

        if (modelAddLevel != null)
            Destroy(modelAddLevel);

        if (restart)
        {
            Block3DButtons = false;
            _timerCoroutine = TimerWaitStep();
            StartCoroutine(ReloadTimer(true));
        }
    }

    public void NextLevel()
    {
        ClearScene(false);
        Coins += CoreUI.S.coinsAfterLevelGlobal;

        if (Level + 1 <= 100)
        {
            if (Level >= 8 && Level % 2 == 0)
            {
                StartCoroutine(ShowAds());
            }

            Level++;
            //CoreUI.S.StartMenuGame(true);
            LoadLevel();
        }
    }

    public void BackToChooseHeroFromLevel()
    {
        ClearScene(false);
        CoreUI.S.StartMenuGame(true);
    }

    public void NextLevelAfterReward()
    {
        StartCoroutine(CoroninueAfterReward());
    }

    private IEnumerator CoroninueAfterReward()
    {
        yield return null;

        ClearScene(false);

        Coins += CoreUI.S.coinsAfterLevelGlobal * 2;

        if (Level + 1 <= 100)
        {
            Level++;
            //CoreUI.S.StartMenuGame(true);
            LoadLevel(); 
        }    
    }

    private IEnumerator ShowAds()
    {
        yield return null;
        GoogleADMob.S.ShowInterstitialVideo(1);
    }

    public void LoadLevel()
    {
        _foundAddLevel = false;

        for (int i = 0; i < _listOfLevelPopIt.Length; i++)
        {
            if (_listOfLevelPopIt[i] == Level)
            {
                CoreUI.S.GameMode(2);
                modelAddLevel = Instantiate(_listOfModelsForAddLevelsPopIt[LevelPopIt]);
                CoreUI.S.ChangeTexturePattern(LevelPopIt);
                //LevelPopIt++;
                _foundAddLevel = true;

                GameAudioSource.S.PauseBGMusic();
            }
        }

        if (!_foundAddLevel)
        {
            for (int i = 0; i < _listOfLevelSD.Length; i++)
            {
                if (_listOfLevelSD[i] == Level)
                {
                    CoreUI.S.GameMode(3);
                    modelAddLevel = Instantiate(_listOfModelsForAddLevelsSD[LevelSD]);
                    //LevelSD++;
                    _foundAddLevel = true;

                    GameAudioSource.S.PauseBGMusic();
                }
            }
        }

        if (!_foundAddLevel)
        {
            for (int i = 0; i < _listOfLevelSnapperz.Length; i++)
            {
                if (_listOfLevelSnapperz[i] == Level)
                {
                    CoreUI.S.GameMode(3);
                    modelAddLevel = Instantiate(_listOfModelsForAddLevelsSnapperz[LevelSnapperz]);
                    //LevelSnapperz++;
                    _foundAddLevel = true;

                    GameAudioSource.S.PauseBGMusic();
                }
            }
        }

        if (!_foundAddLevel)
        {
            BalanceBot = 5;
            BalancePlayer = 5;
            _countWaitStep = 0;
            CalculateIndicator();
            playerOk = false;
            aiOk = false;
            Block3DButtons = false;

            _timerCoroutine = TimerWaitStep();
            StartCoroutine(ReloadTimer(true));

            CoreUI.S.GameMode(1);

            if (GameAudioSource.S.CheckAudioBGByPause() == false)
            {
                GameAudioSource.S.PlayBGMusic();
            }
        }
    }

    public void LoadLevelFromChooseHero(int id)
    {
        _txtLevel.text = "Level " + PlayerPrefs.GetInt("LevelP" + id);
    }

    private int BalancePlayer
    {
        get
        {
            return _balancePlayer;
        }

        set
        {
            _balancePlayer = value;
        }
    }

    private int BalanceBot
    {
        get
        {
            return _balanceBot;
        }

        set
        {
            _balanceBot = value;
        }
    }

    public int LevelIndicator
    {
        get
        {
            return _levelIndicator;
        }

        set
        {
            _levelIndicator = value;
            _sliderLevelIndicator.value = _levelIndicator;
        }
    }

    public int Coins
    {
        get
        {
            return _coins;
        }

        set
        {
            _coins = value;
            _coinsWallet.text = _coins.ToString();
            PlayerPrefs.SetInt("Coins", _coins);
        }
    }

    public int Level
    {
        get
        {
            switch (ChoosenHero)
            {
                case 0:
                    _level = PlayerPrefs.GetInt("LevelP1");
                    break;

                case 1:
                    _level = PlayerPrefs.GetInt("LevelP2");
                    break;

                case 2:
                    _level = PlayerPrefs.GetInt("LevelP3");
                    break;
            }

            return _level;
        }

        set
        {
            _level = value;

            switch (ChoosenHero)
            {
                case 0:
                    PlayerPrefs.SetInt("LevelP1", _level);
                    break;

                case 1:
                    PlayerPrefs.SetInt("LevelP2", _level);
                    break;

                case 2:
                    PlayerPrefs.SetInt("LevelP3", _level);
                    break;
            }

            _txtLevel.text = "Level " + _level.ToString();
        }
    }

    private void UpdateLevelTxt()
    {
        _txtLevel.text = "Level " + Level.ToString();
    }

    public int LevelPopIt
    {
        get
        {
            switch (ChoosenHero)
            {
                case 0:
                    _levelPopIt = PlayerPrefs.GetInt("LevelPopItP1");
                    break;

                case 1:
                    _levelPopIt = PlayerPrefs.GetInt("LevelPopItP2");
                    break;

                case 2:
                    _levelPopIt = PlayerPrefs.GetInt("LevelPopItP3");
                    break;
            }

            return _levelPopIt;
        }

        set
        {
            _levelPopIt = value;

            switch (ChoosenHero)
            {
                case 0:
                    PlayerPrefs.SetInt("LevelPopItP1", _levelPopIt);
                    break;

                case 1:
                    PlayerPrefs.SetInt("LevelPopItP2", _levelPopIt);
                    break;

                case 2:
                    PlayerPrefs.SetInt("LevelPopItP3", _levelPopIt);
                    break;
            }
        }
    }

    public int LevelSD
    {
        get
        {
            switch (ChoosenHero)
            {
                case 0:
                    _levelSD = PlayerPrefs.GetInt("LevelSDP1");
                    break;

                case 1:
                    _levelSD = PlayerPrefs.GetInt("LevelSDP2");
                    break;

                case 2:
                    _levelSD = PlayerPrefs.GetInt("LevelSDP3");
                    break;
            }

            return _levelSD;
        }

        set
        {
            _levelSD = value;

            switch (ChoosenHero)
            {
                case 0:
                    PlayerPrefs.SetInt("LevelSDP1", _levelSD);
                    break;

                case 1:
                    PlayerPrefs.SetInt("LevelSDP2", _levelSD);
                    break;

                case 2:
                    PlayerPrefs.SetInt("LevelSDP3", _levelSD);
                    break;
            }
        }
    }

    public int LevelSnapperz
    {
        get
        {
            switch (ChoosenHero)
            {
                case 0:
                    _levelSnapperz = PlayerPrefs.GetInt("LevelSnapperzP1");
                    break;

                case 1:
                    _levelSnapperz = PlayerPrefs.GetInt("LevelSnapperzP2");
                    break;

                case 2:
                    _levelSnapperz = PlayerPrefs.GetInt("LevelSnapperzP3");
                    break;
            }

            return _levelSnapperz;
        }

        set
        {
            _levelSnapperz = value;

            switch (ChoosenHero)
            {
                case 0:
                    PlayerPrefs.SetInt("LevelSnapperzP1", _levelSnapperz);
                    break;

                case 1:
                    PlayerPrefs.SetInt("LevelSnapperzP2", _levelSnapperz);
                    break;

                case 2:
                    PlayerPrefs.SetInt("LevelSnapperzP3", _levelSnapperz);
                    break;
            }
        }
    }

    public int ChoosenHero
    {
        get
        {
            return _choosenHero;
        }

        set
        {
            _choosenHero = value;
        }
    }

    public int LevelP1
    {
        get
        {
            return _levelP1;
        }

        set
        {
            _levelP1 = value;
            _txtLevel.text = "Level " + _levelP1.ToString();
            PlayerPrefs.SetInt("LevelP1", _levelP1);
        }
    }

    public int LevelPopItP1
    {
        get
        {
            return _levelPopItP1;
        }

        set
        {
            _levelPopItP1 = value;
            PlayerPrefs.SetInt("LevelPopItP1", _levelPopItP1);
        }
    }

    public int LevelSDP1
    {
        get
        {
            return _levelSDP1;
        }

        set
        {
            _levelSDP1 = value;
            PlayerPrefs.SetInt("LevelSDP1", _levelSDP1);
        }
    }

    public int LevelSnapperzP1
    {
        get
        {
            return _levelSnapperzP1;
        }

        set
        {
            _levelSnapperzP1 = value;
            PlayerPrefs.SetInt("LevelSnapperzP1", _levelSnapperzP1);
        }
    }

    public int LevelP2
    {
        get
        {
            return _levelP2;
        }

        set
        {
            _levelP2 = value;
            _txtLevel.text = "Level " + _levelP2.ToString();
            PlayerPrefs.SetInt("LevelP2", _levelP2);
        }
    }

    public int LevelPopItP2
    {
        get
        {
            return _levelPopItP2;
        }

        set
        {
            _levelPopItP2 = value;
            PlayerPrefs.SetInt("LevelPopItP2", _levelPopItP2);
        }
    }

    public int LevelSDP2
    {
        get
        {
            return _levelSDP2;
        }

        set
        {
            _levelSDP2 = value;
            PlayerPrefs.SetInt("LevelSDP2", _levelSDP2);
        }
    }

    public int LevelSnapperzP2
    {
        get
        {
            return _levelSnapperzP2;
        }

        set
        {
            _levelSnapperzP2 = value;
            PlayerPrefs.SetInt("LevelSnapperzP2", _levelSnapperzP2);
        }
    }

    public int LevelP3
    {
        get
        {
            return _levelP3;
        }

        set
        {
            _levelP3 = value;
            _txtLevel.text = "Level " + _levelP3.ToString();
            PlayerPrefs.SetInt("LevelP3", _levelP3);
        }
    }

    public int LevelPopItP3
    {
        get
        {
            return _levelPopItP3;
        }

        set
        {
            _levelPopItP3 = value;
            PlayerPrefs.SetInt("LevelPopItP3", _levelPopItP3);
        }
    }

    public int LevelSDP3
    {
        get
        {
            return _levelSDP3;
        }

        set
        {
            _levelSDP3 = value;
            PlayerPrefs.SetInt("LevelSDP3", _levelSDP3);
        }
    }

    public int LevelSnapperzP3
    {
        get
        {
            return _levelSnapperzP3;
        }

        set
        {
            _levelSnapperzP3 = value;
            PlayerPrefs.SetInt("LevelSnapperzP3", _levelSnapperzP3);
        }
    }

    public bool Block3DButtons
    {
        get
        {
            return _block3DButtons;
        }

        set 
        {
            _block3DButtons = value;
        }
    }
}

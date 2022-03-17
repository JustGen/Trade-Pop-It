using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroesManager : MonoBehaviour
{
    public static HeroesManager S;

    [SerializeField] private List<GameObject> _listOfHeroes;
    [SerializeField] private GameObject _btnBuyHero;
    [SerializeField] private Text _txtBuyHero;
    [SerializeField] private int _priceHero1;
    [SerializeField] private int _priceHero2;
    [SerializeField] private MeshRenderer _hand1;
    [SerializeField] private MeshRenderer _hand2;
    [SerializeField] private List<Material> _listOfMaterialsForHeroHands;

    private int _idCurrHero;
    private int _idChooseHero;
    private GameObject _currHero;

    private int _buyHero0;
    private int _buyHero1;
    private int _buyHero2;

    private void Awake()
    {
        S = this;
        LoadSaves();
    }

    private void LoadSaves()
    {
        if (PlayerPrefs.HasKey("IdChooseHero"))
            IdChooseHero = PlayerPrefs.GetInt("IdChooseHero");
        else
            IdChooseHero = 0;

        if (PlayerPrefs.HasKey("BuyHero1"))
            BuyHero1 = PlayerPrefs.GetInt("BuyHero1");
        else
            BuyHero1 = 0;

        if (PlayerPrefs.HasKey("BuyHero2"))
            BuyHero2 = PlayerPrefs.GetInt("BuyHero2");
        else
            BuyHero2 = 0;

        IdHero = IdChooseHero;
    }

    public void ChangeHero()
    {
        if (IdHero != 2)
            IdHero++;
        else
            IdHero = 0;
    }

    public int IdHero
    {
        get
        {
            return _idCurrHero;
        }

        set
        {
            _idCurrHero = value;

            if (_currHero == null)
            {
                SpawnHero();
            } 
            else
            {
                Destroy(_currHero);
                SpawnHero();
            }

            CoreGame.S.LoadLevelFromChooseHero(_idCurrHero + 1);
        }
    }

    private void SpawnHero()
    {
        _currHero = Instantiate(_listOfHeroes[IdHero]);
        _currHero.transform.SetParent(this.transform);

        _hand1.material = _listOfMaterialsForHeroHands[IdHero];
        _hand2.material = _listOfMaterialsForHeroHands[IdHero];

        if (IdHero == 1)
        {
            if (_buyHero1 == 1)
            {
                _btnBuyHero.SetActive(false);
            }
            else
            {
                _btnBuyHero.SetActive(true);

                if (PlayerPrefs.GetInt("Lang") == 1)
                    _txtBuyHero.text = "Купить (" + _priceHero1 + ")";
                else
                    _txtBuyHero.text = "Buy (" + _priceHero1 + ")";
            } 
        }
        else if (IdHero == 2)
        {
            if (_buyHero2 == 1)
            {
                _btnBuyHero.SetActive(false);
            }
            else
            {
                _btnBuyHero.SetActive(true);

                if (PlayerPrefs.GetInt("Lang") == 1)
                    _txtBuyHero.text = "Купить (" + _priceHero2 + ")";
                else
                    _txtBuyHero.text = "Buy (" + _priceHero2 + ")";
            }
        }
        else
        {
            _btnBuyHero.SetActive(false);
        }
    }

    public void HeroActivatorForGameMode(bool status)
    {
        _currHero.SetActive(status);
    }

    public bool CheckBuyHeroBeforeStart()
    {
        bool buy;

        if (IdHero == 1)
        {
            if (PlayerPrefs.GetInt("BuyHero1") == 1)
                buy = true;
            else
                buy = false;
        }
        else if (IdHero == 2)
        {
            if (PlayerPrefs.GetInt("BuyHero2") == 1)
                buy = true;
            else
                buy = false;
        }
        else
        {
            buy = true;
        }

        return buy;
    }

    public void Buy()
    {
        BuyHero(IdHero);
    }

    private void BuyHero(int id)
    {
        if (id == 1 && CoreGame.S.Coins >= _priceHero1)
        {
            BuyHero1 = 1;
            CoreGame.S.Coins -= _priceHero1;
            _btnBuyHero.SetActive(false);
        }
        else if(id == 2 && CoreGame.S.Coins >= _priceHero2)
        {
            BuyHero2 = 1;
            CoreGame.S.Coins -= _priceHero2;
            _btnBuyHero.SetActive(false);
        }    
    }

    public void LoadHeroForGame()
    {
        IdChooseHero = IdHero;
    }

    public int IdChooseHero
    {
        get
        {
            _idChooseHero = PlayerPrefs.GetInt("IdChooseHero");
            return _idChooseHero;
        }

        set
        {
            _idChooseHero = value;
            PlayerPrefs.SetInt("IdChooseHero", _idChooseHero);
            //_btnBuyHero.SetActive(false);
            CoreGame.S.ChoosenHero = value;
        }
    }

    private int BuyHero1
    {
        get
        {
            return _buyHero1;
        }

        set
        {
            _buyHero1 = value;
            PlayerPrefs.SetInt("BuyHero1", _buyHero1);
        }
    }

    private int BuyHero2
    {
        get
        {
            return _buyHero2;
        }

        set
        {
            _buyHero2 = value;
            PlayerPrefs.SetInt("BuyHero2", _buyHero2);
        }
    }
}

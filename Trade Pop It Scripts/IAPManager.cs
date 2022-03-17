using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour, IStoreListener //для получения сообщений из Unity Purchasing
{
    [SerializeField] private GameObject _itemShopNoAds;

    IStoreController m_StoreController;

    private string noads = "com.trading.noads";
    private string money500 = "com.trading.money500";
    private string money1500 = "com.trading.money1500";
    private string money3500 = "com.trading.money3500";
    private string money6000 = "com.trading.money6000";

    void Start()
    {
        InitializePurchasing();
        RestoreVariable();

        //if (PlayerPrefs.HasKey("firstStart") == false)
        //{
        //    PlayerPrefs.SetInt("firstStart", 1);
        //    RestoreMyProduct();
        //}
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(noads, ProductType.NonConsumable);
        builder.AddProduct(money500, ProductType.Consumable);
        builder.AddProduct(money1500, ProductType.Consumable);
        builder.AddProduct(money3500, ProductType.Consumable);
        builder.AddProduct(money6000, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    void RestoreVariable()
    {
        if (PlayerPrefs.HasKey("ads"))
        {
            _itemShopNoAds.SetActive(false);
        }
    }

    public void BuyProduct(string productName)
    {
        m_StoreController.InitiatePurchase(productName);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;

        if (product.definition.id == noads)
        {
            Product_NoAds();
        }

        if (product.definition.id == money500)
        {
            GetMoney(500);
        }

        if (product.definition.id == money1500)
        {
            GetMoney(1500);
        }

        if (product.definition.id == money3500)
        {
            GetMoney(3500);
        }

        if (product.definition.id == money6000)
        {
            GetMoney(6000);
        }

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        return PurchaseProcessingResult.Complete;
    }

    private void Product_NoAds()
    {
        PlayerPrefs.SetInt("ads", 1);
        _itemShopNoAds.SetActive(false);
    }

    private void GetMoney(int count)
    {
        CoreGame.S.Coins += count;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }


    //public void RestoreMyProduct()
    //{
    //    if (CodelessIAPStoreListener.Instance.StoreController.products.WithID(noads).hasReceipt)
    //    {
    //        Product_NoAds();
    //    }

    //    if (CodelessIAPStoreListener.Instance.StoreController.products.WithID(vip).hasReceipt)
    //    {
    //        Product_VIP();
    //    }
    //}
}

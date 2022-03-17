using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoogleADMob : MonoBehaviour
{
    public static GoogleADMob S;

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialSkip;
    private InterstitialAd interstitialLose;
    //private BannerView bannerView;

    private string adUnitId_rewardedAd;
    private string adUnitId_interstitial_skip;
    private string adUnitId_interstitial_lose;
    //private string adUnitId_bannerView;

    private int idInterstitial;

    private void Awake()
    {
        S = this;
    }

    public void Start()
    {
        idInterstitial = 0;
        adUnitId_rewardedAd = "ca-app-pub-8820301686154131/3495014610";
        adUnitId_interstitial_skip = "ca-app-pub-8820301686154131/1326864899";
        adUnitId_interstitial_lose = "ca-app-pub-8820301686154131/9518617045";
        //adUnitId_bannerView = "ca-app-pub-8820301686154131/8853477068";

        //adUnitId_rewardedAd = "ca-app-pub-3940256099942544/5224354917"; //test
        //adUnitId_interstitial_skip = "ca-app-pub-3940256099942544/1033173712"; //test
        //adUnitId_interstitial_lose = "ca-app-pub-3940256099942544/1033173712"; //test
        ////adUnitId_bannerView = "ca-app-pub-3940256099942544/6300978111"; //test

        MobileAds.Initialize(initStatus => { });

        RequestRewardedAd();
        RequestInterstitialSkip();
        RequestInterstitialLose();
        //RequestBanner();
    }

    private void RequestInterstitialSkip()
    {
        this.interstitialSkip = new InterstitialAd(adUnitId_interstitial_skip);

        this.interstitialSkip.OnAdLoaded += HandleOnAdLoaded;
        this.interstitialSkip.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.interstitialSkip.OnAdOpening += HandleOnAdOpened;
        this.interstitialSkip.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialSkip.LoadAd(request);
    }

    private void RequestInterstitialLose()
    {
        this.interstitialLose = new InterstitialAd(adUnitId_interstitial_lose);

        this.interstitialLose.OnAdLoaded += HandleOnAdLoaded;
        this.interstitialLose.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.interstitialLose.OnAdOpening += HandleOnAdOpened;
        this.interstitialLose.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialLose.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        switch (idInterstitial)
        {
            case 1:
                RequestInterstitialSkip();
                break;

            case 2:
                RequestInterstitialLose();
                break;
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {

    }

    public void ShowInterstitialVideo(int idAds)
    {
        switch (idAds)
        {
            case 1:
                idInterstitial = 1;
                if (interstitialSkip.IsLoaded() && PlayerPrefs.GetInt("ads") == 0)
                {
                    interstitialSkip.Show();
                }
                break;

            case 2:
                idInterstitial = 2;
                if (interstitialLose.IsLoaded() && PlayerPrefs.GetInt("ads") == 0)
                {
                    interstitialLose.Show();
                }
                break;
        }  
    }

    //########################################################################################################

    private void RequestRewardedAd()
    {
        this.rewardedAd = new RewardedAd(adUnitId_rewardedAd);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {

    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {

    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        CoreGame.S.NextLevelAfterReward();
    }

    public void ShowRewardedVideo()
    {
        rewardedAd.Show();
    }

    private void OnDestroy()
    {
        // Unsubscribe from reward video event
        this.interstitialSkip.OnAdLoaded -= HandleOnAdLoaded;
        this.interstitialSkip.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        this.interstitialSkip.OnAdOpening -= HandleOnAdOpened;
        this.interstitialSkip.OnAdClosed -= HandleOnAdClosed;

        this.interstitialLose.OnAdLoaded -= HandleOnAdLoaded;
        this.interstitialLose.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        this.interstitialLose.OnAdOpening -= HandleOnAdOpened;
        this.interstitialLose.OnAdClosed -= HandleOnAdClosed;

        this.rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        this.rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
    }

    //########################################################################################################

    //private void RequestBanner()
    //{
    //    this.bannerView = new BannerView(adUnitId_bannerView, AdSize.Banner, AdPosition.Top);
    //    AdRequest request1 = new AdRequest.Builder().Build();
    //    this.bannerView.LoadAd(request1);
    //}
}

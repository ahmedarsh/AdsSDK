
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;



public class GoogleAdMobController : MonoBehaviour
    {
        private BannerView smartBannerView, smartBannerView2;
        private BannerView bannerViewSmal;
        private BannerView bannerViewMediumRec;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        private RewardedInterstitialAd rewardedInterstitialAd;

        public string bannerId = "ca-app-pub-3940256099942544/6300978111";
        public string interId = "ca-app-pub-3940256099942544/1033173712";
        public string rewardInterId = "ca-app-pub-3940256099942544/5354046379";
        public string rewardVideoId = "ca-app-pub-3940256099942544/5224354917";
        [Header("Test App ID")] public string appId = "ca-app-pub-3940256099942544~3347511713";

        public static GoogleAdMobController Instance;

        public bool isDoReward;
        public int RewardAmont;
        private AudioSource buttonClick;

        public enum BannerType
        {
            SmallBanner,
            SmartBannner,
            MediumRec
        }

        private BannerType bannerType;

        #region UNITY MONOBEHAVIOR METHODS

        void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            buttonClick = GetComponent<AudioSource>();
        }

        void Start()
        {
            MobileAds.SetiOSAppPauseOnBackground(true);

            List<String> deviceIds = new List<String>() {AdRequest.TestDeviceSimulator};

            // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
            deviceIds.Add("33BE2250B43518CCDA7DE426D04EE231");
#endif

            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder()
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.False)
                    .SetTestDeviceIds(deviceIds).build();

            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(HandleInitCompleteAction);

        }
private static readonly Queue<Action> _executionQueue = new Queue<Action>();

	public void Update() {
		lock(_executionQueue) {
			while (_executionQueue.Count > 0) {
				_executionQueue.Dequeue().Invoke();
			}
		}
	}

	/// <summary>
	/// Locks the queue and adds the IEnumerator to the queue
	/// </summary>
	/// <param name="action">IEnumerator function that will be executed from the main thread.</param>
	public void Enqueue(IEnumerator action) {
		lock (_executionQueue) {
			_executionQueue.Enqueue (() => {
				StartCoroutine (action);
			});
		}
	}

        /// <summary>
        /// Locks the queue and adds the Action to the queue
	/// </summary>
	/// <param name="action">function that will be executed from the main thread.</param>
	public void Enqueue(Action action)
	{
		Enqueue(ActionWrapper(action));
	}
	
	/// <summary>
	/// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
	/// </summary>
	/// <param name="action">function that will be executed from the main thread.</param>
	/// <returns>A Task that can be awaited until the action completes</returns>
	public Task EnqueueAsync(Action action)
	{
		var tcs = new TaskCompletionSource<bool>();

		void WrappedAction() {
			try 
			{
				action();
				tcs.TrySetResult(true);
			} catch (Exception ex) 
			{
				tcs.TrySetException(ex);
			}
		}

		Enqueue(ActionWrapper(WrappedAction));
		return tcs.Task;
	}

	
	IEnumerator ActionWrapper(Action a)
	{
		a();
		yield return null;
	}

    
        private void OnValidate()
        {
#if UNITY_EDITOR
            bannerId = ReCalculateIds(bannerId);
            interId = ReCalculateIds(interId);
            rewardInterId = ReCalculateIds(rewardInterId);
            rewardVideoId = ReCalculateIds(rewardVideoId);
            appId = ReCalculateIds(appId);
            GoogleMobileAds.Editor.SetMobileAds.SetApId = appId;    
#endif
        }

        public string ReCalculateIds(string inputValue)
        {
            return inputValue.Replace(" ", String.Empty);
        }

        private void OnEnable()
        {
            Invoke("RequestAfterWait", 3f);
        }

        void RequestAfterWait()
        {
            bool isConnected = Awu.IsNetworkAvailable;
            if (isConnected)
            {
                RequestAndLoadRewardedInterstitialAd();
                RequestAndLoadRewardedAd();
                RequestAndLoadInterstitialAd();
            }
        }

        private void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                ShowSmartBanner();
            });
        }



        #endregion

        #region HELPER METHODS

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();
        }

        #endregion

        #region BANNER ADS

        public void RequestBannerAd(BannerType bt, AdSize adSize, AdPosition adPosition)
        {

#if UNITY_EDITOR
            string adUnitId = "unused";

#elif UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";
      string  adUnitId = bannerId;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

            if (bt == BannerType.SmartBannner)
            {
                if (smartBannerView != null)
                {
                    smartBannerView.Destroy();
                }

                smartBannerView = new BannerView(adUnitId, adSize, adPosition);
                Enqueue((() =>
                {
                    smartBannerView.OnAdLoaded += (sender, args) => print("on AdLoad");
                    smartBannerView.OnAdFailedToLoad += (sender, args) => print("on failed load");
                    smartBannerView.OnAdOpening += (sender, args) => print("on Opens");
                    smartBannerView.OnAdClosed += (sender, args) => print("on Closed");
                }));

                smartBannerView.LoadAd(CreateAdRequest());

            }
            else if (bt == BannerType.SmallBanner)
            {
                if (bannerViewSmal != null)
                {
                    bannerViewSmal.Destroy();
                }
                bannerViewSmal = new BannerView(adUnitId, adSize, adPosition);
                
                Enqueue((() =>
                {
                    bannerViewSmal.OnAdLoaded += (sender, args) => print("on AdLoad");
                    bannerViewSmal.OnAdFailedToLoad += (sender, args) => print("on failed load");
                    bannerViewSmal.OnAdOpening += (sender, args) => print("on Opens");
                    bannerViewSmal.OnAdClosed += (sender, args) => print("on Closed");
                }));
                
                bannerViewSmal.LoadAd(CreateAdRequest());
            }
            else if (bt == BannerType.MediumRec)
            {
                if (bannerViewMediumRec != null)
                {
                    bannerViewMediumRec.Destroy();
                }

                bannerViewMediumRec = new BannerView(adUnitId, adSize, adPosition);

                Enqueue((() =>
                {
                    bannerViewMediumRec.OnAdLoaded += (sender, args) => print("on AdLoad");
                    bannerViewMediumRec.OnAdFailedToLoad += (sender, args) => print("on failed load");
                    bannerViewMediumRec.OnAdOpening += (sender, args) => print("on Opens");
                    bannerViewMediumRec.OnAdClosed += (sender, args) => print("on Closed");
                }));

                bannerViewMediumRec.LoadAd(CreateAdRequest());
            }

        }
        
        public void ShowSmartBanner()
        {
            bool isConnected = Awu.IsNetworkAvailable;
            if (isConnected)
            {
                if (PlayerPrefs.GetInt("RemoveAds") != 1)
                {
                    if (smartBannerView == null)
                    {
                        RequestBannerAd(BannerType.SmartBannner, AdSize.SmartBanner, AdPosition.Top);
                        print("Request Smart ");
                    }
                    else
                    {
                        smartBannerView.Show();
                    }
                }
            }
            else
            {
                DestroySmartBanner();
            }
        }

        public void SetSmartBannerPos(AdPosition adPosition)
        {
            smartBannerView.SetPosition(adPosition);
        }

        public void ShowSmallBanner(AdPosition adPosition)
        {
            bool isConnected = Awu.IsNetworkAvailable;
            if (isConnected)
            {
                if (PlayerPrefs.GetInt("RemoveAds") != 1)
                {
                    if (bannerViewSmal == null)
                    {
                        RequestBannerAd(BannerType.SmallBanner, AdSize.Banner, adPosition);
                    }
                    else
                    {
                        bannerViewSmal.Show();
                    }
                }
            }
            else
            {
                DestroySmallBanner();
            }

        }

        public void SetMedRecPos(AdPosition adPosition)
        {
            bannerViewMediumRec.SetPosition(adPosition);
        }

        public void ShowMediumRec()
        {
            bool isConnected = Awu.IsNetworkAvailable;
            if (isConnected)
            {
                if (PlayerPrefs.GetInt("RemoveAds") != 1)
                {
                    if (bannerViewMediumRec == null)
                    {
                        RequestBannerAd(BannerType.MediumRec, AdSize.MediumRectangle, AdPosition.TopRight);
                    }
                    else
                    {
                        bannerViewMediumRec.Show();
                    }

                }
            }
            else
            {
                DestroyMediumRec();
            }
        }


        public void DestroySmartBanner()
        {
            if (PlayerPrefs.GetInt("RemoveAds") != 1)
            {
                if (smartBannerView != null)
                {
                    smartBannerView.Hide();
                    print("Destroy smart");
                }
            }
        }

        public void DestroySmallBanner()
        {
            if (PlayerPrefs.GetInt("RemoveAds") != 1)
            {
                if (bannerViewSmal != null)
                {
                    bannerViewSmal.Hide();
                }
            }
        }

        public void DestroyMediumRec()
        {
            if (PlayerPrefs.GetInt("RemoveAds") != 1)
            {
                if (bannerViewMediumRec != null)
                {
                    bannerViewMediumRec.Hide();
                }
            }
        }

        #endregion

        #region INTERSTITIAL ADS

        public void RequestAndLoadInterstitialAd()
        {
#if UNITY_EDITOR
            string adUnitId = "unused";
            //  adUnitId = interId;
#elif UNITY_ANDROID
       // string adUnitId = "ca-app-pub-3940256099942544/1033173712";
      string  adUnitId = interId;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

            // Clean up interstitial before using it
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
            }

            interstitialAd = new InterstitialAd(adUnitId);


            Enqueue((() =>
            {
                interstitialAd.OnAdLoaded += (sender, args) => { print(" AdLoaded"); };
                interstitialAd.OnAdFailedToLoad += (sender, args) =>
                {
                    print(" onAdFailedToLoad");
                    RequestAndLoadRewardedAd();
                };
                interstitialAd.OnAdOpening += (sender, args) => { print(" adOpens"); };
                interstitialAd.OnAdClosed += (sender, args) =>
                {
                    print(" adClosed");
                    RequestAndLoadInterstitialAd();
                };
            }));
            // Add Event Handlers


            // Load an interstitial ad
            interstitialAd.LoadAd(CreateAdRequest());
        }

        public void ShowRewardAds()
        {
            bool isConnected = Awu.IsNetworkAvailable;
            if (isConnected)
            {
                if (PlayerPrefs.GetInt("RemoveAds") != 1)
                {
                    if (rewardedInterstitialAd != null)
                    {
                        rewardedInterstitialAd.Show((reward) =>
                        {
                            MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {

                                var amont = PlayerPrefs.GetInt("Coins");
                                PlayerPrefs.SetInt("Coins", amont + 50);
                            });
                        });
                    }

                    else if (rewardedAd != null)
                    {
                        rewardedAd.Show();
                    }
                    else if (interstitialAd.IsLoaded())
                    {
                        interstitialAd.Show();
                    }
                }
            }

        }

        public void ShowInterstitialAd()
        {

            bool isConnected = Awu.IsNetworkAvailable;
            if (isConnected)
            {
                if (PlayerPrefs.GetInt("RemoveAds") != 1)
                {
                    if (interstitialAd.IsLoaded())
                    {
                        interstitialAd.Show();
                    }
                    else
                    {
                        RequestAndLoadInterstitialAd();
                    }
                }
            }

        }

        public void DestroyInterstitialAd()
        {
            if (PlayerPrefs.GetInt("RemoveAds") != 1)
            {
                if (interstitialAd != null)
                {
                    interstitialAd.Destroy();
                }
            }

        }

        #endregion

        #region REWARDED ADS

        public void RequestAndLoadRewardedAd()
        {
            // statusText.text = "Requesting Rewarded Ad.";
#if UNITY_EDITOR
            string adUnitId = "unused";

#elif UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/5224354917";
      string  adUnitId = rewardVideoId;
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

            // create new rewarded ad instance
            rewardedAd = new RewardedAd(adUnitId);

            // Add Event Handlers

            Enqueue((() =>
            {
                rewardedAd.OnAdLoaded += (sender, args) => print("R ad Loaded");
                rewardedAd.OnAdFailedToLoad += (sender, args) =>
                {
                    print("R ad failed Loaded");
                    RequestAndLoadRewardedAd();
                };
                rewardedAd.OnAdOpening += (sender, args) => print("R ad opened");
                rewardedAd.OnAdFailedToShow += (sender, args) => print("R ad failed to show");
                rewardedAd.OnAdClosed += (sender, args) =>
                {
                    print("R closed");
                    RequestAndLoadRewardedAd();
                };
                rewardedAd.OnUserEarnedReward += (sender, args) =>
                {

                    if (isDoReward)
                    {
                        var amont = PlayerPrefs.GetInt("TotalCoins");
                        PlayerPrefs.SetInt("TotalCoins", amont + RewardAmont);
                        // MMenuController.instance.UpdateCoins();
                        isDoReward = false;
                    }
                    else
                    {
                        var amont = PlayerPrefs.GetInt("TotalCoins");
                        PlayerPrefs.SetInt("TotalCoins", amont + 50);
                        // MMenuController.instance.UpdateCoins();
                    }

                };
            }));



            rewardedAd.LoadAd(CreateAdRequest());
        }

        public void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Show();
            }
            else
            {
                RequestAndLoadRewardedAd();
            }
        }

        public void RequestAndLoadRewardedInterstitialAd()
        {
          
#if UNITY_EDITOR
            string adUnitId = "unused";

#elif UNITY_ANDROID
          //  string adUnitId = "ca-app-pub-3940256099942544/5354046379";
       string adUnitId = rewardInterId;
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif


            RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), rIadLoadCallback);

        }

        private void rIadLoadCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs arg2)
        {
            rewardedInterstitialAd = ad;
            Enqueue((() =>
            {
                rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
                {
                    print("Ad Failed to Load");
                    RequestAndLoadRewardedAd();

                };
                rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
                {
                    print("Ad did to Load");
                };
                rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
                {
                    print("Ad Dismiss to Load");
                    RequestAndLoadRewardedInterstitialAd();
                };
                ;
                rewardedInterstitialAd.OnPaidEvent += (sender, args) => { print("Ad Failed to Load"); };
                ;
            }));

        }

        public void ShowRewardedInterstitialAd()
        {
            if (rewardedInterstitialAd != null)
            {
                rewardedInterstitialAd.Show((reward) =>
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {

                        var amont = PlayerPrefs.GetInt("Coins");
                        PlayerPrefs.SetInt("Coins", amont + 50);
                    });
                });
            }
            else
            {
                RequestAndLoadRewardedInterstitialAd();
            }
        }

        public void VibrateMedium()
        {
            Vibration.Vibrate();
        }

        public void ButtonClick()
        {
            Vibration.VibrateLight();
            if(buttonClick) buttonClick.Play();
        }
        #endregion
    }
    
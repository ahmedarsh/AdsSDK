using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomAdPanel : MonoBehaviour
{
   public List<GameObject> btns=new List<GameObject>();

   public bool showAd = false;
   public bool isInter = false;
   public bool isMeBannr;

   public bool isbannerDown;
   public AdPosition adPosition;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (btns.Count > 0)
        {
            int range = Random.Range(0, btns.Count);
            foreach (var obj in btns)
            {
                obj.SetActive(false);
            }
            btns[range].SetActive(true);
        }
       
        if(showAd)
            GoogleAdMobController.Instance.ShowRewardAds();

        if (isInter)
        {
           GoogleAdMobController.Instance.ShowInterstitialAd();
          
        }

        if (isMeBannr)
        {
            
            GoogleAdMobController.Instance.ShowMediumRec();
            GoogleAdMobController.Instance.SetMedRecPos(adPosition);
        }

        if (isbannerDown)
        {
            /*GoogleAdMobController.Instance.DestroySmartBanner();
            GoogleAdMobController.Instance.ShowSmartBannerBottum();*/
            GoogleAdMobController.Instance.DestroySmartBanner();
            GoogleAdMobController.Instance.ShowSmartBanner();
            GoogleAdMobController.Instance.SetSmartBannerPos(AdPosition.Bottom);
            print("is show smart banner ");
        }
    }

    
  
    public void OpenURL(string url)
    {
        
        Application.OpenURL(url);
    }

    private void OnDisable()
    {
        if (isMeBannr)
        {   
            GoogleAdMobController.Instance.DestroyMediumRec();
        }
        
        if (isbannerDown)
        {
            /*GoogleAdMobController.Instance.DestroySmartBannerBottum();
            GoogleAdMobController.Instance.ShowSmartBanner();*/
            GoogleAdMobController.Instance.DestroySmartBanner();
            GoogleAdMobController.Instance.ShowSmartBanner();
            GoogleAdMobController.Instance.SetSmartBannerPos(AdPosition.Top);
            print("is destroy smart banner ");
        }
    }
}

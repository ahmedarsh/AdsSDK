
using UnityEditor;
using UnityEngine;

namespace GoogleMobileAds.Editor
{
   
    
    public class SetMobileAds :  UnityEditor.Editor
    {
        public static string SetApId
        {
            set
            {
                GoogleMobileAdsSettings.Instance.GoogleMobileAdsAndroidAppId = value;
            }
        }
    
       
    }
}
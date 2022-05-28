using System;
using System.Collections;
using System.Collections.Generic;

using System.Globalization;


#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS


#endif
using UnityEngine;
using Random = UnityEngine.Random;

public class MobileNotificationManager : MonoBehaviour
{
    public string[] messages;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("IsFirstLaunch")==0)
        {
            PlayerPrefs.SetInt("IsFirstLaunch", 1);
         //   UserPrefs.isFirstLaunch = false;
            
            SetDefaultNotification();
        }
        else
        {
            //GAManager.Instance.LogDesignEvent("GamePlay:HouseStart:" + 0);
            //if (UserPrefs.selectedHouse >= UserPrefs.eventHouseIndex)
            //    GAManager.Instance.LogDesignEvent("GamePlay:LayerUnlocked:House" + " Event_House_" + (UserPrefs.selectedHouse - GameData.Data.GetDefaultHouseLength()) + ":Walls_Framing");
            //else
            //    GAManager.Instance.LogDesignEvent("GamePlay:LayerUnlocked:House" + UserPrefs.selectedHouse + ":Walls_Framing");
         //   FlurrySDK.Flurry.LogEvent("HouseStart_" + 0);

         //   UserPrefs.isFirstLaunch = true;
          //  UserPrefs.SetValue("IsFirstLaunch", false);
#if UNITY_ANDROID
            
            CreateAndroidNotification();  
#endif
        }
    }

    private void SetDefaultNotification()
    {
#if UNITY_ANDROID

      
            CreateAndroidNotification();
#else

        StartCoroutine(RequestAuthorization());
#endif
    }

#if UNITY_ANDROID

    private void CreateAndroidNotification()
    {
        int index = Random.Range(0, messages.Length);
       var message = messages[index];
        Debug.Log("GT>> sending notification call: " + message);


        var time = GetGameTime("NotificationTime", true);

        if (time.TotalSeconds > 0 && time.TotalSeconds < 28900)
        {
            Debug.Log("GT>> sending notification return");
            return;
        }


        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.RegisterNotificationChannel(c);
        var notification = new AndroidNotification();
        notification.Title = Application.productName;
        notification.Text = message;
        notification.Color = new Color32(0xff, 0x44, 0x44, 255);

        notification.FireTime = System.DateTime.Now.AddSeconds(28800);
        notification.RepeatInterval = System.TimeSpan.FromDays(1);// UnityEngine.iOS.CalendarUnit.Day;
        notification.SmallIcon = "app_icon";
        notification.LargeIcon = "app_icon";

        var notificationIdentifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
    /*private void CreateAndroidHalloweenNotification()
    {
        var message = "Halloween House Available To Build";
        Debug.Log("GT>> sending notification call: " + message);

        var c1 = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.RegisterNotificationChannel(c1);
        var notification = new AndroidNotification();
        notification.Title = Application.productName;
        notification.Text = message;
        notification.Color = new Color32(0xff, 0x44, 0x44, 255);

        notification.FireTime = System.DateTime.Now.AddSeconds(10);
        //notification.RepeatInterval = System.TimeSpan.FromDays(1);// UnityEngine.iOS.CalendarUnit.Day;
        notification.SmallIcon = "app_icon";
        notification.LargeIcon = "app_icon";

        var notificationIdentifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        Debug.Log("Here We Go");
        if (!UserPrefs.GetValue("HalloweenNote", false))
            UserPrefs.SetValue("HalloweenNote", true);
    }*/
#else

    /*private IEnumerator RequestAuthorization()
    {
        Debug.Log("gt>> Request Authorization");
        using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound, false))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization: \n";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;

            if (req.Granted)
            {
                CreateIONotification();
            }
            Debug.Log(res);
        }
    }

    /*private void CreateIOHalloweenNotification()
    {
        var message = "Halloween House Available To Build";
        Debug.Log("GT>> sending notification call: " + message);

        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new System.TimeSpan(0, 0, 0, 10),
            Repeats = true
        };

        var iosNotification = new iOSNotification
        {
            Title = Application.productName,
            Body = message,
            Identifier = "notification_01",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "Fun",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger
        };

        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.ScheduleNotification(iosNotification);
      
    }#1#
    private void CreateIONotification()
    {
        int index = Random.Range(0, messages.Length);
        var message = messages[index];        Debug.Log("GT>> sending notification call: " + message);

        var time = GetGameTime("NotificationTime", true);

        if (time.TotalSeconds > 0 && time.TotalSeconds < 86500)
        {
            return;
        }

        SetGameTime("NotificationTime");


        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new System.TimeSpan(1, 0, 0, 0),
            Repeats = true
        };

        var iosNotification = new iOSNotification
        {
            Title = Application.productName,
            Body = message,
            Identifier = "notification_01",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "Fun",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger
        };

        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.ScheduleNotification(iosNotification);
        
    }*/
#endif
    
    
    public static void SetGameTime(string key)
    {
        string FMT = "O";
        PlayerPrefs.SetString(key, DateTime.Now.ToString(FMT));
    }

    public static TimeSpan GetGameTime(string key, bool setIfNull = false, bool isInverse = false)
    {
        string FMT = "O";
        string lastRewardTimeStr = PlayerPrefs.GetString(key);

        if (!string.IsNullOrEmpty(lastRewardTimeStr))
        {
            var lastRewardTime = DateTime.ParseExact(lastRewardTimeStr, FMT, CultureInfo.InvariantCulture);
            if (isInverse)
                return (lastRewardTime - DateTime.Now);
            else
                return (DateTime.Now - lastRewardTime);
        }
        else
        {
            if (setIfNull)
            {
                SetGameTime(key);
            }
            return TimeSpan.Zero;
        }
    }
}
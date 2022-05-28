using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager,
// one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.

public class IAPManagerTemplate : MonoBehaviour, IStoreListener
{
    public static IAPManagerTemplate Instance { set; get; }

    private static IStoreController m_StoreController;
    // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider;
    // The store-specific Purchasing subsystems.

    

    //  public static string RemoveAds = "remove_ads";
    //  public static string UnlockLevels = "unlock_levels";
    //  public static string OneThousandCoins = "one_th_coins";


    // KEYS CONSUMABLE
    public static string dollars_2 = "buy_get_twohundrad_coins";
    public static string dollars_4 = "buy_unlcok_all_levels";
    public static string dollars_5 = "buy_remove_ads";
    public static string dollars_6 = "buy_unlcok_all_vehicles";
    public static string dollars_10 = "buy_unlock_all_game";
    
   
    
    



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //_freeCoinsRewardVideo = GetComponent<FreeCoinsRewardVideo> ();
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.


        //*************************************

        // builder.AddProduct(RemoveAds, ProductType.NonConsumable);

        // Continue adding the non-consumable product.
        // builder.AddProduct(UnlockLevels, ProductType.NonConsumable);

        // builder.AddProduct(OneThousandCoins, ProductType.Consumable);

        //******************************************

        // Continue adding the consumable product.
        builder.AddProduct(dollars_2, ProductType.Consumable);
        builder.AddProduct(dollars_4, ProductType.NonConsumable);
        builder.AddProduct(dollars_5, ProductType.NonConsumable);
        builder.AddProduct(dollars_6, ProductType.NonConsumable);
        builder.AddProduct(dollars_10, ProductType.NonConsumable);
       


        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    
    //******************** PURCHASE INAPP METHODS ***************
    public void BuyTwoHundradCoins()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(dollars_2);
    }
    public void BuyRemoveAds()
    {
        // Buy the consumable product using its general identifier. Expect a response either
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(dollars_5);
    }

    public void BuyUnlockLevels()
    {
        // Buy the consumable product using its general identifier. Expect a response either
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(dollars_4);
    } 
    public void BuyUnlockVehicles()
    {
        // Buy the consumable product using its general identifier. Expect a response either
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(dollars_6);
    }

    public void BuyUnlockAllGame()
    {
        // Buy the consumable product using its general identifier. Expect a response either
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(dollars_10);
    }


   

    public void BuyEightThousands()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(dollars_4);
    }


    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }




    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, dollars_2, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 1000 coins to the player's in-game score.
            updateCoins(200);
        }
       
        else if (String.Equals(args.purchasedProduct.definition.id, dollars_4, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 1000 coins to the player's in-game score.
            UnlockAllLevels();
        }
        
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, dollars_5, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // The consumable item has been successfully purchased, add 1000 coins to the player's in-game score.
            UnlockRremoveAds();
        } 
        else if (String.Equals(args.purchasedProduct.definition.id, dollars_6, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            UnlockAllLevelsVehicles();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, dollars_10, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
            UnlockEveryThing();
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

  


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    
    
     void UnlockEveryThing()
     {
         UnlockAllLevels();

         UnlockAllLevelsVehicles();
         UnlockRremoveAds();
         
     } 
     void UnlockAllLevels()
    {
      //  PlayerPrefs.SetInt("LevelCompleted",LevelManager.instance.m_Levels.Count);
       // LevelManager.instance.LevelLocking();
    } 
     void UnlockAllLevelsVehicles()
    {
       // for (int i = 0; i <VehiclesManager.instance.Vehicles.Count; i++)
       // {
       //     PlayerPrefs.SetInt("Buy" + i, 1);
      //  }
        
    } 
     void UnlockRremoveAds()
    {
          
        PlayerPrefs.SetInt("RemoveAds",1);
        GoogleAdMobController.Instance.DestroySmartBanner();
    }
     
     public void updateCoins(int Coins)
     {
         PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins"+ Coins));
      //   MMenuController.instance.UpdateCoins();
     }
     public void GetExtraCoins(int amount)
     {
         GoogleAdMobController.Instance.RewardAmont = amount;
         GoogleAdMobController.Instance.isDoReward = true;
         GoogleAdMobController.Instance.ShowRewardedAd();
        
     }
    
}

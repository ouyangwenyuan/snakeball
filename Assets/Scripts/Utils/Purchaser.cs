﻿using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager,
// one of the existing Survival Shooter scripts.
namespace CompleteProject
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class Purchaser : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        //********************* 内购品 id start *****************************
        //com.puzzles.jigsawlegendios.monthly_subscribe
        //		将用于报告的唯一字母数字 ID。
        //		com.puzzles.jigsawlegendios.weekly_subscribe
        //
        //		com.puzzles.jigsawlegendios.yearly_subscribe
        //********************* 内购品 id end *****************************

        //订阅】、ID
        public static string kProductNameAppleSubscriptionWeekly = GlobalValues.purchaseProductId;


        void Start()
        {
            // If we haven't set up the Unity Purchasing reference
            //			if (m_StoreController == null)
            //			{
            //				// Begin to configure our connection to Purchasing
            //				InitializePurchasing();
            //			}
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
            //			builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            // Continue adding the non-consumable product.
            //			builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs
            // must only be referenced here.

            //			builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
            //				{ kProductNameAppleSubscription, AppleAppStore.Name },
            //				{ kProductNameGooglePlaySubscription, GooglePlay.Name },
            //			});

            builder.AddProduct(kProductNameAppleSubscriptionWeekly, ProductType.Subscription);
            // builder.AddProduct (kProductNameAppleSubscriptionMonthly, ProductType.Subscription);
            // builder.AddProduct (kProductNameAppleSubscriptionYearly, ProductType.Subscription);

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void BuySubscriptionWeekly()
        {
            BuyProductID(kProductNameAppleSubscriptionWeekly);
        }


        void BuyProductID(string productId)
        {
            Debug.Log("BuyProductID,productId-->>" + productId + ", IsInitialized-->>" + IsInitialized());
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
                    VisiblenLoding();
                    // ... report the product look-up failure situation
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                VisiblenLoding();
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }


        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google.
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            Debug.Log("================RestorePurchases===========");
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                VisiblenLoding();
                return;
            }

            // If we are running on an Apple device ...
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.

                apple.RestoreTransactions((result) =>
                {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then
                    // no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                    if (!result)     //RestorePurchases  Fail
                    {
                        Debug.Log("RestorePurchases result   =======     Fail");
                        VisiblenLoding();
                        GlobalValues.isPurchase = false;
                        PlayerPrefs.SetInt("PurchaseType", 0);
                        StatisticsUtils.LogAppEvent("RestorePurchases", "OnRestore", "Fail");
                    }
                    else
                    {
                        Debug.Log("RestorePurchases result   =======     Success");
                        StatisticsUtils.LogAppEvent("RestorePurchases", "OnRestore", "Success");
                    }
                });
            }
            // Otherwise ...
            else
            {
                VisiblenLoding();
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        //
        // --- IStoreListener
        //

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
            VisiblenLoding();
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        //购买成功回调
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (String.Equals(args.purchasedProduct.definition.id, kProductNameAppleSubscriptionWeekly, StringComparison.Ordinal))
            {
                //统计购买成功的国家 
                //                StatisticsUtils.LogAppEvent("Purchase_Country", "Success_CountryCode", MyCountryTool.getCountryCode());
                Debug.Log(string.Format("ProcessPurchase: PASS. weekly, Product: '{0}'", args.purchasedProduct.definition.id));
                // TODO: The subscription item has been successfully purchased, grant this to the player.
                PurchaseManager.instance.onPurchanseSuccess(1);

            }
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                VisiblenLoding();
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still
            // saving purchased products to the cloud, and when that save is delayed.

            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {

            VisiblenLoding();
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }


        void VisiblenLoding()
        {
            if (PurchaseManager.instance != null)
            {
                PurchaseManager.instance.OnLodingImg(false);
            }
        }
    }


}
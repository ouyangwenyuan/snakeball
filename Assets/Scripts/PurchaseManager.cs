using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CompleteProject;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class PurchaseManager : MonoBehaviour
{
    public InlineText _text;
    public Transform StrongPurView;
    public Transform rootView;
    public Transform LodingView;
    public Purchaser purchaser;
    public Text mMoneyText;
    private static PurchaseManager _instance;
    //Get singleton
    public static PurchaseManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PurchaseManager>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        //初始化内购
        Purchaser.kProductNameAppleSubscriptionWeekly = GlobalValues.purchaseProductId;
        purchaser.InitializePurchasing();
    }
    // Use this for initialization
    void Start()
    {
        OnLodingImg(false);
        //投放模式
        if (GlobalValues.openPurchase)
        {
            StrongPurView.gameObject.SetActive(true);
        }
        else
        {
            StrongPurView.gameObject.SetActive(false);
        }

        mMoneyText.text = GlobalValues.price + "/week";
		string test = "\n\nSubscription options include:\n\nYou can unlock all levels, enjoy unlimited tips, and feel free to play new levels (weekly updated) during the subscription. You can also cancel it at any time.\nCrazy Ball Premium (paid weekly) for 3-Days Free Trial then " + GlobalValues.price + " / week Payment will be charged to iTunes Account at confirmation of purchase.\nSubscription automatically renews unless auto-renew is turned off at least 24-hours before the end of the current period.\nAccount will be charged for renewal within 24-hours prior to the end of the current period, and identify the cost of the renewal.\nSubscriptions may be managed by the user and auto-renewal may be turned off by going to the user’s Account Settings after purchase.\nAny unused portion of a free trial period, if offered, will be forfeited when the user purchases a subscription to that publication, where applicable.\nPlease visit [-101#https://support.apple.com/en-us/HT202039] for more information.\nPrivacy Policy：\n[-102#https://sites.google.com/view/crazy-ball/privacy-policy]\nTerms of Use:\n[-103#https://sites.google.com/view/crazy-ball/terms-of-use]";
        _text.text = test;
    }
    public void CloseThisScene()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        SceneManager.LoadScene("MenuScene");
    }

    public void OnSubscribeClick()
    {
        OnLodingImg(true);
        StatisticsUtils.LogAppEvent("Win_Purchaser", "Click_Buy", "OnSubscribeClick");
        purchaser.BuySubscriptionWeekly();
    }

    public void OnRestoreClick()
    {
        OnLodingImg(true);
        StatisticsUtils.LogAppEvent("Win_Purchaser", "OnRestoreClick", "OnRestoreClick");
        purchaser.RestorePurchases();
    }

    public void onPurchanseSuccess(int type)
    {
        OnLodingImg(false);
        //保存玩家数据
        savePurchase(type);
        //进入主界面
        SceneManager.LoadScene("MenuScene");
        StatisticsUtils.LogAppEvent("OnPurchaseFinish", "OnPurchaseFinish", "buy success");
    }

    /*
    * SavePurchase(type) 
    */
    public void savePurchase(int type)
    {
        System.DateTime now = System.DateTime.Now;
        long nowSecond = now.TotalSeconds();
        PlayerPrefs.SetString("LastCheckPurchaseTime", nowSecond + "");
        GlobalValues.isPurchase = true;
        PlayerPrefs.SetInt("PurchaseType", type);
    }

    public void OnLodingImg(bool isHin)
    {
        LodingView.gameObject.SetActive(isHin);
    }

    void OnEnable()
    {
        _text.OnHrefClick.AddListener(OnHrefClick);
    }

    void OnDisable()
    {
        _text.OnHrefClick.RemoveListener(OnHrefClick);
    }

    private void OnHrefClick(string hrefName, int id)
    {
        Debug.Log("点击了 " + hrefName + "  id:" + id);
        switch (id)
        {
            case 101:   //Terms of Use
                cancel_subscribe_button();
                break;
            case 102:   //Privacy Policy
                purchase_click_PrivacyPolicy();
                break;
            case 103:   //Subscription
                purchase_click_TermsOfServices();
                break;
        }
    }

	//点击TermsOfServices响应时间
    private void purchase_click_TermsOfServices()
    {
        StatisticsUtils.LogAppEvent("Win_Purchaser", "Services", "PrivacyPolicy");
        GlobalValues.servicesType = "TermsOfServices";
        ServicesManager.getInstance(rootView).ToogleActiveState(true);

    }
	//点击PrivacyPolicy响应时间
    private void purchase_click_PrivacyPolicy()
    {
        StatisticsUtils.LogAppEvent("Win_Purchaser", "Services", "PrivacyPolicy");
        GlobalValues.servicesType = "PrivacyPolicy";
        ServicesManager.getInstance(rootView).ToogleActiveState(true);
    }
	//取消订阅链接
    private void cancel_subscribe_button()
    {
        StatisticsUtils.LogAppEvent("Win_Purchaser", "Services", "Cancel_subscribe");
        Application.OpenURL("https://support.apple.com/en-us/HT202039");
    }
}
	
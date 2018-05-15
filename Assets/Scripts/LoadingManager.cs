using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.IO;
using Facebook.Unity;
using CompleteProject;
using System.Collections.Generic;

public class LoadingManager : MonoBehaviour
{
    void Awake()
    {
//		PlayerPrefs.DeleteAll ();
        //初始化 Facebook 
        if (!FB.IsInitialized)
            FB.Init(InitCallback, OnHideUnity);
        else
            FB.ActivateApp();
    }

    private void InitCallback()
    {
        // 初始化完成回掉
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    // Use this for initialization
    void Start()
    {
#if DEBUG
        PlayerPrefs.DeleteAll();
#endif
        //检查是否购买
        CheckPurchase();
        //加载订阅配置文件
        DownloadController.Instance.GetString(Configs.urlPurchaseConfig, eventHandlerPurchase, eventOnError, overTime);
    }

    void CheckPurchase()
    {
        GlobalValues.isPurchase = PlayerPrefs.GetInt("PurchaseType", 0) == 1 ? true : false;
    }

    //**************  订阅配置文件  **************//
    private void eventHandlerPurchase(string text, Dictionary<string, string> responseHeaders)
    {
        Debug.LogWarning(" 购买配置文件请求 ====成功 ==== >>>" + text);
        PlayerPrefs.SetString("purchaseConfig_data", text);
        initDate(text);

    }

    private void eventOnError(string error)
    {
        Debug.LogWarning("购买配置文件请求失败 ==== >>>" + error + "=>" + Configs.urlPurchaseConfig);
        string text = PlayerPrefs.GetString("purchaseConfig_data");
        if (!string.IsNullOrEmpty(text))
        {
            initDate(text);
        }
    }

    private void overTime(string progress)
    {
        Debug.LogWarning("网络超时加载进度 =============" + progress);
        if (GlobalValues.IsDownloadOverTime)
        {
            string text = PlayerPrefs.GetString("purchaseConfig_data");
            if (!string.IsNullOrEmpty(text))
            {
                initDate(text);
            }
        }
    }
    //初始化数据
    void initDate(string text)
    {
        PurchaseConfig purchaseConfig = JsonUtility.FromJson<PurchaseConfig>(text);
        //投放模式开关
        GlobalValues.openPurchase = purchaseConfig.onoff == 1 ? true : false;
        //审核开关（直接进入程序）
        GlobalValues.need_display_uurcahse = purchaseConfig.need_display_uurcahse == 1 ? true : false;

        NormalPayment normalPayment = purchaseConfig.normal_purchases;
        WeeklyOne weeklyOne = normalPayment.weeklyOne;
        WeeklyTwo weeklyTwo = normalPayment.weeklyTwo;
        string weeklyOne_id = weeklyOne.purchase_product_id;
        string weeklyTwo_id = weeklyTwo.purchase_product_id;
        Debug.LogWarning("weeklyOne_id=====>>>>>>" + weeklyOne_id + "        weeklyTwo_id=====>>>>>>" + weeklyTwo_id);
        if (!string.IsNullOrEmpty(weeklyOne_id) && !string.IsNullOrEmpty(weeklyTwo_id))
        {
            //根据审核开关  选择对应的价格
            if (GlobalValues.need_display_uurcahse)
            {
                PlayerPrefs.SetString("purchase_product_id", weeklyOne_id);
                GlobalValues.purchaseProductId = weeklyOne_id;
                GlobalValues.price = weeklyOne.price;
            }
            else
            {
                PlayerPrefs.SetString("purchase_product_id", weeklyTwo_id);
                GlobalValues.purchaseProductId = weeklyTwo_id;
                GlobalValues.price = weeklyTwo.price;
            }
        }
        else
        {
            PlayerPrefs.SetString("purchase_product_id", purchaseConfig.purchase_product_id);
            GlobalValues.purchaseProductId = purchaseConfig.purchase_product_id;
            GlobalValues.price = purchaseConfig.price;
        }
        Debug.LogWarning("purchaseProductId=====>>>>>>" + GlobalValues.purchaseProductId);

        StartCoroutine(LoadMainOrPurScene());
    }
    //跳转
    IEnumerator LoadMainOrPurScene()
    {
        yield return new WaitForSeconds(1f);
        if (!GlobalValues.need_display_uurcahse || GlobalValues.isPurchase || LoadingManager.isChina() || GlobalValues.isIPad())
            SceneManager.LoadScene("HomeScene");   //进入菜单页面
        else
            SceneManager.LoadScene("PurchaseScene"); //进入订阅页面

    }
	//判断中国区用户  进行区分对待 
    public static bool isChina()
    {
        string countryCode = MyCountryTool.getCountryCode();
        // string countryCode = "en_CN";
        bool flag = true;
        Debug.LogWarning("所在国家=======" + countryCode);
        // StatisticsUtils.LogAppEvent("Country", "CountryCode", countryCode);
        if (countryCode.Equals("en_CN") || countryCode.Equals("zh_CN"))
        {
            //表示设备在中国地区
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;

    }
}

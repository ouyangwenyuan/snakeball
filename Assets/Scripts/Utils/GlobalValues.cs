using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;
using System;
public class GlobalValues : MonoBehaviour
{
    //产品名字
    public static string APP_NAME = "Crazy Ball";
    public static string packageName = "com.puzzlegame.crazyball";
    //TODO:  上线前检查此变量
    public static string enableRelease = "1"; // debug = 0 ,release = 1
    public static string appVersion = "1.0.0";
    public static string appVersionBuild = "(100)";
    public static string startTime = "2018-04-27";
    public static string servicesType;
    //产品价格
    public static string price = "$0.99";
    public static string purchaseProductId = "crazyball.subscribe.weekly01";
    //审核开关
    public static bool need_display_uurcahse = false;
    //投放模式开关
    public static bool openPurchase = false;
    //内购成功开关 
    public static bool isPurchase = false;
    //全局控制声音
    public static string SoundSetting = "SoundSetting";
    //是否超时
    public static bool IsDownloadOverTime = false;


    public static bool isIPad()
    {
        return (DeviceGeneration.iPadUnknown == Device.generation ||
        Device.generation == DeviceGeneration.iPadMini1Gen ||
        Device.generation == DeviceGeneration.iPadMini2Gen ||
        Device.generation == DeviceGeneration.iPadMini3Gen ||
        Device.generation == DeviceGeneration.iPadMini4Gen ||
        Device.generation == DeviceGeneration.iPadPro1Gen ||
        Device.generation == DeviceGeneration.iPadPro10Inch1Gen ||
        Device.generation == DeviceGeneration.iPad1Gen ||
        Device.generation == DeviceGeneration.iPad2Gen ||
        Device.generation == DeviceGeneration.iPad3Gen ||
        Device.generation == DeviceGeneration.iPad4Gen ||
        Device.generation == DeviceGeneration.iPadAir1 ||
        Device.generation == DeviceGeneration.iPadAir2);
    }

    public static bool isPhoneX()
    {
        float screenWidth = 1125;
        float screenHeight = 2436;
        if (Screen.width == screenWidth && Screen.height == screenHeight)
            return true;
        else
            return false;
    }

}

public class Configs
{
    // 支付配置
    public static string urlPurchaseConfig = "http://gameof.thrones.trafficmanager.net/legend/init?appid=" + GlobalValues.packageName + "&version=" + GlobalValues.appVersion;

}

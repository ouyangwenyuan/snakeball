using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServicesManager : MonoBehaviour
{
    private GameObject servicesView;
    private Text TitleName, AboutText, SubText;
    private GameObject priScroll, TermsScroll, SubScrollView, AboutUs;
    //获取脚本实例
    public static ServicesManager getInstance(Transform parent)
    {
        //每个parent 中只有一个SettingScript, 说白了就是单例
        if (parent == null)
        {
            Debug.LogError("========parent  must  not  be null========");
            return null;
        }
		//设置界面相关内容
        ServicesManager listener = parent.gameObject.GetComponent<ServicesManager>();
        GameObject itemObj = null;
        if (listener == null)
        {
            listener = parent.gameObject.AddComponent<ServicesManager>();
            GameObject o = Resources.Load("Prefabs/ServicesPrefab", typeof(GameObject)) as GameObject;
            itemObj = (GameObject)Instantiate(o, parent);
            itemObj.name = "ServicesPrefab";
            listener.TitleName = GameObject.Find("TitleView/TitleName").GetComponent<Text>();
            listener.priScroll = GameObject.Find("PriScrollView");
            listener.TermsScroll = GameObject.Find("TermsScrollView");
            listener.SubScrollView = GameObject.Find("SubScrollView");
            listener.AboutUs = GameObject.Find("AboutUs");
            listener.AboutText = GameObject.Find("AboutUs/bg/content/AboutText").GetComponent<Text>();
            listener.SubText = GameObject.Find("SubScrollView/TextInline/SubText").GetComponent<Text>();
        }
        else
        {
            itemObj = parent.Find("ServicesPrefab").gameObject;
        }

        //放在最外层
        itemObj.transform.SetSiblingIndex(itemObj.transform.parent.GetChildCount() - 1);
        listener.servicesView = itemObj;

		//条款界面相关内容 
        if (GlobalValues.servicesType == "TermsOfServices")
        {
            listener.TitleName.text = "Terms Of Services";
            listener.TermsScroll.SetActive(true);
            listener.priScroll.SetActive(false);
            listener.SubScrollView.SetActive(false);
            listener.AboutUs.SetActive(false);
        }
        else if (GlobalValues.servicesType == "PrivacyPolicy")
        {
            listener.TitleName.text = "Privacy Policy";
            listener.priScroll.SetActive(true);
            listener.TermsScroll.SetActive(false);
            listener.SubScrollView.SetActive(false);
            listener.AboutUs.SetActive(false);
        }
        else if (GlobalValues.servicesType == "Subscription")
        {
            listener.TitleName.text = "Subscription Agreement";
            listener.SubText.text = "\n\nSubscription options include:\n\nYou can unlock all levels, enjoy unlimited tips, and feel free to play new levels (weekly updated) during the subscription. You can also cancel it at any time.\nCrazy Ball Premium (paid weekly) for 3-Days Free Trial then " + GlobalValues.price + " / week Payment will be charged to iTunes Account at confirmation of purchase.\nSubscription automatically renews unless auto-renew is turned off at least 24-hours before the end of the current period.\nAccount will be charged for renewal within 24-hours prior to the end of the current period, and identify the cost of the renewal.\nSubscriptions may be managed by the user and auto-renewal may be turned off by going to the user’s Account Settings after purchase.\nAny unused portion of a free trial period, if offered, will be forfeited when the user purchases a subscription to that publication, where applicable.\nPlease visit [-101#https://support.apple.com/en-us/HT202039] for more information.\n\n\n.";
            listener.priScroll.SetActive(false);
            listener.TermsScroll.SetActive(false);
            listener.SubScrollView.SetActive(true);
            listener.AboutUs.SetActive(false);
        }
		//about US界面内容 
        else if (GlobalValues.servicesType == "AboutUs")
        {
            listener.priScroll.SetActive(false);
            listener.TermsScroll.SetActive(false);
            listener.SubScrollView.SetActive(false);
            listener.AboutUs.SetActive(true);
            listener.AboutText.text = GlobalValues.appVersion + GlobalValues.appVersionBuild;
        }

        return listener;
    }


    //点击设置按钮时调用 example: SettingScript.getInstance(transform).toogleActiveState(true);
    public void ToogleActiveState(bool isShow)
    {
        Debug.Log("toogleActiveState isShow-->>" + isShow);
        servicesView.SetActive(isShow);
    }

}
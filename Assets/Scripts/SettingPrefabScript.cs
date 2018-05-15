using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingPrefabScript : MonoBehaviour
{
    public Image toogleButton;
    public Sprite spriteEnable;
    public Sprite spriteDisable;

    void Start()
    {
        initSoundBtn();
    }

    public void onClickClose()
    {
        GameObject o = GameObject.Find("settingPanel");
        if (o != null)
        {
            o.SetActive(false);
            SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        }
    }
    private void initSoundBtn()
    {
        if (GameManager.soundEnabled)
        {
            toogleButton.sprite = spriteEnable;
        }
        else
        {
            toogleButton.sprite = spriteDisable;
        }
    }
    public void onClickSoundButton()
    {
        Debug.Log("onClickSoundButton");
        GameManager.soundEnabled = !GameManager.soundEnabled;
        if (GameManager.soundEnabled)
        {
            PlayerPrefs.SetInt(GlobalValues.SoundSetting, 1);
            toogleButton.sprite = spriteEnable;
            SoundManager.instance.PlaySound(SoundManager.instance.music);
        }
        if (!GameManager.soundEnabled)
        {
            PlayerPrefs.SetInt(GlobalValues.SoundSetting, 0);
            toogleButton.sprite = spriteDisable;
            SoundManager.instance.music.Stop();
        }
    }

    public void onClickTerms()
    {
        Debug.Log("onClickTerms");
        SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        GlobalValues.servicesType = "TermsOfServices";
        ServicesManager.getInstance(transform).ToogleActiveState(true);
    }
    public void onClickPricyPlocy()
    {
        Debug.Log("onClickPricyPlocy");
        SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        GlobalValues.servicesType = "PrivacyPolicy";
        ServicesManager.getInstance(transform).ToogleActiveState(true);

    }
    public void onClickSubscriptionAgreement()
    {
        Debug.Log("onClickSubscriptionAgreement");
        SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        GlobalValues.servicesType = "Subscription";
        ServicesManager.getInstance(transform).ToogleActiveState(true);
    }

    public void onClickAboutUs()
    {
        Debug.Log("onClickAboutUs");
        SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        GlobalValues.servicesType = "AboutUs";
        ServicesManager.getInstance(transform).ToogleActiveState(true);
    }
}

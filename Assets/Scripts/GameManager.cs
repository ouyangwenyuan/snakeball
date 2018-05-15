using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static int currentPack;
    public static int currentLevel;
    public static bool soundEnabled;
    public GameObject FreePre;
    public GameObject WeeklyPre;
    public Button ItemsPre;
    private DateTime dt1;

    void Start()
    {
        CreatMainMenu();
    }
    //创建主界面 进行相关初始化 
    void CreatMainMenu()
    {
        GameObject panel = GameObject.Find("Content");
        dt1 = Convert.ToDateTime(GlobalValues.startTime);
        DateTime dt2 = System.DateTime.Now;
        TimeSpan ts1 = new TimeSpan(dt1.Ticks);
        TimeSpan ts2 = new TimeSpan(dt2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        int dateDiff = ts.Days;
        Debug.LogWarning(" 时间差============== >>>" + dateDiff.ToString() + "天");
        int picks = dateDiff / 7 + 2;
        if (picks >= 19)  //最多19周
        {
            picks = 19;
        }
        for (int i = 0; i < picks; i++)
        {
            GameObject ItemPre = null;
            int m;
            if (i == 0)
            {
                ItemPre = Instantiate(FreePre);
                ItemPre.transform.SetParent(panel.transform, false);
                m = 5;
            }
            else
            {
                ItemPre = Instantiate(WeeklyPre);
                ItemPre.transform.SetParent(panel.transform, false);
                Text ItemTitle = ItemPre.transform.Find("ItemTitle").GetComponent<Text>();
                ItemTitle.text = "WEEKLY PACK " + i;
                Text ItemTime = ItemPre.transform.Find("ItemTime").GetComponent<Text>();
                ItemTime.text = dt1.ToString("yyyy/MM/dd") + "-" + dt1.AddDays(7).ToString("yyyy/MM/dd");
                dt1 = dt1.AddDays(7);
                m = 9;
            }
            GameObject ItemContent = ItemPre.transform.Find("ItemContent").gameObject;
            //每周8关卡
            for (int j = 1; j < m; j++)
            {
                Button ItemButton = Instantiate(ItemsPre);
                ItemButton.transform.SetParent(ItemContent.transform, false);
                Image ItemColor = ItemButton.transform.GetComponent<Image>();
                ItemColor.color = new Color32(228, 228, 228, 255);
                LevelButtonScript buttonScript = ItemButton.GetComponent<LevelButtonScript>(); // 获取脚本对象
                GameObject buttonUnlocked = ItemButton.transform.Find("locked").gameObject;
                buttonUnlocked.SetActive(true);
                Text num = ItemButton.GetComponentInChildren<Text>();
                num.text = j.ToString();
                num.gameObject.SetActive(false);
                buttonScript.SetDate(i, j);
                //纪录关卡名称
                string levelStr = "LP" + i + "_" + "level-" + j;
                int isLock = 0;
                if (i == 0 && j == 1)
                {
                    isLock = 1;
                }
                //首次进入 初始化
                if (!PlayerPrefs.HasKey(levelStr))
                    PlayerPrefs.SetInt(levelStr, isLock);
                else
                    isLock = PlayerPrefs.GetInt(levelStr);

                //关卡是否解锁
                if (isLock != 0)
                {
                    num.gameObject.SetActive(true);
                    buttonUnlocked.SetActive(false);
                    if (i == 0)
                        ItemColor.color = new Color32(0, 255, 14, 255);
                    else
                        ItemColor.color = new Color32(0, 186, 255, 255);
                }
            }
        }
    }

    public void clickBackHome()
    {
        SceneManager.LoadScene("HomeScene");   //进入菜单页面
    }



}

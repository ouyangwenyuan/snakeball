using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LevelButtonScript : MonoBehaviour
{
    private int pics;
    private int level;
    private string levelStr;

    //添加每组数据的价格等级等
    public void SetDate(int pics, int level)
    {
        this.pics = pics;
        this.level = level;
        this.levelStr = "LP" + pics + "_" + "level-" + level;
    }
    //加载level 以及点击音效 
    public void LoadLevel()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.btnclick);
        GameManager.currentPack = pics;
        GameManager.currentLevel = level;
        int isLock = PlayerPrefs.GetInt(levelStr);
        if (isLock == 1 || GlobalValues.isPurchase)
        {
            SceneManager.LoadScene("MainGame");
        }
        else if (pics != 0)
        {
            SceneManager.LoadScene("PurchaseScene");
        }

    }

}

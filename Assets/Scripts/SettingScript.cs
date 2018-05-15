using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SettingScript : MonoBehaviour
{
    private GameObject settingO;


    //获取脚本实例
    public static SettingScript getInstance(Transform parent)
    {
        //每个parent 中只有一个SettingScript, 说白了就是单例
        if (parent == null)
        {
            Debug.LogError("========parent  must  not  be null========");
            return null;
        }

        SettingScript listener = parent.gameObject.GetComponent<SettingScript>();
        GameObject itemObj = null;
        if (listener == null)
        {
            listener = parent.gameObject.AddComponent<SettingScript>();
            GameObject o = Resources.Load("Prefabs/SettingPrefab", typeof(GameObject)) as GameObject;
            itemObj = (GameObject)Instantiate(o, parent);
            itemObj.name = "settingPanel";
        }
        else
        {
            itemObj = parent.Find("settingPanel").gameObject;
        }

        //放在最外层
        itemObj.transform.SetSiblingIndex(itemObj.transform.parent.GetChildCount() - 1);
        listener.settingO = itemObj;
        return listener;

    }


    //点击设置按钮时调用 example: SettingScript.getInstance(transform).toogleActiveState(true);
    public void toogleActiveState(bool isShow)
    {

        Debug.Log("toogleActiveState isShow-->>" + isShow);
        settingO.SetActive(isShow);
    }

}

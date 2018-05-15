using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{

    public Transform rootView;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void clickSetting()
    {
        SettingScript.getInstance(rootView).toogleActiveState(true);
    }

}

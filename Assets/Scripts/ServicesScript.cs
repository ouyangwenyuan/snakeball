using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ServicesScript : MonoBehaviour
{
    public void onClickClose()
    {
        GameObject o = GameObject.Find("ServicesPrefab");
        if (o != null)
        {
            o.SetActive(false);
        }
    }

}

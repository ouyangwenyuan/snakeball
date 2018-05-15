using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
public class MyCountryTool : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern string _getCountryCode();
    public static string getCountryCode()
    {
#if DEBUG
        Debug.Log("=====debug getCountryCode======");
        return "";
#endif
        Debug.Log("=====release getCountryCode======");
        return _getCountryCode();
    }

}

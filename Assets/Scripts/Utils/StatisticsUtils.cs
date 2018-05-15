using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fabric.Answers;
using Facebook.Unity;

//埋点统计
public class StatisticsUtils
{
    public static void LogAppEvent(string m_MapKey, string m_Key, string m_valoue)
    {
        Answers.LogCustom(m_MapKey, customAttributes: new Dictionary<string, object>()
        {
            {m_Key, m_valoue}
        });
        FB.LogAppEvent(m_MapKey, null, new Dictionary<string, object>()
        {
            {m_Key, m_valoue}
        });
        Purchase(m_MapKey, m_Key, m_valoue);
    }

    public static void Purchase(string m_MapKey, string m_Key, string m_valoue)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add(m_Key, m_valoue);
        AppsFlyer.trackRichEvent(m_MapKey, eventValue);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public delegate void EventHandlerHTTPString(string text, Dictionary<string, string> responseHeaders);
public delegate void EventHandlerHTTPTexture(Texture2D texture, Dictionary<string, string> responseHeaders);
public delegate void EventHnadlerHTTPAssetBundle(AssetBundle assetBundle, Dictionary<string, string> responseHeaders);
public delegate void EventHandlerOnError(string error);
public delegate void EventHandlerOverTime(string time);

public class DownloadController : MonoBehaviour
{
    public float beforeTime;
    public float beforeProgress;
    public float TIME_OUT = 10f;

    public static DownloadController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DownloadController>();
            }
            return _instance;
        }
    }
    private static DownloadController _instance = null;

    public bool CheckTimeOut(WWW request)
    {
        float now = Time.time;
        if (now - beforeTime > TIME_OUT) return true;

        if (request.progress != beforeProgress)
        {
            beforeTime = now;
            beforeProgress = request.progress;
        }
        return false;
    }

    public void PostForm(string url, WWWForm formData, EventHandlerHTTPString stringCallback, EventHandlerOnError errorCallback)
    {
        StartCoroutine(RunPostFormCoroutine(url, formData, stringCallback, errorCallback));
    }

    public void GetString(string url, EventHandlerHTTPString stringCallback, EventHandlerOnError errorCallback, EventHandlerOverTime overTime = null)
    {
        StartCoroutine(RunGetStringCoroutine(url, stringCallback, errorCallback, overTime));
    }

    public void GetTexture(string url, EventHandlerHTTPTexture textureCallback, EventHandlerOnError errorCallback)
    {
        StartCoroutine(RunGetTextureCoroutine(url, textureCallback, errorCallback));
    }

    public void GetAssetBundle(string url, EventHnadlerHTTPAssetBundle assetBuntleCallback, EventHandlerOnError errorCallback)
    {
        StartCoroutine(RunGetAssetBundleCoroutine(url, assetBuntleCallback, errorCallback));
    }

    public void GetFile(string url, string dbfilename)
    {
        StartCoroutine(RunGetFileCoroutine(url, dbfilename));
    }

    private IEnumerator RunPostFormCoroutine(string url, WWWForm formData, EventHandlerHTTPString stringCallback, EventHandlerOnError errorCallback)
    {
        WWW www = new WWW(url, formData);

        while (!www.isDone)
        {
            yield return null;

        }

        if (string.IsNullOrEmpty(www.error))
        {
            if (stringCallback != null)
            {
                stringCallback(www.text, www.responseHeaders);
            }
            else
            {
                Debug.Log("<Color=#4f3c3c>no request callback method.</color>");
                yield return null;
            }
        }
        else
        {
            if (errorCallback != null)
                errorCallback(www.error);
        }

        if (www != null)
        {
            www.Dispose();
            www = null;
        }
    }

    private IEnumerator RunGetStringCoroutine(string url, EventHandlerHTTPString stringCallback, EventHandlerOnError errorCallback, EventHandlerOverTime overTime = null)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return null;

            if (overTime != null && CheckTimeOut(www) && !GlobalValues.IsDownloadOverTime)
            {
                GlobalValues.IsDownloadOverTime = true;
                overTime("progress" + www.progress);
            }
        }

        if (string.IsNullOrEmpty(www.error))
        {
            if (stringCallback != null)
            {
                stringCallback(www.text, www.responseHeaders);
            }
            else
            {
                Debug.Log("no request Log Error");
                yield return null;
            }
        }
        else
        {
            if (errorCallback != null)
                errorCallback(www.error);
        }

        if (www != null)
        {
            www.Dispose();
            www = null;
        }
    }

    private IEnumerator RunGetTextureCoroutine(string url, EventHandlerHTTPTexture textureCallback, EventHandlerOnError errorCallback)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(www.error))
        {
            if (textureCallback != null)
            {
                textureCallback(www.texture, www.responseHeaders);
            }
            else
            {
                Debug.Log("<Color=#4f3c3c>no request callback method.</color>");
                yield return null;
            }
        }
        else
        {
            if (errorCallback != null)
                errorCallback(www.error);
        }

        if (www != null)
        {
            www.Dispose();
            www = null;
        }
    }

    private IEnumerator RunGetFileCoroutine(string url, string dbfilename)
    {
        string filename = string.Format("{0}/{1}", Application.persistentDataPath, dbfilename);

        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        byte[] bytes;

        WWW www = new WWW(url);
        yield return www;
        bytes = www.bytes;

        if (bytes != null)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    private IEnumerator RunGetAssetBundleCoroutine(string url, EventHnadlerHTTPAssetBundle assetBundleCallback, EventHandlerOnError errorCallback)
    {
        WWW www = new WWW(url);

        while (!www.isDone)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(www.error))
        {
            if (assetBundleCallback != null)
            {
                assetBundleCallback(www.assetBundle, www.responseHeaders);
            }
            else
            {
                Debug.Log("<Color=#4f3c3c>no request callback method.</color>");
                yield return null;
            }
        }
        else
        {
            if (errorCallback != null)
                errorCallback(www.error);
        }

        if (www != null)
        {
            www.Dispose();
            www = null;
        }
    }


}

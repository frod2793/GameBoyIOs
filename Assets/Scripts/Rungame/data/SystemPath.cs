using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class SystemPath : MonoBehaviour
{
    public static string GetPath(string fileName)
    {
        string path = GetPath();
        return Path.Combine(GetPath(), fileName);
    }

    public static string GetPath()
    {
        string path = null;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Application.persistentDataPath, "Resources/rungame/friendRun/data/");
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Assets", "Resources/rungame/friendRun/data/");
            case RuntimePlatform.WindowsEditor:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Assets", "Resources/rungame/friendRun/data/");
            default:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Resources/rungame/friendRun/data/");
        }
    }
}

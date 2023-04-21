using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SettingsDialog : MonoBehaviour
{
    [SerializeField]
    GameObject apply;

    [SerializeField]
    GameObject cancel;

    [SerializeField]
    GameObject github;

    [SerializeField]
    GameObject steam;

    [SerializeField]
    GameObject close;

    Dictionary<string, string> buttonRefs = new Dictionary<string, string>();

    public string GetGameObjectPath(GameObject obj)
    {
        string stop = gameObject.name;

        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            string parentName = obj.name;
            if (parentName == stop) break;

            path = "/" + parentName + path;
        }

        return path.Substring(1, path.Length - 1);
    }

    public string GenerateJson()
    {
        if(apply) buttonRefs.Add("apply", GetGameObjectPath(apply));
        if(steam) buttonRefs.Add("steam", GetGameObjectPath(steam));
        if(close) buttonRefs.Add("close", GetGameObjectPath(close));
        if(cancel) buttonRefs.Add("cancel", GetGameObjectPath(cancel));
        if(github) buttonRefs.Add("github", GetGameObjectPath(github));

        return JsonConvert.SerializeObject(buttonRefs, Formatting.Indented);
    }
}

using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveJson(string fileName, string json)
    {
        var path = GetPath(fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);
    }

    public static string LoadJson(string fileName)
    {
        var path = GetPath(fileName);
        return File.Exists(path) ? File.ReadAllText(path) : null;
    }

    private static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, "HuadongFeimeng", fileName);
    }

    public static string ToJsonPretty<T>(T data)
    {
        return JsonUtility.ToJson(data, true);
    }

    public static T FromJson<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return default;
        return JsonUtility.FromJson<T>(json);
    }
}


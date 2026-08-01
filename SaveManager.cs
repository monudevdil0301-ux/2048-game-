using UnityEngine;

public static class SaveManager
{
    public static void SaveInt(
        string key,
        int value
    )
    {
        PlayerPrefs.SetInt(
            key,
            value
        );

        PlayerPrefs.Save();
    }

    public static int GetInt(
        string key,
        int defaultValue = 0
    )
    {
        return PlayerPrefs.GetInt(
            key,
            defaultValue
        );
    }

    public static void SaveString(
        string key,
        string value
    )
    {
        PlayerPrefs.SetString(
            key,
            value
        );

        PlayerPrefs.Save();
    }

    public static string GetString(
        string key,
        string defaultValue = ""
    )
    {
        return PlayerPrefs.GetString(
            key,
            defaultValue
        );
    }

    public static void DeleteKey(
        string key
    )
    {
        PlayerPrefs.DeleteKey(
            key
        );

        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();

        PlayerPrefs.Save();
    }
}
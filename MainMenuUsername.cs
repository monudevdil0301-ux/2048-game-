using TMPro;
using UnityEngine;

public class MainMenuUsername : MonoBehaviour
{
    public TMP_Text usernameText;

    void Start()
    {
        RefreshUsername();
    }

    public void RefreshUsername()
    {
        usernameText.text = PlayerPrefs.GetString("Username", "Player");
    }
}
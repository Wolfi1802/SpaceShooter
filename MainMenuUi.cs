using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MainMenuUi : MonoBehaviour
{
    public HighscoreManager highscoreManager;
    public GameObject HighscoreEntries;
    public GameObject HihgscoreENtryUiPrefab;

    private void Start()
    {
        this.highscoreManager.Load();
        ShowHighscores();
    }

    private void ShowHighscores()
    {
        for (int i = HighscoreEntries.transform.childCount - 1; i > 0; i--)
        {
            var child = HighscoreEntries.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        var highscores = highscoreManager.GetList();

        foreach (var item in highscores)
        {
            var highscoreEntry = Instantiate(HihgscoreENtryUiPrefab, HighscoreEntries.transform);
            var textMeshPro = highscoreEntry.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = $"{item.Score} - {item.Name}";

        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void CloseGame()
    {
        this.highscoreManager.Save();
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            Application.Quit();
        }
    }
}

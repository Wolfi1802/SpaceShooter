using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class HighscoreManager : MonoBehaviour
{
    [Serializable]
    public class HighscoreContainer
    {
        public HighscoreEntry[] Highscores;
    }

    [Serializable]
    public class HighscoreEntry
    {
        public string Name;
        public int Score;
    }

    private const string FileName = "Highscore.Json";
    private const int MaxHighscores = 5;

    public string HighscoreFilePath => Path.Combine(Application.persistentDataPath, FileName);

    private List<HighscoreEntry> highscores = new List<HighscoreEntry>();

    private void Awake()
    {
        this.Load();
    }

    private void OnDestroy()
    {
        this.Save();
    }

    public void Save()//TODO [TS] laden/speichern ist komplet bruch weil funktioniert mal so mal so
    {
        var highscoreContainer = new HighscoreContainer()
        {
            Highscores = highscores.ToArray()
        };
        var json = JsonUtility.ToJson(highscoreContainer);
        Debug.Log(json);
        File.WriteAllText(this.HighscoreFilePath, json);
    }

    public void Load()
    {
        if (!File.Exists(this.HighscoreFilePath))
        {
            return;
        }

        var json = File.ReadAllText(this.HighscoreFilePath);
        Debug.Log(json);
        var highscoreContainer = JsonUtility.FromJson<HighscoreContainer>(json);

        if (highscoreContainer == null || highscoreContainer.Highscores == null)
        {
            return;
        }

        this.highscores = new List<HighscoreEntry>(highscoreContainer.Highscores);
    }

    private void Add(HighscoreEntry entry)
    {
        Debug.Log(entry.Score);
        highscores.Insert(0, entry);

        highscores = highscores.Take(MaxHighscores).ToList();
    }

    public bool IsNewHighscore(int score)
    {
        if (score < 0)
            return false;
        if (highscores.Count == 0)
            return true;

        var firstEntry = highscores[0];
        return score > firstEntry.Score;
    }

    public void Add(string playerName, int score)
    {
        if (!IsNewHighscore(score))
        {
            return;
        }

        var entry = new HighscoreEntry()
        {
            Name = playerName,
            Score = score
        };
        Add(entry);
    }

    public List<HighscoreEntry> GetList()
    {
        return this.highscores;
    }
}

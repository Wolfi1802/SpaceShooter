using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI PointsText;
    public GameObject HealthContainer;
    public GameObject HealtUIItemPrefab;

    public GameObject GameOverUi;
    public GameObject HighScoreUi;
    public TMP_InputField NameInputField;
    public HighscoreManager HighscoreManager;

    private int points = 0;

    private void Awake()
    {
        GameOverUi.SetActive(false);
        HighScoreUi.SetActive(false);
    }

    public void GameOver()
    {
        GameOverUi.SetActive(true);

        var isNewHighscore = HighscoreManager.IsNewHighscore(points);
        HighScoreUi.SetActive(isNewHighscore);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddToHighscore()
    {
        var playerName = NameInputField.text;

        if(string.IsNullOrWhiteSpace(playerName))
        {
            return;
        }

        HighscoreManager.Add(playerName, points);
        HighScoreUi.SetActive(false);
    }

    public void AddPoints(int points)
    {
        this.points += points;
        PointsText.text = this.points.ToString();
    }

    public void SetHealth(int health)
    {
        for (int i= HealthContainer.transform.childCount - 1;  i > 0; i--)
        {
            var child = HealthContainer.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < health; i++)
        {
            Instantiate(HealtUIItemPrefab, HealthContainer.transform);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private HighScore currentHighScore;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        currentHighScore = LoadHighScore();
        ShowHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points} Player : {Player.Instance.PlayerName}";

        if (m_Points > currentHighScore.Score)
        {
            currentHighScore.Score = m_Points;
            currentHighScore.Player = Player.Instance.PlayerName;
            ShowHighScore();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScore();
    }

    private HighScore LoadHighScore()
    {
        HighScore highScore;
        Debug.Log($"Checking for highscore at {Application.persistentDataPath + "/highscore.json"}");
        if (File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/highscore.json");
            highScore = JsonUtility.FromJson<HighScore>(json);
            Debug.Log("Highscore found and loaded.");
        }
        else
        {
            highScore = new HighScore();
        }
        Debug.Log($"HighScore : {highScore.Score} by {highScore.Player}");

        return highScore;
    }

    private void ShowHighScore()
    {
        HighScoreText.text = $"HighScore : {currentHighScore.Score} by {currentHighScore.Player}";
    }

    private void SaveHighScore()
    {
        string json = JsonUtility.ToJson(currentHighScore);
        Debug.Log($"json: {json}");
        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
        Debug.Log($"New Highscore saved to {Application.persistentDataPath + "/highscore.json"}!");

    }

    [System.Serializable]
    private class HighScore
    {
        public int Score;
        public string Player;
    }
}

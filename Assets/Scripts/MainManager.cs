using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public static MainManager Instance;

    public Text ScoreText;
    public Text ScoreText2;
    public GameObject GameOverText;
    public int highScore; // Variable to store high score for updating in Game Over

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private string playerName; // Variable to store player name after scene load



    private void Awake()
    {
        Instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        playerName = StartMenuManager.Instance.playerName; // Save player name to input field in StartMenuManager from previous scene
        if (StartMenuManager.Instance != null) // If statement to produce score text ensuring StartMenuManager exists
        {
            ScoreText2.text = "Best Score: " + StartMenuManager.Instance.LoadName() + " : " + StartMenuManager.Instance.LoadScore().ToString();
        }
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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
            StartMenuManager.Instance.playerName = playerName; // Stops the player name from being reset on Game Over
            int previousBest = StartMenuManager.Instance.LoadScore();
            if (m_Points > previousBest) // Check if the current score is higher than the previous best to update high score
            {
                highScore = m_Points;
                StartMenuManager.Instance.SaveNameScore(playerName, highScore);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);       
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}

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
    public int highScore;
    
    private bool m_Started = false;
    private int m_Points;
    private string playerName;
    
    private bool m_GameOver = false;
    private static int loadCount = 0;


    private void Awake()
    {
        loadCount++;
        if (StartMenuManager.Instance != null)
        {
            ScoreText2.text = "Best Score: " + StartMenuManager.Instance.LoadName() + " : " + StartMenuManager.Instance.LoadScore().ToString();
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (loadCount > 1)
        {
            ScoreText2.text = "Best Score: " + StartMenuManager.Instance.LoadName() + " : " + StartMenuManager.Instance.LoadScore().ToString();
        }
       
        playerName = StartMenuManager.Instance.LoadName();
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
        if (m_Points > highScore)
        {
            highScore = m_Points;
            StartMenuManager.Instance.SaveName();
            StartMenuManager.Instance.SaveScore(highScore);
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    /*[System.Serializable]
    public class SaveData
    {
        public int score;
        public string playername;
        public int loadCount;
    }
    public void SaveNameScore(int score)
    {
        SaveData data = new SaveData();
        data.score = score;
        data.playername = playerName;
        data.loadCount = ++loadCount; // Save the load count to save previous player name
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public int LoadNameScore()
    {
        int highScore = 0;
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            data.playername = playerName; // Ensure the player name is loaded correctly
            highScore = data.score;
            loadCount = data.loadCount; // Load the previous load count
        }
        return highScore;
    }*/
}

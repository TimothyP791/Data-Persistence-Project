using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Overlays;
using System.IO;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;
    public TMP_InputField nameInputField;
    public string playerName;
    public TMP_Text highScore;
    public int startScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadName();
    }
    public void Start()
    {
        string name = LoadName();
        int score = LoadScore();

        if (!string.IsNullOrEmpty(name))
        {
            highScore.text = "Best Score: " + name + " : " + score.ToString();
        }
        else
        {
            highScore.text = "Best Score: ";
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string playername;
        public int score;
    }

    public void SaveNameScoreFromUI()
    {
        playerName = nameInputField.text;
        int score = startScore;
    }
    public void SaveNameScore(string name, int score) 
    {
        SaveData data = new SaveData();
        data.playername = name;
        data.score = score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public string LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playername;
            
        }
        return playerName;
    }

    public int LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            startScore = data.score;
            
        }
        return startScore;
    }
}

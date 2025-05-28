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
    public int score;
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
        if (LoadName() != null)
        {
            highScore.text = "Best Score : " + LoadName() + " : " + LoadScore().ToString();
        }
        else
        {
            highScore.text = "Best Score:";
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public string playername;
        public int score;
    }

    public void SaveName()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            playerName = nameInputField.text;
            return; // Ensure this only runs in the start menu scene
        }
        else
        { 
        playerName = LoadName();
        }
        SaveData data = new SaveData();
        data.playername = playerName;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        
    }

    public void SaveScore(int score) //TODO: Figure out why score isn't saving
    {
        SaveData data = new SaveData();
        data.score = score;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public string LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path) && MainManager.Instance != null)
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
        if (File.Exists(path) && MainManager.Instance != null)
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            score = data.score;
            //highScore.text = "Best Score: " + playerName + " : " + data.score.ToString();
        }
        return score;
    }
}

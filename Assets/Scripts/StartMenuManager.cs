using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Overlays;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;
    public TMP_InputField nameInputField;
    public string playerName;
    public TMP_Text highScore;
    public int startScore;
    public bool isInitialized = false;
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

        nameInputField.text = LoadName();
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
            highScore.text = "Best Score: : 0";
        }
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); // this will stop the game in the editor
#else
        Application.Quit(); // this will quit the game
#endif
        string tempName = "";
        int tempScore = 0;
        SaveNameScore(tempName, tempScore);// Change this to store name and highscore once testing of main scene is completed
    }

    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int score;
    }

    /*public void SaveNameScoreFromUI()
    {
        if (LoadScore() == 0)
        {
            playerName = nameInputField.text;
            int score = startScore;
            SaveNameScore(playerName, score);
        }
        else
        {
            playerName = nameInputField.text;
            int score = LoadScore();
            SaveNameScore(playerName, score);
        }
        
    }*/
    public void CapturePlayerName()
    {
        if (nameInputField != null)
        {
            playerName = nameInputField.text;
            isInitialized = true;
        }
        
    }
    public void SaveNameScore(string name, int score) 
    {
        SaveData data = new SaveData();
        data.playerName = name;
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

            playerName = data.playerName;
            
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

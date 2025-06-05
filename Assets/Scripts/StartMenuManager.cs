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
    public int startScore; // score to be saved when the game starts
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake() // Function to ensure only one instance of StartMenuManager exists
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().buildIndex == 0) // Ensures text field is only set when the Start Menu scene is loaded
        {
            playerName = LoadName();

            if (nameInputField != null)
            {
                nameInputField.text = playerName;
            }
        }
    }
    public void Start() //Starting function to load high score text after application end
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
        SaveNameScore(tempName, tempScore);// Means to reset the data so the high score is not saved when the game is closed
    }

    [System.Serializable] //Serializable class to save player name and score to a JSON file
    public class SaveData
    {
        public string playerName;
        public int score;
    }
    public void CapturePlayerName() //Function to capture the player name from the input field when the game starts so it can be used in main scene
    {
        if (nameInputField != null)
        {
            playerName = nameInputField.text;
        }
        
    }
    public void SaveNameScore(string name, int score) //Function to save the player name and score to a JSON file
    {
        SaveData data = new SaveData();
        data.playerName = name;
        data.score = score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public string LoadName() //Function to load the player name from the JSON file
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

    public int LoadScore() //Function to load the score from the JSON file
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

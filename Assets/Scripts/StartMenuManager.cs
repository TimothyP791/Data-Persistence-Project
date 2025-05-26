using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Overlays;
using System.IO;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;
    public TMP_InputField nameInputField;
    public string playerName;
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
    [System.Serializable]
    public class SaveData
    {
        public string playername;
    }

    public void SaveName()
    {
        playerName = nameInputField.text;
        SaveData data = new SaveData();
        data.playername = playerName;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        //Debug.Log("Name saved: " + playerName);
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
}

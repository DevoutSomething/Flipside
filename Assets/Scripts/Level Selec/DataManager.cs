using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string saveFileName = "GameData";
    public string folderName = "SaveData";

    public DefaultData gameData = new DefaultData();

    string defaultPath;
    string fileName;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        defaultPath = Application.persistentDataPath + "/" + folderName;
        fileName = defaultPath + "/" + saveFileName + ".json";

        if (!FolderExists(defaultPath))
        {
            Directory.CreateDirectory(defaultPath);
        }
        LoadGameData();
    }

    bool FolderExists(string folderPath)
    {
        return Directory.Exists(folderPath);
    }
    
    public void LoadGameData()
    {
        if (File.Exists(fileName))
        {
            string saveData = File.ReadAllText(fileName);
            gameData = JsonUtility.FromJson<DefaultData>(saveData);
        }
        else
        {
            SaveGameData();
        }
    }
    public void SaveGameData()
    {
        string saveData = JsonUtility.ToJson(gameData);
        File.WriteAllText(fileName, saveData);
    }
}

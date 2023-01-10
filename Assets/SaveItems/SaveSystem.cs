using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{



    public static void SavePlayer(PlayerDataManager player)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerDataBinary data = new PlayerDataBinary(player);

        formater.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerDataBinary LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.bat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerDataBinary data = formatter.Deserialize(stream) as PlayerDataBinary;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Sace file not found in" + path);
            return null;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFunction : MonoBehaviour
{
    public PlayerDataManager playerData;
    private void Start()
    {
        LoadPlayer();
    }

    public void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            SavePlayer();
        }
        if (Input.GetKeyDown("2"))
        {
            LoadPlayer();
        }
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(playerData);
    }
    public void LoadPlayer()
    {
        PlayerDataBinary data = SaveSystem.LoadPlayer();

        playerData.Level1 = data.Level1;
        playerData.Level2 = data.Level2;
        playerData.Level3 = data.Level3;
        playerData.Level4 = data.Level4;
        playerData.Level5 = data.Level5;
        playerData.Level6 = data.Level6;
    }
}


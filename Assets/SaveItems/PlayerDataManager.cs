using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{

    [Header("Levels Finished")]
    public bool Level1;
    public bool Level2;
    public bool Level3;
    public bool Level4;
    public bool Level5;
    public bool Level6;
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
}

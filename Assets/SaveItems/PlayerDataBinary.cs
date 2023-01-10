using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataBinary
{
    public bool Level1;
    public bool Level2;
    public bool Level3;
    public bool Level4;
    public bool Level5;
    public bool Level6;

    public PlayerDataBinary (PlayerDataManager Player)
    {
        Level1 = Player.Level1;
        Level2 = Player.Level2;
        Level3 = Player.Level3;
        Level4 = Player.Level4;
        Level5 = Player.Level5;
        Level6 = Player.Level6;
    }
}

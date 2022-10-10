using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int currentLevelIndex = 0;
    void Start()
    {
        Invoke("Loadlevel", 3f);
    }
    void Update()
    {
       if(currentLevelIndex + 1 < DataManager.instance.gameData.lockedLevels.Count)
        {
            DataManager.instance.gameData.lockedLevels[currentLevelIndex + 1].isLocked = false;
            DataManager.instance.SaveGameData();
        }
        //SceneManager.LoadScene("Overworld");
    }
}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultData
{
    public string currentLevelName;
    public List<LockedLevels> lockedLevels = new List<LockedLevels>();
}

[System.Serializable]
public class LockedLevels
{
    public string sceneToLoad;
    public bool isLocked; 
}
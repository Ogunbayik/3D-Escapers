using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveService : ISaveService
{
    private const string LEVEL_KEY = "SavedLevelIndex";
    public int GetSavedLevelIndex()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 0);
    }

    public void SaveLevelIndex(int levelIndex)
    {
        PlayerPrefs.SetInt(LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();
    }
}

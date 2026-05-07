using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveService
{
    int GetSavedLevelIndex();
    void SaveLevelIndex(int levelIndex);
}

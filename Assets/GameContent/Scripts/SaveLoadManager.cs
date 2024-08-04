using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : SingletonMB<SaveLoadManager>
{
    private const string LevelReachedKey = "LevelReached";
    private const string LevelStarsKey = "LevelStars_";

    public void SaveProgress(int levelIndex, int stars)
    {
        int currentLevelReached = PlayerPrefs.GetInt(LevelReachedKey, 0);
        if (levelIndex > currentLevelReached)
        {
            PlayerPrefs.SetInt(LevelReachedKey, levelIndex);
        }

        string starsKey = LevelStarsKey + levelIndex;
        PlayerPrefs.SetInt(starsKey, stars);

        PlayerPrefs.Save();
    }

    public int GetStarsForLevel(int levelIndex)
    {
        string starsKey = LevelStarsKey + levelIndex;
        return PlayerPrefs.GetInt(starsKey, 0); 
    }

    public int GetLevelReached()
    {
        return PlayerPrefs.GetInt(LevelReachedKey, 0); 
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LevelReachedKey);

        int currentLevelReached = GetLevelReached();
        for (int i = 0; i <= currentLevelReached; i++)
        {
            PlayerPrefs.DeleteKey(LevelStarsKey + i);
        }

        PlayerPrefs.Save();
    }
}

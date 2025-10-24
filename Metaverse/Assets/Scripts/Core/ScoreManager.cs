using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public static class  ScoreManager
{
    private static int _lastScore;
    public static int LastScore => _lastScore;

    public static int GetHighScore(string key)
    {
        return PlayerPrefs.GetInt($"HS_{key}", 0);
    }

    public static void submitScore(string Key, int score)
    {
        _lastScore = score;
        int hs = GetHighScore(Key);
        if (score > hs)
        {
            PlayerPrefs.SetInt($"HS_{Key}", score);
            PlayerPrefs.Save();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int coins = 0;
    public double distanceHighscore = 0;
    public long highscore = 0;

    public float musicVolume = 1;
    public bool musicMuted = false;
    public float effectsVolume = 1;
    public bool effectsMuted = false;
}

public class PlayerState : MonoBehaviour
{

    public static PlayerState singleton;

    public static event System.Action OnNewHighscore;
    private SaveData saveData = new SaveData();
    

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            saveData = SaveSystem.LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHighscore(long newScore)
    {
        if(newScore > saveData.highscore)
        {
            saveData.highscore = newScore;
        }

        if(OnNewHighscore != null)
        {
            OnNewHighscore();
        }
    }

    public void AddCoins(int coins)
    {
        saveData.coins += coins;
    }

    public int GetCoins()
    {
        return saveData.coins;
    }

    public void SpendCoins(int amount)
    {
        saveData.coins -= amount;
    }

    public void MatchEnded()
    {
        SaveSystem.SaveGame(saveData);
    }

    public long GetHighscore()
    {
        return saveData.highscore;
    }
}

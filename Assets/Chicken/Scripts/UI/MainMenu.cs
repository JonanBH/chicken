using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Text highscoreText;
    [SerializeField]
    private string gameplaySceneName;
    // Start is called before the first frame update
    void Start()
    {
        
        UpdateHighscore();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void UpdateHighscore()
    {
        highscoreText.text = PlayerState.singleton.GetHighscore().ToString();
    }
}

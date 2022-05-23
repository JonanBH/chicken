using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Transform startingPosition;
    [SerializeField]
    private float mapSpeed = 5;
    [SerializeField]
    private Transform projectileParent;
    [SerializeField]
    private float maxHeigth = 3;
    [SerializeField]
    private float gameOverHeigth = -3;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private float distanceToScore = 15;
    [SerializeField]
    private int pointsPerDistance = 1;
    [SerializeField]
    private int scoreToCoinConversion = 50;
    [SerializeField]
    private Button reviveBtn;
    [SerializeField]
    private AdvertiseController adController;

    private double distanceMoved = 0;
    private long score = 0;
    private double lastDistanceScored = 0;
    private int coinsCollected = 0;
    public static bool isPlaying = false;
    public static GameController singleton;
    private bool hasRevived = false;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        CharacterController.OnShootProjectile += HandleProjectileSpawn;

        StartGame();
    }

    private void OnDestroy()
    {
        CharacterController.OnShootProjectile -= HandleProjectileSpawn;
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (Input.GetButtonDown("Fire1"))
        {
            characterController.OnAction();
        }

        if (Input.touchCount > 0)
        {
            UnityEngine.Touch touch = Input.touches[0];

            if(touch.phase == TouchPhase.Began)
            {
                characterController.OnAction();
            }
        }

        MapGenerator.Instance.MoveMap(Vector3.left * mapSpeed * Time.deltaTime);
        distanceMoved += mapSpeed * Time.deltaTime;

        CheckDistanceScore();
    }

    private void StartGame()
    {
        characterController.Init(startingPosition.position);
        score = 0;
        UpdateScore();
        isPlaying = true;
        coinsCollected = 0;
    }

    public void StartGameOnClick()
    {
        StartGame();
    }

    private void HandleProjectileSpawn(GameObject prefab)
    {
        GameObject newProjectile = Instantiate(prefab);
        newProjectile.transform.position = characterController.transform.position;
        newProjectile.transform.SetParent(projectileParent);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 maxHeight = Vector3.up * maxHeigth;
        Vector3 minHeight = Vector3.up * gameOverHeigth;
        Gizmos.DrawLine(Vector3.left * 3 + maxHeight, Vector3.right * 3 + maxHeight);
        Gizmos.DrawLine(Vector3.left * 3 + minHeight, Vector3.right * 3 + minHeight);
    }

    private void CheckDistanceScore()
    {
        while (distanceMoved - lastDistanceScored >= distanceToScore)
        {
            AddScore(pointsPerDistance);
            lastDistanceScored += distanceToScore;
        }
    }

    private int ConvertScoreToCoins()
    {
        return (int)(score / scoreToCoinConversion);
    }

    public float MaxHeigth
    {
        get
        {
            return maxHeigth;
        }
    }

    public float GameOverHeight
    {
        get
        {
            return gameOverHeigth;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "Points = " + score;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        isPlaying = false;

    }

    public void PlayAgain()
    {
        MatchEnded();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        MatchEnded();
        SceneManager.LoadScene("MainMenu");
    }

    public void CallForRevive()
    {
        adController.ShowRewardedAd();
        adController.OnRewardedComplete += Revive;
        adController.OnRewardedFailed += DisableAdsListener;
    }

    private void Revive()
    {
        isPlaying = true;
        characterController.Revive();
        gameOverPanel.SetActive(false);
        reviveBtn.interactable = false;
        DisableAdsListener();
    }

    private void DisableAdsListener()
    {
        adController.OnRewardedComplete -= Revive;
        adController.OnRewardedFailed -= DisableAdsListener;
    }

    private void MatchEnded()
    {
        int totalCoins = coinsCollected + ConvertScoreToCoins();
        
        PlayerState.singleton.AddCoins(totalCoins);
        PlayerState.singleton.UpdateHighscore(score);
        PlayerState.singleton.MatchEnded();
    }

    public void AddCoins(int amount)
    {
        coinsCollected += amount;
    }
}

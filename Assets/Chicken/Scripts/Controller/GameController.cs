using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private double distanceMoved = 0;
    private int score = 0;
    private double lastDistanceScored = 0;
    private int coinsCollected = 0;
    public static bool isPlaying = false;
    public static GameController singleton;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        CharacterController.OnShootProjectile += HandleProjectileSpawn;

        StartGame();
    }

    private void Update()
    {
        //if (!isPlaying) return;

#if UNITY_EDITOR
        if (Input.GetButtonDown("Fire1"))
        {
            characterController.OnAction();
        }
#endif

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
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void AddCoins(int amount)
    {
        coinsCollected += amount;
    }
}

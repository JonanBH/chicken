using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Transform startingPosition;
    [SerializeField]
    private Transform mapParent;

    public static bool isPlaying = false;

    private void Start()
    {
        CharacterController.OnShootProjectile += HandleProjectileSpawn;
    }

    private void Update()
    {
        //if (!isPlaying) return;

        if(Input.touchCount > 0)
        {
            UnityEngine.Touch touch = Input.touches[0];

            if(touch.phase == TouchPhase.Began)
            {
                characterController.Flap();
            }
        }
    }

    private void StartGame()
    {
        characterController.Init(startingPosition.position);
    }

    public void StartGameOnClick()
    {
        StartGame();
    }

    private void GameOver()
    {

    }

    private void HandleProjectileSpawn(GameObject prefab)
    {
        GameObject newProjectile = Instantiate(prefab);
        newProjectile.transform.position = characterController.transform.position;
        newProjectile.transform.SetParent(mapParent);
    }
}

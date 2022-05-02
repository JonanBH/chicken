using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float flapForce = 10;
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private Transform visualAxis;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float blockedTime = 1;
    [SerializeField]
    private LayerMask groundLayers;
    [SerializeField]
    private Transform groundTransformOrigin;
    [SerializeField]
    private float groundDistance = 0.1f;
    [SerializeField]
    private float jumpForce = 10;
    [SerializeField]
    private float maxEnergy = 100;
    [SerializeField]
    private float energyCostPerFlap = 10;
    [SerializeField]
    private float energyRecoverPerSecond = 10;
    

    public event System.Action OnLanded;
    public event System.Action OnFlaped;
    public event System.Action OnJumped;
    public event System.Action OnTookDamage;
    public event System.Action<float, float> OnEnergyUpdated;

    private enum ChickenState { IDLE, WALKING, JUMPING, FLYING, FALLING, DEAD }

    private float currentEnergy;
    private bool playing = true;
    private bool flyBlocked = false;
    private bool isGrounded = true;
    private bool isShielded = false;
    private ChickenState currentState = ChickenState.IDLE;

    public static Action<GameObject> OnShootProjectile;

    public void Init(Vector3 position)
    {
        transform.position = position;
        visualAxis.rotation = Quaternion.identity;
        currentState = ChickenState.FLYING;
        currentEnergy = maxEnergy;
    }

    private void Update()
    {
        GroundCheck();

        HandleCurrentState();
    }

    private void FixedUpdate()
    {
        if (!playing) return;
        if(transform.position.y >= GameController.singleton.MaxHeigth && flyBlocked == false)
        {
            flyBlocked = true;
            Invoke("UnlockFly", blockedTime);
        }

        if(transform.position.y <= GameController.singleton.GameOverHeight)
        {
            playing = false;
            _rigidbody2D.isKinematic = true;
            GameController.singleton.GameOver();
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundTransformOrigin.position, Vector2.down, groundDistance, groundLayers);
        bool newIsGrounded = hit && _rigidbody2D.velocity.y <= 0;

        if(newIsGrounded == true && isGrounded == false)
        {
            OnLanded?.Invoke();
            ChangeState(ChickenState.WALKING);
        }

        isGrounded = newIsGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void UnlockFly()
    {
        flyBlocked = false;
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);

        OnEnergyUpdated?.Invoke(currentEnergy, maxEnergy);
    }

    public void RemoveEnergy(float amount)
    {
        currentEnergy = Mathf.Max(currentEnergy - amount, 0);

        OnEnergyUpdated?.Invoke(currentEnergy, maxEnergy);
    }

    public void OnAction()
    {
        switch (currentState)
        {
            case ChickenState.WALKING:
                Jump();
                break;
            case ChickenState.FLYING:
            case ChickenState.JUMPING:
            case ChickenState.FALLING:
                Flap();
                break;
        }
    }

    private void ChangeState(ChickenState newState)
    {
        currentState = newState;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        ChangeState(ChickenState.JUMPING);

        OnJumped?.Invoke();
    }

    public void TakeDamage()
    {
        if (currentState == ChickenState.DEAD) return;

        if (isShielded)
        {
            isShielded = false;
        }
        else
        {
            GameController.singleton.GameOver();
            ChangeState(ChickenState.DEAD);
        }

        if(OnTookDamage != null)
        {
            OnTookDamage();
        }
    }

    public void Flap()
    {
        if (flyBlocked) return;
        if (currentEnergy < energyCostPerFlap) return;

        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);

        currentEnergy -= energyCostPerFlap;

        OnFlaped?.Invoke();
        OnShootProjectile?.Invoke(projectilePrefab);

        ChangeState(ChickenState.FLYING);
    }

    public void Revive()
    {
        ChangeState(ChickenState.WALKING);
    }

    #region ChickenStates

    private void HandleCurrentState()
    {
        switch (currentState)
        {
            case ChickenState.JUMPING:
                HandleJumpingState();
                break;
            case ChickenState.FLYING:
                HandleFlyingState();
                break;
            case ChickenState.WALKING:
                HandleWalkingState();
                break;
        }
    }

    private void HandleJumpingState()
    {
        if(_rigidbody2D.velocity.y <= 0)
        {
            ChangeState(ChickenState.FALLING);
        }
    }

    private void HandleFlyingState()
    {
        if (_rigidbody2D.velocity.y <= 0)
        {
            ChangeState(ChickenState.FALLING);
        }
    }

    private void HandleWalkingState()
    {
        AddEnergy(energyRecoverPerSecond * Time.deltaTime);
    }
    #endregion
}

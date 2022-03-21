using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    private ScriptableEvent target;
    public UnityEvent triggers;

    private void OnEnable()
    {
        target.AddListener(HandleTrigger);
    }

    private void OnDisable()
    {
        target.RemoveListener(HandleTrigger);
    }

    private void HandleTrigger()
    {
        triggers?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Chicken/Event")]
public class ScriptableEvent : ScriptableObject
{
    private Action OnTriggered;

    public void AddListener(Action listener)
    {
        OnTriggered += listener;
    }

    public void RemoveListener(Action listener)
    {
        OnTriggered -= listener;
    }

    public void Trigger()
    {
        OnTriggered?.Invoke();
    }
}

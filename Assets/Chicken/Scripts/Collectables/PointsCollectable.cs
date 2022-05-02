using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCollectable : Collectable
{
    [SerializeField]
    private int pointsAmount = 1;
    protected override void OnCollect()
    {
        base.OnCollect();

        GameController.singleton.AddScore(pointsAmount);

        Destroy(gameObject);
    }
}

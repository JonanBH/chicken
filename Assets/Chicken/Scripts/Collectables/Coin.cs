using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    [SerializeField]
    private int coinAmount = 1;
    protected override void OnCollect()
    {
        base.OnCollect();

        GameController.singleton.AddCoins(coinAmount);

        Destroy(gameObject);
    }
}

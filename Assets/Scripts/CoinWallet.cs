using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    private float _coinsCost;

    public void Collect(float cost)
    {
        _coinsCost += cost;
    }
}

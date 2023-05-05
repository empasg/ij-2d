using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private float _cost;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out CoinWallet coinWallet))
        {
            coinWallet.Collect(_cost);
            gameObject.SetActive(false);
        }
    }
}

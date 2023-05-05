using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyTouch : MonoBehaviour
{
    [SerializeField] private float _touchDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerLife playerLife))
            playerLife.TakeDamage(_touchDamage);
    }
}

using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private float _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        var newHealth = _health - damageAmount;

        if (newHealth <= 0)
            Die();
        else
            _health = newHealth;
    }

    private void Die()
    {
        transform.root.gameObject.SetActive(false);
    }
}

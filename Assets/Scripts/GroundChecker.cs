using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent _onGround = new UnityEvent();
    [SerializeField] private UnityEvent _onGroundLost = new UnityEvent();
    [SerializeField] private Collider2D[] _filter;

    public event UnityAction OnGround
    {
        add => _onGround.AddListener(value);
        remove => _onGround.RemoveListener(value);
    }
    public event UnityAction OnGroundLost
    {
        add => _onGroundLost.AddListener(value);
        remove => _onGroundLost.RemoveListener(value);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsColliderInFilter(collider) == false)
            _onGround.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (IsColliderInFilter(collider) == false)
            _onGroundLost.Invoke();
    }

    private bool IsColliderInFilter(Collider2D collider)
    {
        foreach (Collider2D collider2D in _filter)
        {
            if (collider2D == collider)
                return true;
        }

        return false;
    }
}

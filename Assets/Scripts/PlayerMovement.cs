using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GroundChecker _groundChecker;

    private bool _isGrounded;
    private bool _isJump;
    private bool _isAnimatorWalkingStatusSetted;
    private Coroutine _timeoutInAirCoroutine;

    private void OnEnable()
    {
        _groundChecker.OnGround += OnGround;
        _groundChecker.OnGroundLost += OnGroundLost;
    }

    private void OnDisable()
    {
        _groundChecker.OnGround -= OnGround;
        _groundChecker.OnGroundLost -= OnGroundLost;
    }

    private void FixedUpdate()
    {
        bool canMove = (_isGrounded || _isJump);

        if (canMove && Input.GetAxis("Horizontal") != 0)
        {
            Vector3 desiredPosition = transform.right * Input.GetAxis("Horizontal") * _speed * Time.deltaTime;

            _animator.SetBool("IsWalking", true);
            _isAnimatorWalkingStatusSetted = false;

            _spriteRenderer.flipX = desiredPosition.x < 0;

            transform.position += desiredPosition;
        }
        else
        {
            if (_isAnimatorWalkingStatusSetted == false)
            {
                _isAnimatorWalkingStatusSetted = true;
                _animator.SetBool("IsWalking", false);
            }
        }

        if (_isGrounded && Input.GetKey(KeyCode.Space))
        {
            _isJump = true;

            _animator.SetTrigger("Jump");

            _timeoutInAirCoroutine = StartCoroutine(TimeoutInAir());

            GetComponent<Rigidbody2D>().AddForce(transform.up * _jumpPower);
        }
    }

    private IEnumerator TimeoutInAir()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        _animator.SetBool("InAir", true);
    }

    private void OnGround()
    {
        if (_timeoutInAirCoroutine != null)
            StopCoroutine(_timeoutInAirCoroutine);

        _isGrounded = true;
        _isJump = false;
        _animator.ResetTrigger("Jump");
        _animator.SetBool("InAir", false);
    }

    private void OnGroundLost()
    {
        _isGrounded = false;

        if (_timeoutInAirCoroutine != null)
            _animator.SetBool("InAir", true);
    }
}

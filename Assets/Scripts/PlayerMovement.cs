using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private const string AnimNameWalking = "IsWalking";
    private const string AnimNameJump = "Jump";
    private const string AnimNameInAir = "InAir";
    private const float AnimDelayInAir = 1f;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GroundChecker _groundChecker;

    private bool _isGrounded;
    private bool _isJump;
    private bool _isAnimatorWalkingStatusSetted;
    private WaitForSecondsRealtime _inAirDelay;
    private Coroutine _inAirTimeoutCoroutine;

    private void Awake()
    {
        _inAirDelay = new WaitForSecondsRealtime(AnimDelayInAir);
    }

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
        bool canJump = _isGrounded && _isJump == false;

        if (canMove && Input.GetAxis("Horizontal") != 0)
        {
            Vector3 desiredPosition = transform.right * Input.GetAxis("Horizontal") * _speed * Time.deltaTime;

            _animator.SetBool(AnimNameWalking, true);
            _isAnimatorWalkingStatusSetted = false;

            _spriteRenderer.flipX = desiredPosition.x < 0;

            transform.position += desiredPosition;
        }
        else
        {
            if (_isAnimatorWalkingStatusSetted == false)
            {
                _isAnimatorWalkingStatusSetted = true;
                _animator.SetBool(AnimNameWalking, false);
            }
        }

        if (canJump && Input.GetKey(KeyCode.Space))
        {
            _isJump = true;

            _animator.SetTrigger(AnimNameJump);

            _inAirTimeoutCoroutine = StartCoroutine(TimeoutInAir());

            GetComponent<Rigidbody2D>().AddForce(transform.up * _jumpPower);
        }
    }

    private IEnumerator TimeoutInAir()
    {
        yield return _inAirDelay;

        _animator.SetBool(AnimNameInAir, true);
    }

    private void OnGround()
    {
        if (_isJump)
            StopCoroutine(_inAirTimeoutCoroutine);

        _isGrounded = true;
        _isJump = false;
        _animator.ResetTrigger(AnimNameJump);
        _animator.SetBool(AnimNameInAir, false);
    }

    private void OnGroundLost()
    {
        _isGrounded = false;

        if (_isJump == false)
            _animator.SetBool(AnimNameInAir, true);
    }
}

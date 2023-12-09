using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _climbSpeed = 4f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _jumpGravityScale = 1f;
    [SerializeField] private float _fallGravityScale = 3f;
    [SerializeField] private float _distanceCheckGround = 1.5f;

    [Header("Attack")]
    [SerializeField] private float _attackRange = 2.5f;

    [Header("Audio")]
    [SerializeField] AudioClip deathSFX;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;
    [SerializeField] private EnemiesBehavior _enemy;

    #region Class
    private Rigidbody2D _rb;
    private Animator _myAnimator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _clambCheckCollider;
    private Camera _camera;
    private AudioSource _audioSource;
    #endregion

    #region Struct
    private LayerMask _groundMask;
    private LayerMask _enemyMask;
    private Vector2 _moveInput;
    private RaycastHit2D _hit;

    private bool _isAlive = true;
    private bool _isJump = false;
    private bool _isGrounded = false;
    private bool _isClimbed = false;
    private bool _playerHasHorizontalSpeed = false;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _clambCheckCollider = GetComponent<BoxCollider2D>();
        _groundMask = LayerMask.GetMask("Ground");
        _enemyMask = LayerMask.GetMask("Enemies");
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isAlive)
        {
            OnAlive();
            OnClimb();
            GroundCheck();
            GravityScale();
            Run();
            Jump();
            FlipSprite();
            Attack();
            Death();
        }
    }

    private void OnMove(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }

        _moveInput = value.Get<Vector2>();
    }

    private void GroundCheck()
    {
        _hit = Physics2D.Raycast(transform.position, Vector2.down, _distanceCheckGround, _groundMask);
        if (_hit.collider)
        {
            _isGrounded = true;
            return;
        }

        _isGrounded = false;
    }

    private void OnClimb()
    {
        if (_clambCheckCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            ClimbLadder();
            _isClimbed = true;
        }
        else
        {
            _isClimbed = false;
            _myAnimator.SetBool("isClimb", _isClimbed);
            _myAnimator.SetBool("isClimbIdle", _isClimbed);
        }
    }

    private void GravityScale()
    {
        if (_rb.velocity.y > 0 && !_isClimbed)
        {
            _rb.gravityScale = _jumpGravityScale;
        }
        else if (_isClimbed)
        {
            _rb.gravityScale = 0f;
        }
        else
        {
            _rb.gravityScale = _fallGravityScale;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        _isJump = !_isGrounded && !_isClimbed;

        _myAnimator.SetBool("isJump", _isJump);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * _runSpeed, _rb.velocity.y);
        _rb.velocity = playerVelocity;

        _playerHasHorizontalSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;

        if (_playerHasHorizontalSpeed)
        {
            FlipSprite();
        }

        _myAnimator.SetBool("isRun", _playerHasHorizontalSpeed);
    }

    private void FlipSprite()
    {
        float currentScaleX = transform.localScale.x;

        if ((_moveInput.x > 0 && currentScaleX < 0) || (_moveInput.x < 0 && currentScaleX > 0))
        {
            transform.localScale = new Vector2(-currentScaleX, transform.localScale.y);
        }
    }


    private void ClimbLadder()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _moveInput.y * _climbSpeed);

        bool playerHasVerticallSpeed = Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon;
        _myAnimator.SetBool("isClimb", playerHasVerticallSpeed);

        if (!playerHasVerticallSpeed && !_isGrounded)
        {
            _myAnimator.SetBool("isClimbIdle", _isClimbed);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _myAnimator.SetTrigger("Attack");
            Invoke(nameof(AttackEnemy), 0.25f);
        }
    }

    private void AttackEnemy()
    {
        Vector2 direction = transform.right;

        if (transform.localScale.x < 0)
        {
            direction = -transform.right;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _attackRange, _enemyMask);


        if (hit.collider != null)
        {
            EnemiesBehavior enemy = hit.collider.GetComponent<EnemiesBehavior>();

            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
    }

    private void Death()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Spikes")))
        {
            _isAlive = false;
            _myAnimator.SetTrigger("Die");
            _myAnimator.SetBool("isAlive", _isAlive);
            _audioSource.PlayOneShot(deathSFX);
            StartCoroutine(DieDelay());
        }
    }

    private IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(2f);
        OnDie();
    }

    private void OnAlive()
    {
        _myAnimator.SetBool("isAlive", true);
    }

    private void OnDie()
    {
        if (!_isAlive)
        {
            _gameSession.ProcessPlayerDeath();
        }
    }
}



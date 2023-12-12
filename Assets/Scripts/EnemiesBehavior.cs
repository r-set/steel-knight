using UnityEngine;

public class EnemiesBehavior : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _distanceCheckPlayer = 3f;

    [Header("Audio")]
    [SerializeField] AudioClip enemyDeathSFX;

    [Header("Scripts")]
    [SerializeField] private PlayerBehavior _player;

    #region Class
    private Rigidbody2D _enemyRb;
    private Animator _enemyAnimator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _groundCollider;
    private Camera _camera;
    private AudioSource _audioSource;
    #endregion

    #region Struct
    private LayerMask _playerMask;
    private RaycastHit2D _hit;
    private Vector3 _initialPosition;

    private bool _isAlive = true;
    private bool _isDying = false;
    #endregion

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _groundCollider = GetComponent<BoxCollider2D>();
        _playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        if (_isAlive)
        {
            OnAlive();
            Move();
            PlayerCheck();
        }
    }

    private void Move()
    {
        _enemyRb.velocity = new Vector2(_moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _moveSpeed = -_moveSpeed;
        FlipEnemy();
        _enemyAnimator.SetBool("isMove", true);
    }

    private void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_enemyRb.velocity.x)), 1f);
    }

    private void PlayerCheck()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        if (distance <= _distanceCheckPlayer)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (_player.transform.position - transform.position).normalized, distance, _playerMask);
        }
    }

    private void OnAlive()
    {
        _enemyAnimator.SetBool("isAlive", true);
    }

    public void TakeDamage()
    {
        if (!_isDying && _isAlive)
        {
            float dealyEnemyDeath = 0.25f;

            _isDying = true;
            _isAlive = false;
            _enemyAnimator.SetBool("isAlive", _isAlive);
            _enemyAnimator.SetTrigger("Hit");
            _enemyRb.velocity = Vector2.zero;
            _enemyRb.gravityScale = 0f;
            _enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;

            Invoke(nameof(PlayDeathAnimation), dealyEnemyDeath);
        }
    }

    private void PlayDeathAnimation()
    {
        _enemyAnimator.SetTrigger("Die");
        _audioSource.PlayOneShot(enemyDeathSFX);
        
        Invoke(nameof(OnDeathAnimationComplete), 1.5f);
    }

    public void OnDeathAnimationComplete()
    {
        _enemyRb.velocity = Vector2.zero;
        _enemyRb.bodyType = RigidbodyType2D.Kinematic;
        _bodyCollider.enabled = false;
        _groundCollider.enabled = false;
        _isDying = false;
        _enemyAnimator.enabled = false;
    }
}

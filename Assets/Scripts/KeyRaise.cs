using UnityEngine;

public class KeyRaise : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _keyRaiseSFX;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;

    private int _quantityKeyRaise = 1;
    private bool _wasCollected = false;

    private Camera _camera;
    private AudioSource _audioSource;

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_wasCollected && _gameSession.key < _gameSession.needKeys)
        {
            _wasCollected = true;
            _gameSession.AddToScore(_quantityKeyRaise);
            _audioSource.PlayOneShot(_keyRaiseSFX);
            Destroy(gameObject);
        }
    }
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Delay")]
    [SerializeField] private float _levelLoadDelay = 1f;

    [Header("Text")]
    [SerializeField] private TMP_Text _exitText;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;

    [Header("Audio")]
    [SerializeField] AudioClip doorOpenSFX;

    private bool _isKeyCollect = false;
    private int _currentSceneIndex;
    private TMP_Text _exitTextInstance;
    private Animator _doorAnimator;
    private Camera _camera;
    private AudioSource _audioSource;
 
    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();
        _doorAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _exitTextInstance = _exitText;
        _exitTextInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        _isKeyCollect = _gameSession.key >= _gameSession.needKeys;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isKeyCollect)
            {
                StartCoroutine(LoadNextLevelWithDelay());
            }
            else
            {
                int keysNeeded = Mathf.Max(0, _gameSession.needKeys - _gameSession.key);
                _exitTextInstance.text = $"You need {keysNeeded} keys";
                _exitTextInstance.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _exitTextInstance.gameObject.SetActive(false);
        }
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = (_currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator LoadNextLevelWithDelay()
    {
        _doorAnimator.SetTrigger("Open");
        _gameSession.key = 0;
        _audioSource.PlayOneShot(doorOpenSFX);
        yield return new WaitForSecondsRealtime(_levelLoadDelay);
        LoadNextLevel();
    }
}
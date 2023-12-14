using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text _exitText;

    [Header("Next Level")]
    [SerializeField] private string _nameNextStage;
    [SerializeField] private int _nextCurrentLevel;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;
    [SerializeField] private StageManager _stageManager;

    [Header("Audio")]
    [SerializeField] private AudioClip _doorOpenSFX;

    private bool _isKeyCollect = false;
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
                OpenDoor();
            }
            else
            {
                int keysNeeded = Mathf.Max(0, _gameSession.needKeys - _gameSession.key);

                if (keysNeeded == 0)
                {
                    _exitTextInstance.text = $"You need {keysNeeded} keys";
                }
                else if (keysNeeded >= 1)
                {
                    _exitTextInstance.text = $"You need {keysNeeded} key";
                }
                else
                {
                    _exitTextInstance.text = $"Door open";
                }

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

    private void OpenDoor()
    {
        _doorAnimator.SetTrigger("Open");
        _gameSession.key = 0;

        _audioSource.PlayOneShot(_doorOpenSFX);

        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        _stageManager.LoadScene(_nameNextStage);
        _stageManager.currentLevel = _nextCurrentLevel;

        _gameSession.SetCurrentLevel(_nextCurrentLevel);
    }
}
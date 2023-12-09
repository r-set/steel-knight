using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _gameAudio;
    [SerializeField] private AudioClip _menuAudio;

    [Header("Menu Sound")]
    [SerializeField] private string[] _menuScenes = { "MainMenu", "Stages" };

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (IsMenuScene(sceneName))
        {
            PlayMenuAudio();
        }
        else
        {
            PlayGameAudio();
        }
    }

    private bool IsMenuScene(string sceneName)
    {
        foreach (var menuScene in _menuScenes)
        {
            if (sceneName.Equals(menuScene))
            {
                return true;
            }
        }
        return false;
    }

    private void PlayGameAudio()
    {
        if (_audioSource != null && _gameAudio != null && !_audioSource.isPlaying)
        {
            _audioSource.clip = _gameAudio;
            _audioSource.Play();
        }
    }

    private void PlayMenuAudio()
    {
        if (_audioSource != null && _menuAudio != null && !_audioSource.isPlaying)
        {
            _audioSource.clip = _menuAudio;
            _audioSource.Play();
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _gameAudio;
    [SerializeField] private AudioClip _menuAudio;

    [Header("Menu Sound")]
    [SerializeField] private SceneName[] _menuScenes = { SceneName.MainMenu, SceneName.Stages};

    private Camera _camera;
    private AudioSource _audioSource;

    public enum SceneName
    {
        MainMenu,
        Stages,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
        Stage10
    }

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        SceneName loadedSceneEnum;
        if (System.Enum.TryParse(sceneName, out loadedSceneEnum))
        {
            if (IsMenuScene(loadedSceneEnum))
            {
                PlayMenuAudio();
            }
            else
            {
                PlayGameAudio();
            }
        }
        else
        {
            Debug.LogWarning("Scene not found in enum");
        }
    }

    private bool IsMenuScene(SceneName scene)
    {
        foreach (var menuScene in _menuScenes)
        {
            if (scene == menuScene)
            {
                return true;
            }
        }
        return false;
    }

    public void PlayGameAudio()
    {
        if (_audioSource != null && _gameAudio != null)
        {
            _audioSource.clip = _gameAudio;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or Game Audio Clip is missing");
        }
    }

    public void PlayMenuAudio()
    {
        if (_audioSource != null && _menuAudio != null)
        {
            _audioSource.clip = _menuAudio;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or Menu Audio Clip is missing!");
        }
    }
}
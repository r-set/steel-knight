using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _gameAudio;
    [SerializeField] private AudioClip _menuAudio;

    [Header("Menu Sound")]
    [SerializeField] private string[] _menuScenes = { "MainMenu", "Stages", "Control" };

    [Header("AudioToggle")]
    [SerializeField] private Toggle _soundToggle;
    private bool isSoundOff = false;
    private string soundKey = "SoundState";

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
         if (PlayerPrefs.HasKey(soundKey))
        {
            isSoundOff = PlayerPrefs.GetInt(soundKey) == 1;
        }

        _soundToggle.onValueChanged.AddListener(ToggleSound);
        _soundToggle.isOn = isSoundOff;
        SetAudioVolume(isSoundOff);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!isSoundOff)
        {
            SetAudioVolume(!hasFocus);
        }
    }

    void OnApplicationPause(bool isPaused)
    {
        if (!isSoundOff)
        {
            SetAudioVolume(isPaused);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ToggleSound(bool isSoundDisabled)
    {
        isSoundOff = isSoundDisabled;

        PlayerPrefs.SetInt(soundKey, isSoundOff ? 1 : 0);
        PlayerPrefs.Save();

        SetAudioVolume(isSoundOff);
    }

    private void SetAudioVolume(bool isMuted)
    {
        AudioListener.pause = isMuted;
        AudioListener.volume = isMuted ? 0.0f : 0.5f;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (IsMenuScene(sceneName))
        {
            _audioSource.clip = _menuAudio;

            PlayAudio();
        }
        else
        {
            _audioSource.clip = _gameAudio;

            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }
    }
}
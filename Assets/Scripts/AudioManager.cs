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
        AudioListener.volume = isMuted ? 0.0f : 1.0f;
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
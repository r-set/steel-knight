using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _buttonSFX;

    private Camera _camera;
    private AudioSource _audioSource;

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }

    private void PlayButtonSFX()
    {
        _audioSource.PlayOneShot(_buttonSFX);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        LoadSceneWithButtonSFX("MainMenu");
    }

    public void LoadStages()
    {
        LoadSceneWithButtonSFX("Stages");
    }

    public void LoadStage(int level)
    {
        if (level >= 1 && level <= 10)
        {
            string sceneName = "Stage" + level;
            LoadSceneWithButtonSFX(sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        PlayButtonSFX();
    }

    private void LoadSceneWithButtonSFX(string sceneName)
    {
        LoadScene(sceneName);
        PlayButtonSFX();
    }
}
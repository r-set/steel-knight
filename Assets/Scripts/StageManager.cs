using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _stage1Button;
    [SerializeField] private Button _stage2Button;
    [SerializeField] private Button _stage3Button;
    [SerializeField] private Button _stage4Button;
    [SerializeField] private Button _stage5Button;
    [SerializeField] private Button _stage6Button;
    [SerializeField] private Button _stage7Button;
    [SerializeField] private Button _stage8Button;
    [SerializeField] private Button _stage9Button;
    [SerializeField] private Button _stage10Button;

    [Header("Audio")]
    [SerializeField] private AudioClip _buttonSFX;

    private Camera _camera;
    private AudioSource _audioSource;
    [HideInInspector] public int currentLevel = 1;

    [Header("Scripts")]
    [SerializeField] private GameSession _gameSession;

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = _camera.GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentLevel = _gameSession.GetCurrentLevel();

        for (int i = 1; i <= 10; i++)
        {
            Button stageButton = GetStageButton(i);
            if (stageButton != null)
            {
                stageButton.interactable = false;
            }
        }

        for (int i = 1; i <= currentLevel; i++)
        {
            Button stageButton = GetStageButton(i);
            if (stageButton != null)
            {
                stageButton.interactable = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }

    private Button GetStageButton(int level)
    {
        switch (level)
        {
            case 1:
                return _stage1Button;
            case 2:
                return _stage2Button;
            case 3:
                return _stage3Button;
            case 4:
                return _stage4Button;
            case 5:
                return _stage5Button;
            case 6:
                return _stage6Button;
            case 7:
                return _stage7Button;
            case 8:
                return _stage8Button;
            case 9:
                return _stage9Button;
            case 10:
                return _stage10Button;

            default:
                return null;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        LoadSceneWithButtonSFX("MainMenu");
    }

    public void LoadControl()
    {
        LoadSceneWithButtonSFX("Control");
    }

    public void LoadStages()
    {
        LoadSceneWithButtonSFX("Stages");
    }

    public void LoadStage(int level)
    {
        GameSession.playerLives = 3;

        if (level >= 1 && level <= 10)
        {
            string sceneName = "Stage" + level;
            LoadSceneWithButtonSFX(sceneName);
        }
        else
        {
            LoadSceneWithButtonSFX("MainMenu");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        PlayButtonSFX();
        _gameSession.SetCurrentLevel(1);
    }

    private void LoadSceneWithButtonSFX(string sceneName)
    {
        LoadScene(sceneName);
        PlayButtonSFX();
    }

    private void PlayButtonSFX()
    {
        _audioSource.PlayOneShot(_buttonSFX);
    }

}
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [Header("Stat")]
    public static int playerLives = 3;
    private int _initialPlayerLives;

    [Header("Key")]
    public int key = 0;
    public int needKeys = 2;
    private int _initialKey;

    [Header("Text")]
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _keyText;

    [Header("Sound")]
    [SerializeField] private Toggle _soundToggle;
    private bool _isSoundOff = false;
    private string _soundKey = "SoundState";

    private void Start()
    {
        _initialPlayerLives = playerLives;
        _initialKey = key;

        UpdateUIText();

        if (PlayerPrefs.HasKey(_soundKey))
        {
            _isSoundOff = PlayerPrefs.GetInt(_soundKey) == 1;
        }

        _soundToggle.onValueChanged.AddListener(ToggleSound);
        _soundToggle.isOn = _isSoundOff;
        SetAudioVolume(_isSoundOff);
    }

    private void UpdateUIText()
    {
        _livesText.text = playerLives.ToString();
        _keyText.text = key.ToString() + "/" + needKeys.ToString();
    }

    private void ToggleSound(bool isSoundDisabled)
    {
        _isSoundOff = isSoundDisabled;

        PlayerPrefs.SetInt(_soundKey, _isSoundOff ? 1 : 0);
        PlayerPrefs.Save();

        SetAudioVolume(_isSoundOff);
    }

    private void SetAudioVolume(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0.0f : 1.0f;
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToKey(int keyToAdd)
    {
        key += keyToAdd;
        UpdateUIText();
    }

    private void TakeLife()
    {
        playerLives--;
        key = 0;
        UpdateUIText();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGameSession()
    {
        playerLives = _initialPlayerLives;
        key = _initialKey;

        SceneManager.LoadScene(0);
    }

    public void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.Save();
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel", 0);
    }
}
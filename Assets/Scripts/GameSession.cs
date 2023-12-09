using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private int _playerLives = 5;

    [Header("Key")]
    public int key = 0;
    public int needKeys = 2;
    private int _initialPlayerLives;
    private int _initialKey;

    [Header("Text")]
    [SerializeField] private TMP_Text _livesText;
    [SerializeField] private TMP_Text _keyText;

    [Header("Sound")]
    [SerializeField] private Toggle _soundToggle;
    private bool isSoundOff = false;
    private string soundKey = "SoundState";

    private void Start()
    {
        _initialPlayerLives = _playerLives;
        _initialKey = key;

        UpdateUIText();

        if (PlayerPrefs.HasKey(soundKey))
        {
            isSoundOff = PlayerPrefs.GetInt(soundKey) == 1;
        }

        _soundToggle.onValueChanged.AddListener(ToggleSound);
        _soundToggle.isOn = isSoundOff;
        SetAudioVolume(isSoundOff);
    }

    private void UpdateUIText()
    {
        _livesText.text = _playerLives.ToString();
        _keyText.text = key.ToString() + "/" + needKeys.ToString();
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

    public void ProcessPlayerDeath()
    {
        if (_playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int keyToAdd)
    {
        key += keyToAdd;
        UpdateUIText();
    }

    private void TakeLife()
    {
        _playerLives--;
        key = 0;
        UpdateUIText();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGameSession()
    {
        _playerLives = _initialPlayerLives;
        key = _initialKey;

        SceneManager.LoadScene(0);
    }
}
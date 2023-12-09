using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
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

    private void Start()
    {
        _initialPlayerLives = _playerLives;
        _initialKey = key;

        UpdateUIText();

        _soundToggle.onValueChanged.AddListener(ToggleSound);
        _soundToggle.isOn = isSoundOff;
    }

    private void UpdateUIText()
    {
        _livesText.text = _playerLives.ToString();
        _keyText.text = key.ToString() + "/" + needKeys.ToString();
    }

    private void ToggleSound(bool isSoundDisabled)
    {
        isSoundOff = isSoundDisabled;

        if (isSoundOff)
        {
            AudioListener.volume = 0.0f;
        }
        else
        {
            AudioListener.volume = 1.0f;
        }
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
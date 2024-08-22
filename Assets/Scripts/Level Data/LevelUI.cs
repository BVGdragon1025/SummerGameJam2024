using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; // Referencja do Text dla zdrowia
    public TextMeshProUGUI moneyText; // Referencja do Text dla pieniêdzy
    public TextMeshProUGUI plagueText; // Referencja do Text dla plagi

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (_gameManager != null)
        {
            healthText.SetText($"{_gameManager.CurrentPlayerPlague}");
            moneyText.SetText($"{_gameManager.CurrentCurrency}");
            plagueText.SetText($"{_gameManager.CurrentLvlPlague}");
        }
    }
}
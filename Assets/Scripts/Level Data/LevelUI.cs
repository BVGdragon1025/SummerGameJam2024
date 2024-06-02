using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public LevelData levelData; // Referencja do ScriptableObject
    public TextMeshProUGUI healthText; // Referencja do Text dla zdrowia
    public TextMeshProUGUI moneyText; // Referencja do Text dla pieniêdzy
    public TextMeshProUGUI plagueText; // Referencja do Text dla plagi

    void Update()
    {
        if (levelData != null)
        {
            healthText.text = $"{levelData.playerPlagueValue}";
            moneyText.text = $"{levelData.currency}";
            plagueText.text = $"{levelData.lvlPlagueValue}";
        }
    }
}
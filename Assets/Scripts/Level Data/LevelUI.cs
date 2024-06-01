using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public LevelData levelData; // Referencja do ScriptableObject
    public TextMeshProUGUI healthText; // Referencja do Text dla zdrowia
    public TextMeshProUGUI moneyText; // Referencja do Text dla pieni�dzy
    public TextMeshProUGUI plagueText; // Referencja do Text dla plagi

    void Update()
    {
        if (levelData != null)
        {
            healthText.text = "Health: " + levelData.playerPlagueValue;
            moneyText.text = "Money: " + levelData.currency;
            plagueText.text = "Plague: " + levelData.lvlPlagueValue;
        }
    }
}
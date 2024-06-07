using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;

    public TextMeshProUGUI name;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI gives;
    public TextMeshProUGUI time;
    public TextMeshProUGUI givesUpgrade;
    public TextMeshProUGUI addInfo;

    public Image givesImage;

    public Sprite spriteMoney;
    public Sprite spritePlayerPlague;
    public Sprite spriteForestPlague;

    public GameObject basic;

    private void Awake()


    {
        if (Instance != null && Instance != this )
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAndShowToolTip(Building building)
    {
        gameObject.SetActive(true);
        switch (building.BuildingType)
        {
            case BuildingType.ResourceUpgrade:
                name.text = $"{building.BuildingName}";
                cost.text = $"{building.BuildingCost}";
                basic.SetActive(false);
                givesUpgrade.text = $"+{building.Currency} Resource Production, to all structures around";
                break;
            case BuildingType.SpeedUpgrade:
                name.text = $"{building.BuildingName}";
                cost.text = $"{building.BuildingCost}";
                basic.SetActive(false);
                givesUpgrade.text = $"{building.Currency * 100}% to Resource Speed, to all structures around";
                break;
            case BuildingType.HealthUpgrade:
                name.text = $"{building.BuildingName}";
                cost.text = $"{building.BuildingCost}";
                basic.SetActive(false);
                givesUpgrade.text = $"{building.Currency}s to Plague Timer, to all structures around";
                break;
            case BuildingType.Tree:
                name.text = $"{building.BuildingName}";
                cost.text = $"{building.BuildingCost}";
                gives.text = $"+{building.Currency}";
                time.text = $"/{building.SpawnRate}s";
                addInfo.text = $"Requires {building.BuildingType}";
                givesImage.sprite = spriteMoney;
                givesUpgrade.text = string.Empty;
                basic.SetActive(true);
                break;
            case BuildingType.Meadow:
                name.text = $"{building.BuildingName}";
                cost.text = $"{building.BuildingCost}";
                gives.text = $"-{building.Currency}";
                time.text = $"/{building.SpawnRate}s";
                addInfo.text = $"Requires {building.BuildingType}";
                givesImage.sprite = spritePlayerPlague;
                givesUpgrade.text = string.Empty;
                basic.SetActive(true);
                break;
            case BuildingType.Pond:
                name.text = $"{building.BuildingName}";
                cost.text = $"{building.BuildingCost}";
                gives.text = $"-{building.Currency}";
                time.text = $"/{building.SpawnRate}s";
                addInfo.text = $"Requires {building.BuildingType}";
                givesImage.sprite = spriteForestPlague;
                givesUpgrade.text = string.Empty;
                basic.SetActive(true);
                break;
            default:
                name.text = $"Holy Shit, is that an Easter Egg?";
                break;
        }

    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }

}

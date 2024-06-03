using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;

    public TextMeshProUGUI textComponent;

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
                textComponent.text = $" Name: {building.BuildingName}, \n Cost: {building.BuildingCost}, \n Gives: +{building.Currency} Resource Production, to all structures around.";
                break;
            case BuildingType.SpeedUpgrade:
                textComponent.text = $" Name: {building.BuildingName}, \n Cost: {building.BuildingCost}, \n Gives: {building.Currency * 100}% to Resource Speed, to all structures around.";
                break;
            case BuildingType.HealthUpgrade:
                textComponent.text = $" Name: {building.BuildingName}, \n Cost: {building.BuildingCost}, \n Gives: {building.Currency}s to Plague Timer, to all structures around.";
                break;
            case BuildingType.Tree:
                textComponent.text = $" Name: {building.BuildingName}, \n Cost: {building.BuildingCost}, \n Gives: {building.Currency} Nature Points / {building.SpawnRate}s, \n Requires element: {building.BuildingType}";
                break;
            case BuildingType.Meadow:
                textComponent.text = $" Name: {building.BuildingName}, \n Cost: {building.BuildingCost}, \n Gives: -{building.Currency} Player Plague / {building.SpawnRate}s, \n Requires element: {building.BuildingType}";
                break;
            case BuildingType.Pond:
                textComponent.text = $" Name: {building.BuildingName}, \n Cost: {building.BuildingCost}, \n Gives: -{building.Currency} Forest Plague / {building.SpawnRate}s, \n Requires element: {building.BuildingType}";
                break;
            default:
                textComponent.text = $"Holy Shit, is that an Easter Egg?";
                break;
        }

    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }
}

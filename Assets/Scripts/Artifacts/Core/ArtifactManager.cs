using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArtifactManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _forestArtifactDescription;
    [SerializeField] private ArtifactSO _artifactSO;
    [SerializeField] private List<Button> _artifactButtons;
    [SerializeField] private Button _continueButton;
    [SerializeField] private MainMenu _mainMenu;

    private void OnEnable()
    {
        _mainMenu = GetComponent<MainMenu>();
        GetRandomForestArtifact();
        GetRandomPlayerArtifacts();
        _continueButton.gameObject.SetActive(false);
    }

    private void GetRandomForestArtifact()
    {
        int random = Random.Range(0, _artifactSO.forestArtifacts.Count);
        Artifact chosenArtifact = _artifactSO.forestArtifacts[random];
        _artifactSO.forestArtifacts.Remove(chosenArtifact);
        _forestArtifactDescription.text = $"The Plague is getting stronger. To make your quest harder, you're cursed with {chosenArtifact.ArtifactName}. {chosenArtifact.ArtifactDescription}";
        chosenArtifact.UseArtifact();
    }

    private void GetRandomPlayerArtifacts()
    {
        List<Artifact> artfactList = new(_artifactSO.playerArtifacts);

        for(int i = 0; i < _artifactButtons.Count; i++)
        {
            int random = Random.Range(0, _artifactSO.playerArtifacts.Count);
            Artifact artifact = _artifactSO.playerArtifacts[random];
            var texts = _artifactButtons[i].gameObject.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
            texts[0].text = artifact.ArtifactName;
            texts[1].text = artifact.ArtifactDescription;
            _artifactButtons[i].onClick.RemoveAllListeners();
            _artifactButtons[i].onClick.AddListener(delegate { SetContinueButton(artifact); });
            artfactList.Remove(artifact);

        }
    }

    private void SetContinueButton(Artifact artifact)
    {
        if (!_continueButton.gameObject.activeSelf)
        {
            _continueButton.gameObject.SetActive(true);

        }

        _continueButton.onClick.RemoveAllListeners();
        _continueButton.onClick.AddListener(artifact.UseArtifact);
        _continueButton.onClick.AddListener(delegate { RemoveArtifactFromList(artifact); });
        _continueButton.onClick.AddListener(_mainMenu.Play);
    }

    private void RemoveArtifactFromList(Artifact artifact)
    {
        _artifactSO.playerArtifacts.Remove(artifact);
    }

}

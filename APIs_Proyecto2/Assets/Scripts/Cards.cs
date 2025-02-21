using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI speciesText;
    public TextMeshProUGUI originText;
    public RawImage characterImage;

    public void UpdateCard(API_Manager.Character character, Texture2D texture)
    {
        nameText.text = character.name;
        statusText.text = character.status;
        speciesText.text = character.species;
        originText.text = character.origin.name;
        characterImage.texture = texture;
    }
}

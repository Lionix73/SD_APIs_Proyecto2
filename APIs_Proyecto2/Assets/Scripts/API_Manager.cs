using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class API_Manager : MonoBehaviour
{
    [SerializeField]
    private string url = "https://rickandmortyapi.com/api/character";
    
    [SerializeField]
    private string myurl = "https://my-json-server.typicode.com/lionix73/sd_apis_proyecto2/";

    [SerializeField]
    private Card[] cards;

    [SerializeField]
    private InputField idInput;

    [SerializeField]
    private Dropdown dropdown;

    private string selectedUrl;

    void Start()
    {
        HandleDropdownValueChanged(dropdown.value);

        dropdown.onValueChanged.AddListener(HandleDropdownValueChanged);
    }

    private void HandleDropdownValueChanged(int value)
    {
        switch (value)
        {
            case 0:
                selectedUrl = url;
                break;
            case 1:
                selectedUrl = myurl;
                break;
            default:
                selectedUrl = url;
                break;
        }
    }

    public void SendRequest(int cardIndex)
    {
        int id;
        if (int.TryParse(idInput.text, out id))
        {
            if (selectedUrl == myurl && (id < 1 || id > 20))
            {
                Debug.Log("ID out of range for myurl. Please enter an ID between 1 and 20.");
            }
            else
            {
                StartCoroutine(GetCharacter(id, cardIndex));
            }
        }
    }

    IEnumerator GetCharacter(int id, int cardIndex)
    {
        UnityWebRequest www = UnityWebRequest.Get(selectedUrl + "/" + id);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
                Character character = JsonUtility.FromJson<Character>(www.downloadHandler.text);
                StartCoroutine(GetImage(character.image, character, cardIndex));
            }
            else
            {
                string mensaje = "status:" + www.responseCode;
                mensaje += "\nErro: " + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    IEnumerator GetImage(string imageUrl, Character character, int cardIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            UpdateCard(character, texture, cardIndex);
        }
    }

    private void UpdateCard(Character character, Texture2D texture, int cardIndex)
    {
        if (cardIndex >= 0 && cardIndex < cards.Length)
        {
            cards[cardIndex].UpdateCard(character, texture);
        }
    }

    [System.Serializable]
    public class Character
    {
        public int id;
        public string name;
        public string status;
        public string species;
        public string image;
        public Origin origin;
    }

    [System.Serializable]
    public class Origin
    {
        public string name;
        public string url;
    }

    [System.Serializable]
    public class CharactersList
    {
        public Character[] results;
    }
}
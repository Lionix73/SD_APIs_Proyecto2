using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestAPI : MonoBehaviour
{
    [SerializeField]
    private string url = "https://rickandmortyapi.com/api/character";
    [SerializeField]
    private string myurl = "";

    private RawImage picture;

    public void SendRequest()
    {
        StartCoroutine(GetCharacter(1));
    }

    IEnumerator GetCharacter(int id)
    {
        UnityWebRequest www = UnityWebRequest.Get(url+"/"+id);
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

                StartCoroutine(GetImage(character.image));
                
                
            }
            else
            {
                string mensaje = "status:" + www.responseCode;
                mensaje += "\nErro: " + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    IEnumerator GetImage(string imageUrl)
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
            picture.texture = texture;
        }
    }

    [System.Serializable]
    public class Character{
        public int id;
        public string name;
        public string species;
        public string image;
    }

    [System.Serializable]
    public class CharactersList{
        public Character[] results;
    }
}

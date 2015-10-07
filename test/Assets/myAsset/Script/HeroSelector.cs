using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using HashTable = ExitGames.Client.Photon.Hashtable;

public enum HeroCharacter
{
    ninja = 1,
    samurai,
}

public class HeroSelector : MonoBehaviour {

    MyRoomInfo info;

    public GameObject iconPrefab;
    public Sprite[] heros;

    PhotonView myPhotonView;

    void Awake()
    {

        myPhotonView = GameObject.Find("Client").GetComponent<PhotonView>();

    }

	// Use this for initialization
	void Start () {

        info = GameObject.Find("RoomInfo").GetComponent<MyRoomInfo>();
        SetIcon(HeroCharacter.ninja);//default

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SetIcon(HeroCharacter hero)
    {

        Debug.Log("set icon");
        HashTable hHash;
        switch(hero){
            case HeroCharacter.ninja:
                hHash = new HashTable() { { "HS", HeroCharacter.ninja} };
                PhotonNetwork.player.SetCustomProperties(hHash);
                foreach (Image i in gameObject.GetComponentsInChildren<Image>())
                {
                    i.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                }
                transform.FindChild("ninja").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    
                break;
            case HeroCharacter.samurai:
                hHash = new HashTable() { { "HS", HeroCharacter.samurai} };
                PhotonNetwork.player.SetCustomProperties(hHash);
                foreach (Image i in gameObject.GetComponentsInChildren<Image>())
                {
                    i.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                }
                transform.FindChild("samurai").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    
                break;
        }

        myPhotonView.RPC("RoomUpdate", PhotonTargets.All);

    }

    public void SetIcon(int i)
    {
        SetIcon((HeroCharacter)i);
    }

    public Sprite GetIcon(HeroCharacter hero)
    {
        return heros[(int)hero - 1];
    }

    public Sprite GetIcon(int i)
    {
        return GetIcon((HeroCharacter)i);
    }

    public string GetName(HeroCharacter hero)
    {
        switch(hero){
            case HeroCharacter.ninja:
                return "ninja";
            case HeroCharacter.samurai:
                return "samurai";

        }
        return "";
    }

    public string GetName(int i)
    {
        return GetName((HeroCharacter)i);
    }


}

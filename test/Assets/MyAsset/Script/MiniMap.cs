using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour {

    public RectTransform mapRect;

    float TerrainWidth = 180;//x
    float TerrainLength = 80;//z

    [SerializeField]
    GameObject redHeroClone;
    [SerializeField]
    GameObject blueHeroClone;

    List<Transform> charaObj;
    List<RectTransform> charaPos;
    
    

    void Awake()
    {

        charaObj = new List<Transform>();
        charaPos = new List<RectTransform>();

    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        for(int i = 0;i < charaObj.Count;i++){

            SetPosMinimap(charaObj[i], charaPos[i]);

        }

	}

    public void AddCharaOnMiniMap(Transform t)
    {

        switch (t.gameObject.GetComponent<HeroState>().team)
        {
            case TEAM.BLUE:
                AddBlueCharaOnMiniMap(t);

                break;
            case TEAM.RED:
                AddRedCharaOnMinimap(t);

                break;
        }

    }


    void AddBlueCharaOnMiniMap(Transform t)
    {
        charaObj.Add(t);
        RectTransform c = Instantiate(blueHeroClone).GetComponent<RectTransform>();
        c.parent = transform;

        SetPosMinimap(t, c);

        charaPos.Add(c);
        
        
        
    }

    void AddRedCharaOnMinimap(Transform t)
    {
        charaObj.Add(t);
        RectTransform c = Instantiate(redHeroClone).GetComponent<RectTransform>();
        c.parent = transform;
        

        SetPosMinimap(t, c);

        charaPos.Add(c);

    }


    void SetPosMinimap(Transform t, RectTransform rt)
    {

        float xRate = t.position.x / TerrainWidth;
        float yRate = t.position.z / TerrainLength;

        rt.localPosition = new Vector2(xRate * mapRect.rect.width, yRate * mapRect.rect.height) - new Vector2(mapRect.rect.width / 2 - 15, mapRect.rect.height / 2);

    }

}

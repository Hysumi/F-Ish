using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

    #region Fishing Spot Variables

    public const int maxFishSpots = 10;
    public GameObject fishSpot;
    GameObject[] fishSpotList = new GameObject[maxFishSpots];
    public int spotControl = 0;

    #endregion

    #region Fish Variables

    public float forceRange;
    public float chanceRange;

    FishStatus[] listaPeixes = new FishStatus[5];
    
    #endregion

    void Start ()
    {
        FillFishList();
        FillFishSpotList();
    }

    // Update is called once per frame
    void Update ()
    {
        int K = Random.Range(0, 9);
        Debug.Log(spotControl);
		if(spotControl < 4)
        {
            Debug.Log("Entrou");
            FillFishSpotList();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            Destroy(fishSpotList[K]);
            spotControl--;
        }
	}


    void FillFishSpotList()
    {
        //X: entre -4 e 4;
        //Y: entre -15 e 15;
        for (int i = 0; i < fishSpotList.Length; i++)
        {
            if (!fishSpotList[i])
            {
                Debug.Log("q");
                fishSpotList[i] = Instantiate(fishSpot, new Vector3(Random.Range(-4f, 4f), Random.Range(-15f, 15f), 1), new Quaternion());
                fishSpotList[i].GetComponent<FishingSpot>().listaPeixes = listaPeixes;
                fishSpotList[i].gameObject.name = "SpotFish " + i;

                for (int j = 0; j < listaPeixes.Length; j++)
                {
                    listaPeixes[j].ForceRange(forceRange);
                    listaPeixes[j].ChanceRange(chanceRange);
                }
            }
        }
        spotControl = 10;
    }

    void FillFishList()
    {
        listaPeixes[0].name = "Corvina";
        listaPeixes[0].force = 10;
        listaPeixes[0].chanceAppear = 0.7f;
        listaPeixes[0].ForceRange(forceRange);
        listaPeixes[0].ChanceRange(chanceRange);

        listaPeixes[1].name = "Jacundá";
        listaPeixes[1].force = 20;
        listaPeixes[1].chanceAppear = 0.4f;
        listaPeixes[1].ForceRange(forceRange);
        listaPeixes[1].ChanceRange(chanceRange);

        listaPeixes[2].name = "Pacu";
        listaPeixes[2].force = 60;
        listaPeixes[2].chanceAppear = 0.25f;
        listaPeixes[2].ForceRange(forceRange);
        listaPeixes[2].ChanceRange(chanceRange);

        listaPeixes[3].name = "Dourado";
        listaPeixes[3].force = 80;
        listaPeixes[3].chanceAppear = 0.1f;
        listaPeixes[3].ForceRange(forceRange);
        listaPeixes[3].ChanceRange(chanceRange);

        listaPeixes[4].name = "Piraiba";
        listaPeixes[4].force = 100;
        listaPeixes[4].chanceAppear = 0.05f;
        listaPeixes[4].ForceRange(forceRange);
        listaPeixes[4].ChanceRange(chanceRange);
    }
}

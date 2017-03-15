using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour {

    public float dropRange;
    public float forceRange;

    FishStatus[] listaPeixes = new FishStatus[5];

    // Use this for initialization
    void Start ()
    {
        FillFishList();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void FillFishList()
    {
        //Esses valores tem que ser mudados depois... To colocando em ordem de nivel crescente só pra teste
        listaPeixes[0].name = "Corvina";
        listaPeixes[0].resistence = 20;
        listaPeixes[0].force = 10;
        listaPeixes[0].depth = 1;
        listaPeixes[0].chanceAppear = 0.7f;
        listaPeixes[0].resistenceRange(dropRange);
        listaPeixes[0].forceRange(forceRange);

        listaPeixes[1].name = "Jacundá";
        listaPeixes[1].resistence = 40;
        listaPeixes[1].force = 20;
        listaPeixes[1].depth = 1;
        listaPeixes[1].chanceAppear = 0.4f;
        listaPeixes[1].resistenceRange(dropRange);
        listaPeixes[1].forceRange(forceRange);

        listaPeixes[2].name = "Pacu";
        listaPeixes[2].resistence = 80;
        listaPeixes[2].force = 60;
        listaPeixes[2].depth = 2;
        listaPeixes[2].chanceAppear = 0.25f;
        listaPeixes[2].resistenceRange(dropRange);
        listaPeixes[2].forceRange(forceRange);

        listaPeixes[3].name = "Dourado";
        listaPeixes[3].resistence = 130;
        listaPeixes[3].force = 80;
        listaPeixes[3].depth = 2;
        listaPeixes[3].chanceAppear = 0.1f;
        listaPeixes[3].resistenceRange(dropRange);
        listaPeixes[3].forceRange(forceRange);

        listaPeixes[4].name = "Piraiba";
        listaPeixes[4].resistence = 150;
        listaPeixes[4].force = 100;
        listaPeixes[4].depth = 3;
        listaPeixes[4].chanceAppear = 0.05f;
        listaPeixes[4].resistenceRange(dropRange);
        listaPeixes[4].forceRange(forceRange);
    }

}

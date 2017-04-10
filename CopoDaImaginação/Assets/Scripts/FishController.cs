using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class FishController : MonoBehaviour {

    #region Fishing Spot Variables

    public const int maxFishSpots = 10;
    public GameObject fishSpot;
    GameObject[] fishSpotList = new GameObject[maxFishSpots];
    int spotControl = 0;
    bool fullSpots = false;
    public float trashChanceAppear = 0.9f;

    #endregion

    RiverNumberFishAzure getFromAzure;

    //TIMER OP
    public int min, sec = 10;
    public string sMin, sSec;
    public bool isRunning;
    public bool isPlay;
    //Rect boatArea;
    public Vector2 boatSummonRange;
    Bounds boatMaxCameraArea;

    #region Fish Variables

    public float forceRange;
    public float chanceRange;

    FishStatus[] listaPeixes = new FishStatus[7];

    #endregion

    #region Temporizador

    public float SummonTime;
    float actualTime;

    #endregion

    void Start ()
    {
        getFromAzure = GetComponent<RiverNumberFishAzure>();
        getFromAzure.GetAllFishes();

        //FillFishList();
        boatMaxCameraArea.center = Camera.main.transform.position;
        boatMaxCameraArea.size = new Vector2(boatSummonRange.x, boatSummonRange.y);

        //GAMBIARRA 2
        for (int i = 0; i < fishSpotList.Length; i++)
        {
            if (!fishSpotList[i])
            {
                fishSpotList[i] = Instantiate(fishSpot, new Vector3(Random.Range(-4f, 4f), Random.Range(-15f, 15f), 0f), new Quaternion());
                fishSpotList[i].GetComponent<FishingSpot>().listaPeixes = listaPeixes;
                fishSpotList[i].gameObject.name = i + " SpotFish";
                spotControl++;
            }
        }

        HUD.pauseEvent += PauseTime;
        PauseMenu.unPauseEvent += DepauseTime;
    }

    // Update is called once per frame
    void Update ()
    {
        if (getFromAzure.isReadCompleted==true)
        {
            //FAZER LOAD AQUI
            isPlay = true;
            StartTimer();
            FillFishList();
            getFromAzure.isReadCompleted = false;
            
        } 

        //ESTATICO 3
        if (actualTime >= SummonTime)
            FillFishSpotList();
        Temporizador();
        boatMaxCameraArea.center = Camera.main.transform.position;
    }

    public void PauseTime()
    {
        isRunning = false;
    }

    public void DepauseTime()
    {
        isRunning = true;
    }

    public void CleanTrash(FishStatus fs)
    {
        trashChanceAppear = fs.CleanTrash();
    }
    public void AddTrash(FishStatus fs)
    {
        trashChanceAppear = fs.AddTrash();
    }

    void Temporizador()
    {
        if (SummonTime < actualTime)
            actualTime = 0;
        else
            actualTime += Time.deltaTime;
    }

    void FillFishSpotList()
    {
        //ESTATICO 2
        //X: entre -4 e 4;
        //Y: entre -15 e 15;
        for (int i = 0; i < fishSpotList.Length; i++)
        {
            if (!fishSpotList[i])
            {
                //GAMBIARRA 4
                Vector3 point = new Vector3(Random.Range(-4f, 4f), Random.Range(-15f, 15f), 0);
                Ray ray = new Ray(point, -Vector3.forward);

                if (boatMaxCameraArea.IntersectRay(ray))
                {
                    if (Random.Range(0, 10) < 6) //60%
                    {
                        //GAMBIARRA3
                        point += new Vector3(0, 0, 0f);
                        fishSpotList[i] = Instantiate(fishSpot, point, new Quaternion());
                        fishSpotList[i].GetComponent<FishingSpot>().listaPeixes = listaPeixes;
                        fishSpotList[i].gameObject.name = i + " SpotFish";
                        spotControl++;
                    }
                }
                else
                {
                    point += new Vector3(0, 0, 0f);
                    fishSpotList[i] = Instantiate(fishSpot, point, new Quaternion());
                    fishSpotList[i].GetComponent<FishingSpot>().listaPeixes = listaPeixes;
                    fishSpotList[i].gameObject.name = i + " SpotFish";
                    spotControl++;
                }
                break;
            }
            
        }
    }

    void FillFishList()
    {
        foreach (RiverNumberFish fish in getFromAzure._allFishInDB)
        {
            switch (fish.fishName)
            {
                case "Lixo":
                    listaPeixes[0].name = "Lixo";
                    listaPeixes[0].force = Random.Range(0, 10);
                    listaPeixes[0].chanceAppear = fish.nFish * trashChanceAppear;
                    listaPeixes[0].inventoryWeight = 1;
                    listaPeixes[0].trashItem = Resources.Load("Itens/Lixos/Garrafa") as Lixo;
                    break;
                case "Lambari":
                    listaPeixes[1].name = "Lambari";
                    listaPeixes[1].force = 10;
                    listaPeixes[1].chanceAppear = 0.7f * fish.nFish;
                    listaPeixes[1].isDay = true;
                    listaPeixes[1].hookType = new int[2];
                    listaPeixes[1].hookType[0] = 0;
                    listaPeixes[1].hookType[1] = 1;
                    listaPeixes[1].ambient = 0;
                    listaPeixes[1].inventoryWeight = 1;
                    listaPeixes[1].fishItem = Resources.Load("Itens/Peixes/Lambari") as Peixe;
                    break;
                case "Mandi":
                    listaPeixes[2].name = "Mandi";
                    listaPeixes[2].force = 20;
                    listaPeixes[2].chanceAppear = 0.6f * fish.nFish;
                    listaPeixes[2].isDay = false;
                    listaPeixes[2].hookType = new int[1];
                    listaPeixes[2].hookType[0] = 2;
                    listaPeixes[2].ambient = 0;
                    listaPeixes[2].inventoryWeight = 3;
                    listaPeixes[2].fishItem = Resources.Load("Itens/Peixes/Mandi") as Peixe;
                    break;
                case "Pacu":
                    listaPeixes[3].name = "Pacu";
                    listaPeixes[3].force = 60;
                    listaPeixes[3].chanceAppear = 0.5f*fish.nFish;
                    listaPeixes[3].isDay = true;
                    listaPeixes[3].hookType = new int[2];
                    listaPeixes[3].hookType[0] = 0;
                    listaPeixes[3].hookType[1] = 2;
                    listaPeixes[3].ambient = 1;
                    listaPeixes[3].inventoryWeight = 5;
                    listaPeixes[3].fishItem = Resources.Load("Itens/Peixes/Pacu") as Peixe;
                    break;
                case "Apaiari":
                    listaPeixes[4].name = "Apaiari";
                    listaPeixes[4].force = 80;
                    listaPeixes[4].chanceAppear = 0.45f * fish.nFish;
                    listaPeixes[4].isDay = true;
                    listaPeixes[4].hookType = new int[1];
                    listaPeixes[4].hookType[0] = 2;
                    listaPeixes[4].ambient = 1;
                    listaPeixes[4].inventoryWeight = 7;
                    listaPeixes[4].fishItem = Resources.Load("Itens/Peixes/Apaiari") as Peixe;
                    break;
                case "Piraputanga":
                    listaPeixes[5].name = "Piraputanga";
                    listaPeixes[5].force = 100;
                    listaPeixes[5].chanceAppear = 0.4f * fish.nFish;
                    listaPeixes[5].isDay = false;
                    listaPeixes[5].hookType = new int[1];
                    listaPeixes[5].hookType[0] = 1;
                    listaPeixes[5].ambient = 2;
                    listaPeixes[5].inventoryWeight = 7;
                    listaPeixes[5].fishItem = Resources.Load("Itens/Peixes/Piraputanga") as Peixe;
                    break;
                case "Cachara":
                    listaPeixes[6].name = "Piraputanga";
                    listaPeixes[6].force = 150;
                    listaPeixes[6].chanceAppear = 0.4f * fish.nFish;
                    listaPeixes[6].isDay = true;
                    listaPeixes[6].hookType = new int[1];
                    listaPeixes[6].hookType[0] = 1;
                    listaPeixes[6].ambient = 2;
                    listaPeixes[6].inventoryWeight = 10;
                    listaPeixes[6].fishItem = Resources.Load("Itens/Peixes/Cachara") as Peixe;
                    break;
            }
          
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        StartCoroutine(ItsTime());
    }

    public IEnumerator ItsTime()
    {
        while (isPlay)
        {
            yield return new WaitForSeconds(1);
            if (isRunning)
            {
                sec--;
                if (sec < 0)
                {
                    min--;
                    sec = 59;
                    if (min < 0)
                    {
                        isRunning = false;
                        min = 0;
                        sec = 0;
                        isPlay = false;
                    }
                }
            }
            sMin = "0" + min;
            if (sec >= 0 && sec <= 9)
                sSec = "0" + sec;
            else sSec = sec.ToString();

            GameObject.Find("Timer").GetComponent<Text>().text = "Time Left: " + sMin + ":" + sSec;
        }
    }
}

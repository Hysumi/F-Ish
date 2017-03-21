using UnityEngine;

public class FishController : MonoBehaviour {

    #region Fishing Spot Variables

    public const int maxFishSpots = 10;
    public GameObject fishSpot;
    GameObject[] fishSpotList = new GameObject[maxFishSpots];
    int spotControl = 0;
    bool fullSpots = false;

    #endregion

    //GAMBIARRA 3
    public GameObject MainCamera;
    //Rect boatArea;
    public Vector2 boatRange;
    Bounds boatArea;

    #region Fish Variables

    public float forceRange;
    public float chanceRange;

    FishStatus[] listaPeixes = new FishStatus[5];

    #endregion

    #region Temporizador

    public float SummonTime;
    float actualTime;

    #endregion

    void Start ()
    {
        FillFishList();
        boatArea.center = MainCamera.transform.position;
        boatArea.size = new Vector3(boatRange.x, boatRange.y, 0);

        //boatArea.position = MainCamera.transform.position;
        //boatArea.height = boatRange.y;
        //boatArea.width = boatRange.x;

        //GAMBIARRA 2
        for (int i = 0; i < fishSpotList.Length; i++)
        {
            if (!fishSpotList[i])
            {
                fishSpotList[i] = Instantiate(fishSpot, new Vector3(Random.Range(-4f, 4f), Random.Range(-15f, 15f), 0.1f), new Quaternion());
                fishSpotList[i].GetComponent<FishingSpot>().listaPeixes = listaPeixes;
                fishSpotList[i].gameObject.name = i + " SpotFish";
                spotControl++;
            }

        }
    }

    // Update is called once per frame
    void Update ()
    {
        //ESTATICO 3

        if (actualTime >= SummonTime)
            FillFishSpotList();
        Temporizador();
        boatArea.center = MainCamera.transform.position;

        int K = Random.Range(0, 10);
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(fishSpotList[K]);
            fishSpotList[K] = null;
            spotControl--;
        }
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
                bool teste = false;

                //GAMBIARRA 4
                Vector3 point = new Vector3(Random.Range(-4f, 4f), Random.Range(-15f, 15f), 0);
                Ray ray = new Ray(point, -Vector3.forward);

                Debug.Log(point);
                if (boatArea.IntersectRay(ray))
                {
                    Debug.Log("PARO PARO");
                    teste = true;

                    if (Random.Range(0, 10) < 6) //60%
                    {
                        //GAMBIARRA3
                        if (teste)
                            Debug.Log("OPA PASSOU EIN");
                        point += new Vector3(0, 0, 0.1f);
                        fishSpotList[i] = Instantiate(fishSpot, point, new Quaternion());
                        fishSpotList[i].GetComponent<FishingSpot>().listaPeixes = listaPeixes;
                        fishSpotList[i].gameObject.name = i + " SpotFish";
                        spotControl++;
                    }
                }
                else
                {
                    point += new Vector3(0, 0, 0.1f);
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
        //ESTATICO 1
        listaPeixes[0].name = "Corvina";
        listaPeixes[0].force = 10;
        listaPeixes[0].chanceAppear = 0.7f;

        listaPeixes[1].name = "Jacundá";
        listaPeixes[1].force = 20;
        listaPeixes[1].chanceAppear = 0.4f;

        listaPeixes[2].name = "Pacu";
        listaPeixes[2].force = 60;
        listaPeixes[2].chanceAppear = 0.25f;

        listaPeixes[3].name = "Dourado";
        listaPeixes[3].force = 80;
        listaPeixes[3].chanceAppear = 0.1f;

        listaPeixes[4].name = "Piraiba";
        listaPeixes[4].force = 100;
        listaPeixes[4].chanceAppear = 0.05f;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.3f);
        Gizmos.DrawCube(boatArea.center, boatArea.size);
    }
}

using UnityEngine;

[RequireComponent(typeof(BoatController))]
public class Boat : MonoBehaviour {

    //Linha
    private LineRenderer lRend;
    private Vector3[] pointsOfLine = new Vector3[5];

    private readonly int point_Begin = 0;
    private readonly int point_End = 1;

    //Tipo barco
    public Sprite b1, b2, b3;

    [HideInInspector]
    public BoatController bcontroller;

    public BoatType boatType;
    public BoatCapacityType boatCapacityType;
    public RodType rodType;
    public ReelType reelType;
    public HookAndBaitType baitType;
    
    //GAMBIARRA 1
    public GameObject player;
    public GameObject LineForce;
    public GameObject LineTarget;

    [HideInInspector]
    public BoatStatus sBoat = new BoatStatus();
    RodStatus sRod = new RodStatus();

    public int playerPoints;

    void Start ()
    {
        bcontroller = GetComponent<BoatController>();
        bcontroller.boatState = 0;
        lRend = GetComponent<LineRenderer>();
    }

    void Update ()
    {
        switch (bcontroller.boatState)
        {
            case BoatController.BoatState.Moving:
                player.transform.GetChild(0).gameObject.SetActive(false);
                bcontroller.BoatMovement(this.gameObject, player, sBoat);
                LineForce.transform.position = new Vector3(0, 0, -100);
                break;
            case BoatController.BoatState.Stop:
                player.transform.GetChild(0).gameObject.SetActive(true);
                if (bcontroller.anzol)
                    Line();
                else
                    ResetLine();
                GradientForce(LineForce.GetComponent<Renderer>().material);            
                break;
            case BoatController.BoatState.Fishing:
                bcontroller.Fishing(sRod);
                Line();
                break;
            case BoatController.BoatState.Hooked:
                bcontroller.FishingBattle(sRod);
                Line();
                break;
        }
        RefreshStatus(); //Teria que atualizar quando há troca de itens
    }

    //Debug, quando tiver UI pra mudar o equip, é só dar refresh na estrutura
    void RefreshStatus()
    {
        sBoat = sBoat.RefreshBoatStatus(boatType, boatCapacityType);
        switch (boatType)
        {
            case BoatType.level1:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = b1;
                break;
            case BoatType.level2:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = b2;
                break;
            case BoatType.level3:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = b3;
                break;
        }
        sRod = sRod.RefreshRodStatus(rodType, reelType);
    }

    void GradientForce(Material m)
    {
        float x = bcontroller.ThrowLine(LineForce, LineTarget, sRod, player, this.gameObject.transform.localEulerAngles.z);
        m.color = new Color(x, 1-x, 0);
    }

    void Line()
    {
        if (bcontroller.anzol)
        {
            pointsOfLine[point_Begin] = this.gameObject.GetComponent<Boat>().player.transform.position;
            pointsOfLine[point_End] = bcontroller.anzol.transform.position;
            lRend.SetPositions(pointsOfLine);
        }
    }

    void ResetLine()
    {
        pointsOfLine[point_Begin] = this.gameObject.GetComponent<Boat>().player.transform.position;
        pointsOfLine[point_End] = this.gameObject.GetComponent<Boat>().player.transform.position;
        lRend.SetPositions(pointsOfLine);
    }

}

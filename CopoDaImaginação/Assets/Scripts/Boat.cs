using UnityEngine;

[RequireComponent(typeof(BoatController))]
public class Boat : MonoBehaviour {

    public float chute;
    //Linha
    private LineRenderer lRend;
    private Vector3[] points = new Vector3[5];

    private readonly int point_Begin = 0;
    private readonly int point_End = 1;

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
                bcontroller.BoatMovement(this.gameObject, player, sBoat);
                LineForce.transform.position = new Vector3(0, 0, -100);
                break;
            case BoatController.BoatState.Stop:
                if (bcontroller.anzol)
                    Line();
                else
                    ResetLine();
                GradientForce(LineForce.GetComponent<Renderer>().material);            
                break;
            case BoatController.BoatState.Fishing:
                bcontroller.Fishing();
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
        sRod = sRod.RefreshRodStatus(rodType, reelType);
    }

    void GradientForce(Material m)
    {
        float x = bcontroller.ThrowLine(LineForce, LineTarget, sRod, player.transform.position);
        m.color = new Color(x, 1-x, 0);
    }

    void Line()
    {
        if (bcontroller.anzol)
        {
            points[point_Begin] = this.gameObject.GetComponent<Boat>().player.transform.position;
            points[point_End] = bcontroller.anzol.transform.position;
            lRend.SetPositions(points);
        }
    }

    void ResetLine()
    {
        points[point_Begin] = this.gameObject.GetComponent<Boat>().player.transform.position;
        points[point_End] = this.gameObject.GetComponent<Boat>().player.transform.position;
        lRend.SetPositions(points);
    }

}

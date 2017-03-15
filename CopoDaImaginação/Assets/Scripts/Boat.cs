using UnityEngine;

[RequireComponent(typeof(BoatController))]
public class Boat : MonoBehaviour {

    [HideInInspector]
    public BoatController bcontroller;

    public BoatType boatType;
    public RodType rodType;
    public ReelType reelType;
    public BaitType baitType;
    public HookType hookType;
    
    public GameObject player;
    public GameObject LineForce;
    public GameObject LineTarget;

    BoatStatus sBoat = new BoatStatus();
    RodStatus sRod = new RodStatus();
    ReelStatus sReel = new ReelStatus();

    public float maxThrowForce, minThrowForce;

	void Start ()
    {
        bcontroller = GetComponent<BoatController>();
    }

    void Update ()
    {
        RefreshStatus(); //Teria que atualizar quando há troca de itens

        if (!bcontroller.isStopped)
        {
            bcontroller.BoatMovement(this.gameObject, player, sBoat);
            LineForce.transform.position = new Vector3(0, 0, -100);
        }
        else
        {
            GradientForce(LineForce.GetComponent<Renderer>().material);
        }

    }

    //Debug, quando tiver UI pra mudar o equip, é só dar refresh na estrutura
    void RefreshStatus()
    {
        sBoat = sBoat.RefreshBoatStatus(boatType);
        sRod = sRod.RefreshRodStatus(rodType);
        sReel = sReel.RefreshReelStatus(reelType);
    }

    float GradientForce(Material m)
    {
        float x = bcontroller.ThrowLine(LineForce, LineTarget, sRod, player.transform.position);
        m.color = new Color(x, 1-x, 0);
        return x;
    }


}

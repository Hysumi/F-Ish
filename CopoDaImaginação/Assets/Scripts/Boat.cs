using UnityEngine;

[RequireComponent(typeof(BoatController))]
public class Boat : MonoBehaviour {

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

    public int points;

    void Start ()
    {
        bcontroller = GetComponent<BoatController>();
        bcontroller.boatState = 0;
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
                GradientForce(LineForce.GetComponent<Renderer>().material);
                break;
            case BoatController.BoatState.Fishing:
                bcontroller.Fishing();
                break;
            case BoatController.BoatState.Hooked:
                bcontroller.FishingBattle(sRod);
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


}

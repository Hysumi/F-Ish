using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    //Inventario
    public enum BoatState
    {
        Moving = 0,
        Stop = 1,
        Fishing = 2,
        Hooked = 3
    }

    public delegate void Pescou(Item novoItem);
    public static event Pescou pescouPeixe;

    public BoatState boatState = BoatState.Moving;
    public float arrowSpacing;
    public float FishingBonusChance;
    [HideInInspector]
    public Animator anim;

    //Essas variáveis vão sumir
    public bool isDay;
    public int ambient;
    public string fishName;
    //

    [HideInInspector]
    public FishController fishController; //TINHA QUE TRANSFORMAR ISSO NUM EVENTO

    public float reactionTime;
    float _reactionTimmer = 0;

    public float distanceToFlee;
    public float fleeSpeed;
    public float forceDecrement;
    public float holdRange;
    public List<FishStatus> capturedFishList = new List<FishStatus>(); //ARMAZENAR DEPOIS

    Vector2 dragOrigin;

    //Bools de controle 
    bool isDragging;
    bool isHoldLine = false;
    bool canInstantiateHook = false;
    bool isHooked = false;
    [HideInInspector]
    public bool isStopped;

    //Variável usada para câmera seguir
    [HideInInspector]
    public float directionX, directionY;
    [HideInInspector]
    public Vector2 boatDirection;

    //Variáveis da linha
    float throwForce;

    //Anzol 
    [HideInInspector]
    public GameObject anzol;
    RaycastHit2D anzolRaycastHit;
    RaycastHit2D[] anzolFishingTrigger; //PODIA SER UM EVENTO DO PEIXE
    [HideInInspector]
    public Vector2 lineDirection;
    Vector3 hookEndPosition;
    public float throwSpeed;
    float hookSize;
    Bounds anzolBounds;

    //Gambiarra: CLICAR NO BARCO
    Bounds boatArea;

    //Bounds da Camera para determinar a área de visão
    Bounds mainCameraBounds;

    //Variávies do peixe pego
    GameObject fishingSpot;
    bool selected = false;
    Vector3 fishOrigin;
    int selectedFish;
    float originalFishResistence, actualFishResistence; 
    float originalReelResistence, actualReelResistence;
    int actualBoatCapacity = 0;
       
    public GameObject barras;
    GameObject _fishBar, _rodBar;
    Vector3 _arrowSpacing;

    public void BoatMovement(GameObject boat, GameObject player, BoatStatus b)
    {
        if (Mathf.Abs(player.transform.localEulerAngles.z) > 10)
        {
            SoftRotation(0, 300, player);
        }
        else
        {
            player.transform.localEulerAngles = Vector3.zero;
            _fishBar = barras.transform.GetChild(0).gameObject;
            _rodBar = barras.transform.GetChild(1).gameObject;

            boatArea = boat.GetComponent<BoxCollider2D>().bounds;
            Vector2 dragVector = AnalogStick();

            if (isDragging && !isStopped)
            {
                directionX = Mathf.Sign(boatDirection.x);
                directionY = Mathf.Sign(boatDirection.y);

                float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;
                angle += 90;

                SoftRotation(angle, b.rotationSpeed, boat);

                boatDirection = player.transform.position - boat.transform.position;
                Vector3 newPos = boatDirection * Time.deltaTime * DragForce(dragVector, b.dragToMaxSpeed, b.maxSpeed) * b.maxSpeed;
                boat.transform.position += newPos;
                dragOrigin += boatDirection * Time.deltaTime * DragForce(dragVector, b.dragToMaxSpeed, b.maxSpeed) * b.maxSpeed;
            }
        }
    }
    
    public float ThrowLine(GameObject line, GameObject target, RodStatus r, GameObject player, float boatAngle)
    {
        Vector3 dragVector = AnalogStick();        
        barras.transform.position = new Vector3(barras.transform.position.x, barras.transform.position.y, -20);
        if (isDragging && !canInstantiateHook)
        {
            isHoldLine = true;
            float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;
            angle -= 90;

            throwForce = DragForce(dragVector, r.maxDistance, r.maxDistance);

            _arrowSpacing = dragVector.normalized * arrowSpacing;
            player.transform.localEulerAngles = new Vector3(0,0, angle - boatAngle);
            line.transform.localEulerAngles = new Vector3(0, 0, angle);
            line.transform.position = player.transform.position + _arrowSpacing;

            return (throwForce);
        }
        else if (!isHoldLine && !canInstantiateHook)
        {
            mainCameraBounds.center = Camera.main.transform.position + new Vector3(0, 0, 10);
            mainCameraBounds.size = new Vector2(Camera.main.aspect * 2f * Camera.main.orthographicSize, 2f * Camera.main.orthographicSize);

            lineDirection = dragVector.normalized;

            anzol = Instantiate(target, dragVector.normalized * r.maxDistance * throwForce + line.transform.position, new Quaternion());

            anzolRaycastHit = Physics2D.CircleCast(anzol.transform.position, 0.1f, Vector2.zero);
            anzolBounds = new Bounds(anzol.transform.position, new Vector3(0.5f, 0.5f));

            if (mainCameraBounds.Intersects(anzolBounds))
            {
                if (!anzolRaycastHit)
                {
                    //Não jogou no barco
                    player.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Throw");
                    
                    canInstantiateHook = true;
                    hookEndPosition = anzol.transform.position;
                    anzol.transform.position = player.transform.position;
                    anzol.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                    hookSize = anzol.transform.localEulerAngles.x;
                    //boatState = BoatState.Fishing;
                }
                else
                {
                    //Jogou o anzol no barco
                    DestroyImmediate(anzol);
                    anzol = null;
                }
            }
            else 
            { 
                //Jogou além da câmera
                DestroyImmediate(anzol);
                anzol = null;
            }
            line.transform.position = player.transform.position + new Vector3(0, 0, -999);
        }
        if (canInstantiateHook && anzol)
        {
            if (Vector3.Distance(anzol.transform.position, hookEndPosition) < 0.2f)
            {
                anim = anzol.GetComponent<Animator>();
                anzol.transform.position = hookEndPosition;
                anzol.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                anim.SetTrigger("Cutucou");
                boatState = BoatState.Fishing;
                canInstantiateHook = false;
            }
            else
            {
                if (Vector3.Distance(anzol.transform.position, hookEndPosition) > Vector3.Distance(player.transform.position, hookEndPosition) / 2)
                {
                    hookSize += Time.deltaTime;
                    if (hookSize >= 0.275f)
                        hookSize = 0.275f;
                    anzol.transform.localScale = new Vector3(hookSize, hookSize, 1);
                }
                else
                {
                    hookSize -= Time.deltaTime;
                    if (hookSize <= 0.2)
                        hookSize = 0.2f;
                    anzol.transform.localScale = new Vector3(hookSize, hookSize, 1);
                }
                anzol.transform.position += _arrowSpacing/arrowSpacing * throwSpeed * Time.deltaTime;
            }
        }

        return (0);
    }

    public void Fishing(RodStatus rs)
    {
        anzolFishingTrigger = Physics2D.CircleCastAll(anzol.transform.position, 1.5f, Vector2.zero); //RAIO ESTÁTICO??

        if (!isHooked)
        {
            for (int i = 0; i < anzolFishingTrigger.Length; i++)
            {
                if (anzolFishingTrigger[i].transform.gameObject.tag == "Fish")
                {
                    FishingSpot fs = anzolFishingTrigger[i].transform.gameObject.GetComponent<FishingSpot>();
                    fs.FishingTrigger(anzol, anzolBounds);

                    if (fs.isHooked)
                    {
                        fishingSpot = anzolFishingTrigger[i].transform.gameObject;
                        //boatState = BoatState.Hooked;
                        isHooked = true;
                        anim.SetTrigger("Fisgou");
                        break;
                    }
                }
            }
            PullLine(rs.reelPullForce);
        }
        else //(boatState == BoatState.Hooked)
        {
            AnalogStick();
            if(!Input.GetMouseButtonDown(0))
                _reactionTimmer += Time.deltaTime;
            else if(_reactionTimmer <= reactionTime)
            {
                fishingSpot.GetComponent<Animator>().speed = 2;
                boatState = BoatState.Hooked;
                _reactionTimmer = 0;
                isHooked = false;
                barras.transform.position = new Vector3(barras.transform.position.x, barras.transform.position.y, 0);
            }
            if (_reactionTimmer > reactionTime)
            {
                _reactionTimmer = 0;
                isHooked = false;
                ResetFishBattle();
            }

            if(boatState == BoatState.Hooked)
                for (int i = 0; i < anzolFishingTrigger.Length; i++)
                {
                    GameObject fs = anzolFishingTrigger[i].transform.gameObject;
                    if(fs.tag != "Player")
                        if (fs.GetComponent<FishingSpot>().isTriggered && fs.name != fishingSpot.name)
                            fs.GetComponent<FishingSpot>().ResetFish();
                }
        }
    }

    public void FishingBattle(RodStatus rs)
    {
        Vector3 stick = AnalogStick();
        //Debug.Log("HoldRange: " + stick.y + " FishResistence: " + actualFishResistence + " ReelResistence: " + actualReelResistence);
        
        if (!selected)
        {
            SelectFishInFishList(fishingSpot.GetComponent<FishingSpot>());
            fishOrigin = fishingSpot.transform.position;
            originalFishResistence = fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].force;
            actualFishResistence = fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].force;
            actualReelResistence = rs.reelResistence;
            originalReelResistence = rs.reelResistence;
        }

        Vector2 fishDirection = (this.gameObject.transform.position + fishingSpot.transform.position).normalized;
        //fishDirection *= -1;

        anzol.transform.position = fishingSpot.GetComponentInChildren<Transform>().position + new Vector3(fishDirection.x, fishDirection.y) / 4;
        lineDirection = (anzol.transform.position - this.gameObject.transform.position).normalized;
        float fishAngle = Mathf.Atan2(fishDirection.y, fishDirection.x) * Mathf.Rad2Deg;
        fishAngle -= 90;
        SoftRotation(fishAngle, 200, fishingSpot);

        if (!isDragging || actualReelResistence <= 0)
        {
            actualFishResistence += Time.deltaTime * forceDecrement;
            if (actualFishResistence >= originalFishResistence)
            {
                fishingSpot.transform.position += Time.deltaTime * new Vector3(fishDirection.x, fishDirection.y) * fleeSpeed;
                actualFishResistence = originalFishResistence;
            }
            else
            {
                fishingSpot.transform.position += Time.deltaTime * new Vector3(fishDirection.x, fishDirection.y) * fleeSpeed / 2;
            }
            if (Vector3.Distance(fishingSpot.transform.position, fishOrigin) > distanceToFlee || actualReelResistence <= 0)
                ResetFishBattle();
        }
        else
        {
            if (rs.force > actualFishResistence && stick.y > holdRange)
            {
                PullLine(rs.reelPullForce);
                actualFishResistence += Time.deltaTime * forceDecrement;
            }
            else if (rs.force <= actualFishResistence && stick.y > holdRange)
            {
                //actualFishResistence -= Time.deltaTime * forceDecrement * 1.5f; //tá tretando com o peixe
                actualReelResistence -= Time.deltaTime * forceDecrement;
                if (actualFishResistence <= 0)
                    actualFishResistence = 0;
            }
            else
            {
                actualFishResistence -= Time.deltaTime * forceDecrement;
                if (actualFishResistence <= 0)
                    actualFishResistence = 0;
            }
            _fishBar.transform.GetChild(0).transform.localScale = new Vector3(_fishBar.transform.GetChild(0).transform.localScale.x, actualFishResistence / originalFishResistence);
            _rodBar.transform.GetChild(0).transform.localScale = new Vector3(_rodBar.transform.GetChild(0).transform.localScale.x, actualReelResistence / originalReelResistence);

        }
    }

    Vector2 AnalogStick()
    {
        if (isDragging)
        {
            Vector2 dragVector;
            Vector2 mousePosClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dragVector = dragOrigin - mousePosClick;

            Debug.DrawLine(dragOrigin, mousePosClick, Color.green);
            
            if (Input.GetMouseButtonUp(0))
            {
                isHoldLine = false;
                isDragging = false;
            }
            return (dragVector);
        }
        else
        {
            directionX = 0;
            directionY = 0;
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (boatArea.IntersectRay(ray))
                {
                    if (isStopped && !anzol)
                    {
                        boatState = BoatState.Moving;
                        isStopped = false;
                        isDragging = false;
                    }
                    else if (!anzol)
                    {
                        boatState = BoatState.Stop;
                        isStopped = true;
                        isDragging = false;
                    }
                    if (boatState == BoatState.Fishing)
                        ResetFishBattle();
                }
                dragOrigin = ray.origin;
            }
        }
        return (Vector2.zero);
    }

    float DragForce(Vector2 dragForce, float dragToMax, float limit)
    {
        //MaxMagnitude: Aprox: 12 (diagonal)
        //                     2.5 (horizontal)
        //                     5 (vertical)
        if (dragForce.magnitude >= dragToMax)
            return 1;
        else
        {
            if (dragForce.magnitude <= 1)
                return (Mathf.Log10(1));
            return (Mathf.Log(dragForce.magnitude, dragToMax));
        }
    }

    void PullLine(float reelPullForce)
    {

        float yDragForce = AnalogStick().y;
        if (yDragForce < 0)
            yDragForce = 0;
        if (anzol)
        {
            anzol.transform.position -= new Vector3(lineDirection.x, lineDirection.y, 0) * reelPullForce * yDragForce / 10 * Time.deltaTime;
            if (fishingSpot)
                fishingSpot.transform.position -= new Vector3(lineDirection.x, lineDirection.y, 0) * reelPullForce * yDragForce * Time.deltaTime / 10; //DIVIDE NO CHUTE
            anzolBounds.center = anzol.transform.position;

            if (boatArea.Intersects(anzolBounds)) //Pegou um peixe
            {
                if (boatState == BoatState.Hooked)
                {
                    actualBoatCapacity += fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].inventoryWeight;

                    if (actualBoatCapacity > this.gameObject.GetComponent<Boat>().sBoat.maxCapacity)
                    {
                        Debug.Log("Lotou");
                        //APRESENTAR A LISTA DE PEIXES, ESCOLHER E DELETAR O PEIXE
                        //se devolver o lixo, tem que aumentar de novo a chance do lixo aparecer
                    }
                    else
                    {
                        capturedFishList.Add(fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish]);
                        try {
                            pescouPeixe(fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].fishItem);
                        }
                        catch
                        {
                            print("pegou lixo");
                        }
                        if (fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].name == "Lixo")
                        {
                            Debug.Log((fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].trashItem).GetType());
                            fishController.CleanTrash(fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish]);
                            pescouPeixe(fishingSpot.GetComponent<FishingSpot>().listaPeixes[selectedFish].trashItem);
                        }
                        foreach (FishStatus f in capturedFishList)
                        {
                            Debug.Log(f.name);
                        }
                    }
                    ResetFishBattle();
                }
                else
                {
                    DestroyImmediate(anzol);
                    anzol = null;
                    boatState = BoatState.Stop;
                    isDragging = false;
                }
            }
        }
    }

    void ResetFishBattle() 
    {
        DestroyImmediate(fishingSpot);
        fishingSpot = null;
        DestroyImmediate(anzol);
        anzol = null;
        boatState = BoatState.Stop;
        selected = false;
        isDragging = false;
    }
    
    void SelectFishInFishList(FishingSpot fs)
    {
        float sumChance = CheckFishTypeCatchChance(fs);
        float rdm = Random.Range(0f, sumChance);
        float actualChance = 0;
      
        for (int i = 0; i < fs.listaPeixes.Length; i++)
        {
            actualChance += fs.listaPeixes[i].chanceAppear;
            if (rdm < actualChance)
            {
                fishName = fs.listaPeixes[i].name;
                selectedFish = i;
                //Debug.Log(fs.listaPeixes[i].name + " " + rdm + " " + actualChance + " " + sumChance);
                break;
            }
        }

        selected = true;
    }

    float CheckFishTypeCatchChance(FishingSpot fs)
    {
        float chance = 0;

        for (int i = 0; i < fs.listaPeixes.Length; i++)
        {
            if (fs.listaPeixes[i].name != "Lixo")
            {
                for (int j = 0; j < fs.listaPeixes[i].hookType.Length; j++)
                {
                    if (fs.listaPeixes[i].hookType[j] == (int)this.gameObject.GetComponent<Boat>().baitType)
                    {
                        fs.listaPeixes[i].chanceAppear *= FishingBonusChance;
                        break;
                    }
                    else if (j + 1 == fs.listaPeixes[i].hookType.Length) //Se não for a isca
                    {
                        fs.listaPeixes[i].chanceAppear /= FishingBonusChance;
                    }
                }
                if (fs.listaPeixes[i].ambient == ambient)
                    fs.listaPeixes[i].chanceAppear *= FishingBonusChance;
                if (fs.listaPeixes[i].isDay == isDay)
                    fs.listaPeixes[i].chanceAppear *= FishingBonusChance;
                else fs.listaPeixes[i].chanceAppear /= FishingBonusChance;
            }
            Debug.Log(fs.listaPeixes[i].chanceAppear);
            chance += fs.listaPeixes[i].chanceAppear;
        }
        return (chance);
    }

    #region Rotações

    public void SoftRotation(float ang, float rotationSpeed, GameObject g)
    {
        if (ang < 0)
            ang += 360;

        float actualAngle = g.transform.localEulerAngles.z;
        float difAngle = ang - actualAngle;

        if (difAngle >= 0)
        {
            if (Mathf.Abs(difAngle) < 180)
                RotationAnticlockwise(actualAngle, rotationSpeed, g);
            else
                RotationClockwise(actualAngle, rotationSpeed, g);
        }
        else
        {
            if (Mathf.Abs(difAngle) > 180)
                RotationAnticlockwise(actualAngle, rotationSpeed, g);
            else
                RotationClockwise(actualAngle, rotationSpeed, g);
        }
    }

    void RotationClockwise(float actual, float rotationSpeed, GameObject g)
    {
        g.transform.localEulerAngles =
            new Vector3(0, 0, actual - rotationSpeed * Time.deltaTime);
    }

    void RotationAnticlockwise(float actual, float rotationSpeed, GameObject g)
    {
        g.transform.localEulerAngles =
            new Vector3(0, 0, actual + rotationSpeed * Time.deltaTime);
    }

    #endregion
}

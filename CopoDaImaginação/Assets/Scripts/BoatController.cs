using UnityEngine;

public class BoatController : MonoBehaviour
{
    public enum BoatState
    {
        Moving = 0,
        Stop = 1,
        Fishing = 2
    }

    public BoatState boatState = BoatState.Moving;

    Vector2 dragOrigin;

    //Bools de controle 
    bool isDragging;
    bool isHoldLine = false;
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
    GameObject anzol;
    RaycastHit2D anzolRaycastHit;
    RaycastHit2D[] AnzolFishingInfluence;
    Vector2 lineDirection;
    Bounds anzolBounds;

    //Gambiarra: CLICAR NO BARCO
    Bounds boatArea;

    //Bounds da Camera
    Bounds mainCameraBounds;

    public void BoatMovement(GameObject boat, GameObject player, BoatStatus b)
    {
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
            dragOrigin += boatDirection * Time.deltaTime * DragForce(dragVector, b.dragToMaxSpeed, b.maxSpeed)*b.maxSpeed;
        }
    }
    
    public float ThrowLine(GameObject line, GameObject target, RodStatus r, Vector3 playerPos)
    {
        Vector3 dragVector = AnalogStick();

        if (isDragging)
        {
            isHoldLine = true;
            float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;
            angle += 180;
            throwForce = DragForce(dragVector, r.maxDistance, r.maxDistance);
            line.transform.localEulerAngles = new Vector3(0, 0, angle);
            line.transform.position = playerPos + new Vector3(0, 0, 0);

            return (throwForce);
        }
        else if (!isHoldLine && !anzol)
        {
            mainCameraBounds.center = Camera.main.transform.position + new Vector3(0, 0, 10);
            mainCameraBounds.size = new Vector2(Camera.main.aspect * 2f * Camera.main.orthographicSize, 2f * Camera.main.orthographicSize);

            lineDirection = dragVector.normalized;

            anzol = Instantiate(target, dragVector.normalized * r.maxDistance * throwForce + line.transform.position, new Quaternion());

            anzolRaycastHit = Physics2D.CircleCast(anzol.transform.position, 0.1f, Vector2.zero);
            anzolBounds = new Bounds(anzol.transform.position, new Vector3(0.2f, 0.2f));

            if (mainCameraBounds.Intersects(anzolBounds))
            {
                if (!anzolRaycastHit)
                { 
                    //Não jogou no barco
                    boatState = BoatState.Fishing;
                }
                else
                {
                    //Jogou o anzol no barco
                    DestroyImmediate(anzol);
                }
            }
            else 
            { 
                //Jogou além da câmera
                DestroyImmediate(anzol);
            }
        line.transform.position = playerPos + new Vector3(0, 0, -999);

        }
        return (0);
    }

    public void Fishing()
    {
        AnzolFishingInfluence = Physics2D.CircleCastAll(anzol.transform.position, 1.5f, Vector2.zero); //RAIO ESTÁTICO??

        for (int i = 0; i < AnzolFishingInfluence.Length; i++)
        {
            if (AnzolFishingInfluence[i].transform.gameObject.tag == "Fish")
                AnzolFishingInfluence[i].transform.gameObject.GetComponent<FishingSpot>().FishingTrigger(anzol.transform.position);
        }
        PullLine();

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
                    if (isStopped)
                    {
                        boatState = BoatState.Moving;
                        isStopped = false;
                    }
                    else
                    {
                        boatState = BoatState.Stop;
                        isStopped = true;
                    }
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

    void PullLine()
    {
        float yDragForce = AnalogStick().y;
        if (yDragForce < 0)
            yDragForce = 0;
        anzol.transform.position -= new Vector3(lineDirection.x, lineDirection.y, 0)*yDragForce *Time.deltaTime;
        anzolBounds.center = anzol.transform.position;

        if(boatArea.Intersects(anzolBounds))
        {
            DestroyImmediate(anzol);
            boatState = BoatState.Stop;
        }
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

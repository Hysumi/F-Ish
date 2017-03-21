using UnityEngine;

public class BoatController : MonoBehaviour
{
    RaycastHit hit;
    Vector3 dragOrigin;

    bool isDragging;
    bool isLineThrow;
    bool isFishing;
    [HideInInspector]
    public bool isStopped;

    //Variável usada para câmera seguir
    [HideInInspector]
    public float directionX, directionY;
    [HideInInspector]
    public Vector3 boatDirection;

    //Variáveis da linha
    float throwForce;

    public void BoatMovement(GameObject boat, GameObject player, BoatStatus b)
    {
        Vector3 dragVector = AnalogStick();

        if (isDragging && !isStopped)
        {
            directionX = Mathf.Sign(boatDirection.x);
            directionY = Mathf.Sign(boatDirection.y);

            float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;
            angle += 90;
            
            SoftRotation(angle, b.rotationSpeed, boat);

            boatDirection = player.transform.position - boat.transform.position;
            boat.transform.position += boatDirection * Time.deltaTime * DragForce(dragVector, b.dragToMaxSpeed, b.maxSpeed) * b.maxSpeed;
            dragOrigin += boatDirection * Time.deltaTime * DragForce(dragVector, b.dragToMaxSpeed, b.maxSpeed)*b.maxSpeed;
        }
    }
    
    public float ThrowLine(GameObject line, GameObject target, RodStatus r, Vector3 playerPos)
    {
        Vector3 dragVector = AnalogStick();

        if (isDragging)
        {
            float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;
            angle += 180;
            throwForce = DragForce(dragVector, r.maxDistance, r.maxDistance);
            line.transform.localEulerAngles = new Vector3(0, 0, angle);
            line.transform.position = playerPos + new Vector3(0,0,3);

            return (throwForce);
        }
        else if (isLineThrow && !isFishing)
        {
            isLineThrow = false;
            throwForce *= r.maxDistance;
            // isFishing = true;
            if (throwForce > 1)
                Instantiate(target, dragVector.normalized * throwForce + line.transform.position, new Quaternion());
            line.transform.position = playerPos + new Vector3(0, 0, -999);
        }


        return (0);
    }

    Vector3 AnalogStick()
    {
        if (isDragging)
        {
            Vector3 dragVector;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            dragVector = dragOrigin - ray.origin;

            Debug.DrawLine(dragOrigin, ray.origin, Color.green);
            
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                isLineThrow = true;
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

                if (Physics.Raycast(ray, out hit))
                    if (hit.collider.tag == "Player")
                    {
                        if (isStopped)
                            isStopped = false;
                        else
                            isStopped = true;
                    }

                dragOrigin = ray.origin;
            }
        }
        return (Vector3.zero);
    }

    float DragForce(Vector3 dragForce, float dragToMax, float limit)
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

    #region Rotações

    void SoftRotation(float ang, float rotationSpeed, GameObject g)
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

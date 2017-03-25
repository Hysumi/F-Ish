using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour {

    //Mudar aqui depois o tamanho do rio
    float riverRange = 4.5f;
    bool outOfRange = false;

    Vector3 anzolPos;

    public FishStatus[] listaPeixes;
    Vector2 fishDirection, fishVector;
    GameObject fishHead;
    BoatController bcontroller;

    float angle, fishSpeed;

    //Se tem um anzol chamando atenção
    bool isTriggered = false;

    #region Temporizador

    float SummonTime = 10;
    float actualTime;

    #endregion

    // Use this for initialization
    void Start ()
    {
        bcontroller = new BoatController();
        fishHead = transform.FindChild("Head").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!isTriggered)
            Oscioso();
        else //OPA, achou uma isca
        {
            Vector3 newDirection = (anzolPos - this.gameObject.transform.position).normalized;
            fishVector = newDirection;
            fishSpeed = 2; //Pode mudar
            FishMovement();
        }

        Temporizador();

        
    }

    void Temporizador()
    {
        if (SummonTime < actualTime)
            actualTime = 0;
        else
            actualTime += Time.deltaTime;
    }

    void FishMovement()
    {
        float angle = Mathf.Atan2(fishVector.y, fishVector.x) * Mathf.Rad2Deg;
        angle += 90;

        bcontroller.SoftRotation(angle, 50, this.gameObject);

        fishDirection = this.gameObject.transform.position - fishHead.transform.position;
        Vector3 newPos = fishDirection * Time.deltaTime * Random.Range(0f, 1f) * fishSpeed;

        this.gameObject.transform.position -= newPos;
    }

    void Oscioso()
    {
        if (actualTime == 0 && !outOfRange)
        {
            fishVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            fishSpeed = Random.Range(0f, 6f);
        }

        if (Mathf.Abs(this.gameObject.transform.position.x) >= riverRange && !outOfRange)
        {
            fishVector *= -1;
            outOfRange = true;
        }
        else if(Mathf.Abs(this.gameObject.transform.position.x) < riverRange)
            outOfRange = false;
        FishMovement();
    }

    public void FishingTrigger(Vector3 anzolPosition)
    {
        isTriggered = true;
        anzolPos = anzolPosition;
    }
}

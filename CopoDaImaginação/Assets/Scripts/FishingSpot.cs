using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpot : MonoBehaviour {

    //Mudar aqui depois o tamanho do rio
    float riverRange = 4.5f;
    bool outOfRange = false;

    GameObject anzol;
    Bounds anzolBounds;

    public FishStatus[] listaPeixes;
    public Vector2 fishDirection, fishVector;
    GameObject fishHead;
    Bounds fishBounds;
    BoatController bcontroller;

    float angle, fishSpeed;
    public int hookHits;

    //Se tem um anzol chamando atenção
    public bool isTriggered = false;
    bool isKnockBack = false;
    public bool isHooked = false;

    #region Temporizador

    float SummonTime = 10;
    float actualTime;

    #endregion

    // Use this for initialization
    void Start ()
    {
        bcontroller = new BoatController();
        fishHead = transform.FindChild("Head").gameObject;
        hookHits = Random.Range(0, 5); //0 a 4 mordidas
        fishBounds.center = fishHead.transform.position;
        fishBounds.size = this.gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        fishBounds.center = fishHead.transform.position;

        if (!isTriggered && !isHooked)
            Oscioso();
        else if (!anzol)
            ResetFish();
        else//OPA, achou uma isca
        {
            if (!isHooked && (anzolBounds.Intersects(fishBounds) || isKnockBack))
            {
                isKnockBack = true;
                if (hookHits == 0)
                    isHooked = true;
                else KnockBack();
            }
            else if (!isHooked)
            {
                Vector3 newDirection = (anzol.transform.position - this.gameObject.transform.position).normalized;
                fishVector = newDirection;

                if (Vector3.Distance(this.gameObject.transform.position, anzol.transform.position) < 0.5f) //Se estiver pronto pra dar o bote
                    fishSpeed = 30;
                else fishSpeed = 2; //Pode mudar

                FishMovement();
            }
        }

        Temporizador();
    }
    public void KnockBack()
    {
        fishSpeed = 2;
        Vector3 newPos = -fishDirection * Time.deltaTime * Random.Range(0f, 1f) * fishSpeed;
        this.gameObject.transform.position -= newPos;
        if (Vector3.Distance(this.gameObject.transform.position, anzol.transform.position) > 0.7f)
        {
            isKnockBack = false;
            hookHits--;
        }
    }

    public void FishingTrigger(GameObject a, Bounds anzolB)
    {
        isTriggered = true;
        anzol = a;
        anzolBounds = anzolB;
    }

    public void NotTriggered()
    {
        isTriggered = false;
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
        angle = Mathf.Atan2(fishVector.y, fishVector.x) * Mathf.Rad2Deg;
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

    public void ResetFish()
    {
        isTriggered = false;
        hookHits = Random.Range(0, 5);
        fishDirection *= -1;
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(fishBounds.center, fishBounds.size);
    }
    */
}

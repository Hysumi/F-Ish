using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;


        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }

    public GameObject target;

    public float lookAheadDstX;
    public float lookAheadDstY;

    public float lookSmoothTimeX;
    public float lookSmoothTimeY;

    public Vector2 focusAreaSize;

    FocusArea focusArea;
    BoxCollider2D col;
    Boat b;

    float currentLookAheadX;
    float currentLookAheadY;

    float targetLookAheadX;
    float targetLookAheadY;

    float lookAheadDirX;
    float lookAheadDirY;

    float smoothLookVelocityX;
    float smoothLookVelocityY;

    bool lookAheadStopedX;
    bool lookAheadStopedY;

    void Start()
    {
        b = target.GetComponent<Boat>();
        col = target.GetComponent<BoxCollider2D>();
        focusArea = new FocusArea(col.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update(col.bounds);

        Vector2 focusPosition = focusArea.centre;

        if(focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(b.bcontroller.directionX) == Mathf.Sign(focusArea.velocity.x) && b.bcontroller.directionX != 0)
            {
                lookAheadStopedX = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            {
                if (!lookAheadStopedX)
                {
                    lookAheadStopedX = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4;
                }
            }
        }
        
        if (focusArea.velocity.y != 0)
        {
            lookAheadDirY = Mathf.Sign(focusArea.velocity.y);
            if (Mathf.Sign(b.bcontroller.directionY) == Mathf.Sign(focusArea.velocity.y) && b.bcontroller.directionY != 0)
            {
                lookAheadStopedY = false;
                targetLookAheadY = lookAheadDirY * lookAheadDstY;
            }
            else
            {
                if (!lookAheadStopedY)
                {
                    lookAheadStopedY = true;
                    targetLookAheadY = currentLookAheadY + (lookAheadDirY * lookAheadDstY - currentLookAheadY) / 4;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        currentLookAheadY = Mathf.SmoothDamp(currentLookAheadY, targetLookAheadY, ref smoothLookVelocityY, lookSmoothTimeY);

        focusPosition += Vector2.right * currentLookAheadX;
        focusPosition += Vector2.up * currentLookAheadY;
        transform.position = (Vector3) focusPosition + Vector3.forward * -10;
    }

   void OnDrawGizmos()
   {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
   }
}

using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 Startpoint;
    [SerializeField] private float speed;
    public enum Direct { None, Left, Right, Up, Down};
    public Direct direct = Direct.None;
    public float minSwipeDistance = 30f;
    private Vector2 startPos;
    void Start()
    {
        
    }
    void Update()
    {
        Control();
        //transform.Translate(speed * - Time.deltaTime, 0f, 0f);

    }


    public void Control()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            Vector2 swipeDirection = endPos - startPos;
            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f, brickLayer))
            {*/
                if (swipeDirection.magnitude >= minSwipeDistance)
            {
                swipeDirection.Normalize();

                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    if (swipeDirection.x > 0)
                    {
                        direct = Direct.Right;
                        Debug.Log("R");
                        transform.Translate(speed * Time.deltaTime, 0f, 0f);
                    }
                    else
                    {
                        Debug.Log("L");
                        direct = Direct.Left;
                        transform.Translate(speed * -Time.deltaTime, 0f, 0f);
                    }
                }
                else
                {
                    if (swipeDirection.y > 0)
                    {
                        direct = Direct.Up;
                    }
                    else
                    {
                        direct = Direct.Down;
                    }
                }
            }
            else
            {
                direct = Direct.None;
            }
        }
    }
}

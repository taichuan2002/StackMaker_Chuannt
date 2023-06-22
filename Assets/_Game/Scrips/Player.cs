using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] LayerMask wallLayer;

    private bool isMoving = false;
    private Vector3 currentPos;
    private Vector3 targetPos;

    void Start()
    {
        
    }
    void Update()
    {
        Control();
    }


    public void Control()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentPos = Input.mousePosition;
                //Debug.Log("Current: " + currentPos.x + ":" + currentPos.y + ":" + currentPos.z);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                targetPos = Input.mousePosition;
                //Debug.Log("Target: " + targetPos.x + ":" + targetPos.y + ":" + targetPos.z);
                Vector3 moveDir = targetPos - currentPos;
                moveDir = GetDirection(moveDir);
                //Debug.Log("Direction: " + moveDir.x + ":" + moveDir.y + ":" + moveDir.z);
                if (moveDir != Vector3.zero)
                {
                    StopAllCoroutines();
                    StartCoroutine(Move(moveDir));
                    StartCoroutine(CheckWall(moveDir));
                }
            }
        }
    }
    private Vector3 GetDirection(Vector3 direction)
    {
        float horizontal = Mathf.RoundToInt(direction.x);
        float vertical = Mathf.RoundToInt(direction.y);

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            return new Vector3(horizontal * 3f, 0f, 0f).normalized;
        }
        else if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            return new Vector3(0f, 0f, vertical * 3f).normalized;
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }
    }

    private IEnumerator Move(Vector3 direction)
    {
        while (true)
        {
            isMoving = true;
            Vector3 movement = new Vector3(direction.x, 0f, direction.z);
            transform.position += movement * speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CheckWall(Vector3 direction)
    {
        while (true)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.position;
            if (Physics.Raycast(raycastPos, direction, out hit, 0.5f, wallLayer))
            {
                StopAllCoroutines();
                isMoving = false;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "BrickBlock")
        {
            Debug.Log("a");
            Destroy(collision.gameObject);
        }
    }
}

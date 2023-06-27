using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask BrickBlock;
    [SerializeField] private Vector3 StartPoint;
    [SerializeField] public GameObject brickPrefab;
    [SerializeField] public GameObject wallPrefab;
    [SerializeField] public GameObject cubePrefab;
    [SerializeField] private int countBrick;
    [SerializeField] private List<GameObject> cloneBrick = new List<GameObject>();

    private float brickHeight;
    private bool isMoving = false;
    private Vector3 currentPos;
    private Vector3 targetPos;

    private void Start()
    {
        countBrick = 0;
        brickHeight = 0.3f;
        transform.position = new Vector3(StartPoint.x, StartPoint.y, StartPoint.z);
    }

    private void Update()
    {
        Control();
    }
   
    private void Control()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                targetPos = Input.mousePosition;
                Vector3 moveDir = targetPos - currentPos;
                moveDir = GetDirection(moveDir);
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
            return Vector3.zero;
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
        
        if (collision.tag == "brick")
        {
            countBrick += 1;
            Destroy(collision.gameObject);
            GameObject newBrick = Instantiate(brickPrefab, gameObject.transform);
            newBrick.transform.position += new Vector3(0, countBrick * brickHeight, 0);
            transform.position += new Vector3(0, brickHeight, 0);
            cloneBrick.Add(newBrick);
        }
        if (collision.tag == "StopPoint")
        {
            StopAllCoroutines();
            isMoving = false;
        }
        if(collision.tag == "unBrick")
        {
            countBrick -= 1;
            if (cloneBrick.Count > 0)
            {
                GameObject lastBrick = cloneBrick[cloneBrick.Count - 1];
                lastBrick.transform.parent = null;
                lastBrick.transform.position -= new Vector3(0.1f, 0, 0);
                cloneBrick.RemoveAt(cloneBrick.Count - 1);
            }

        }

    }

}

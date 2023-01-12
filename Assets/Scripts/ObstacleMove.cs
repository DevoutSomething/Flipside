using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float speed;
    public bool movingToPos1;
    private Vector2 pos1;
    private Vector2 pos2;
    public bool moveX;
    public bool moveY;
    
    private void Start()
    {
        movingToPos1 = true;
        pos1 = object1.transform.position;
        pos2 = object2.transform.position;

    }
    void Update()
    {
        float newSpeed;
        if (moveX || moveY)
        {
            newSpeed = speed;
        }
        if (moveX && moveY)
        {
            newSpeed = speed / 2;
        }
        if (moveY)
        {
            if (movingToPos1)
            {
                Vector3 targetPosition = pos1;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            if (!movingToPos1)
            {
                Vector3 targetPosition = pos2;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            if (transform.position.y >= pos1.y - 0.1)
            {
                movingToPos1 = false;
            }
            if (transform.position.y <= pos2.y + 0.1)
            {
                movingToPos1 = true;
            }
        }
        if (moveX)
        {
            if (movingToPos1)
            {
                Vector3 targetPosition = pos1;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            if (!movingToPos1)
            {
                Vector3 targetPosition = pos2;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            if (transform.position.x >= pos1.x - 0.1)
            {
                movingToPos1 = false;
            }
            if (transform.position.x <= pos2.x + 0.1)
            {
                movingToPos1 = true;
            }
        }

        
    }
}

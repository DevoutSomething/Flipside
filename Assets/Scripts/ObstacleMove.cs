using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public GameObject pos1;
    public GameObject pos2;
    public float speed;
    public bool movingToPos1;
    
    private void Start()
    {
        movingToPos1 = true;
    }
    void Update()
    {
        if (movingToPos1)
        {
            Vector3 targetPosition = pos1.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        if (!movingToPos1)
        {
            Vector3 targetPosition = pos2.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        if (transform.position.y >= pos1.transform.position.y - 0.1)
        {
            movingToPos1 = false;
        }
        if (transform.position.y <= pos2.transform.position.y + 0.1)
        {
            movingToPos1 = true;
        }
    }
}

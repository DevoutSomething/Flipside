using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnvirnmentChecks : MonoBehaviour
{
    public LayerMask groundLayer;
    private BoxCollider2D boxCollider2d;
    [Header("wall = 1 / floor = 2 / player = 3")] 
    public int Checkingfor;
    public GameObject parent;
    private EnemyController enemyController;

    private void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        enemyController = parent.GetComponent<EnemyController>();
    }

    private void Check()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2d.bounds.center, Vector2.left, boxCollider2d.bounds.extents.y + 0.05f, groundLayer);
        if (raycastHit.collider != null)
        {
            if (Checkingfor == 1)
            {
                Debug.Log("wall detected");
                enemyController.TurnAround();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            if (Checkingfor == 1)
            {
                Debug.Log("wall detected");
                enemyController.TurnAround();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ground"))
        {

            if (Checkingfor == 2)
            {
                Debug.Log("no floor detected");
                enemyController.TurnAround();
            }
        }
    }
}

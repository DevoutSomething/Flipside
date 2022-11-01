using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnvirnmentChecks : MonoBehaviour
{
    [Header("wall = 1 / floor = 2 / player = 3")] 
    public int Checkingfor;
    public GameObject parent;
    private EnemyController enemyController;

    private void Start()
    {
        enemyController = parent.GetComponent<EnemyController>();
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

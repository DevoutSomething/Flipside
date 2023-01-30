using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGoUp : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject gameManger;
    public Animator plantAnim;
    public float distance = 6f;
    private bool attacking;
    public float timeNoTarget = .5f;
    void Start()
    {
        attacking = false;
        gameManger = GameObject.Find("GameManger");
    }
    private void Update()
    {
        RaycastHit2D raycastPlant = Physics2D.Raycast(transform.position, Vector2.up, distance, playerLayer);
        if(raycastPlant.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.up * raycastPlant.distance, Color.red);
            if (!attacking)
            {
                StartCoroutine(SendAttackUp());
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector2.up * distance, Color.green);
        }
    }

    public IEnumerator SendAttackUp()
    {
        attacking = true;
        plantAnim.SetTrigger("plantussy");
        yield return new WaitForSecondsRealtime(timeNoTarget);
        attacking = false;
    }
}

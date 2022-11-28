using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusScript : MonoBehaviour
{
    public GameManager gameManager;
    public float startTime;
    public float activeTime;
    private bool SpikedOut;
    public float flipSpikeTimer;
    public float flipSpikeActiveTime;
    private static int groundLayer = 3;
    private static int obLayer = 7;
    private bool canSpike = true;


    private void Start()
    {
        if (gameManager.isFlipped)
        {
            StartCoroutine(SpikeSpawnTime(flipSpikeTimer));
        }
    }
    public void FixedUpdate()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //checks if player is touching cactus then checks if it is in "attack mode" then runs the spike spawn enum
        string whatStanding = collision.collider.name;
        if (gameManager.isFlipped == false)
        {
            if (whatStanding == "player" && SpikedOut == false && canSpike)
            {
                canSpike = false;
                StartCoroutine(SpikeSpawnTime(startTime));
            }
        }
    }

    public IEnumerator SpikeSpawnTime(float startTimeActual)
    {
        //spawns spike
        yield return new WaitForSecondsRealtime(startTimeActual);
        SpikedOut = true;
        gameObject.layer = obLayer;
        canSpike = true;
        Debug.Log("spike spawn");
        if (gameManager.isFlipped)
        {
            StartCoroutine(SpikeDespawnTime(flipSpikeActiveTime));
        }
        else
        {
            StartCoroutine(SpikeDespawnTime(activeTime));
        }
    }
    public IEnumerator SpikeDespawnTime(float activeTimeActual)
    {
        //despawns spike
        yield return new WaitForSecondsRealtime(activeTimeActual);
        gameObject.layer = groundLayer;
        SpikedOut = false;
        Debug.Log("spike despawn");
        if (gameManager.isFlipped)
        {
            StartCoroutine(SpikeSpawnTime(flipSpikeTimer));
        }

    }
}

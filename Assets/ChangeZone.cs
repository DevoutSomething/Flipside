using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeZone : MonoBehaviour
{
    public bool inZone;
    public ChangeZone changeZone;
    public Transform zoneRespawnLocation;
    public Transform camRestrictRightXUpY;
    public Transform camRestrictLeftxDownY;
    private GameObject player;
    private GameObject Camera;
    void Start()
    {
        Camera = GameObject.Find("Player Camera");
        player = GameObject.Find("player");
    }
    private void Update()
    {
        if (inZone)
        {
            Camera.GetComponent<CameraController>().lowerXLimit = camRestrictLeftxDownY.position.x;
            Camera.GetComponent<CameraController>().upperXLimit = camRestrictRightXUpY.position.x;
            Camera.GetComponent<CameraController>().lowerYLimit = camRestrictLeftxDownY.position.y;
            Camera.GetComponent<CameraController>().upperYLimit = camRestrictRightXUpY.position.y;
            player.GetComponent<PlayerHealth>().respawnPoint = new Vector2(zoneRespawnLocation.position.x, zoneRespawnLocation.position.y);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.name == "player" && inZone)
        {
            Debug.Log(" Collide with gate");
            changeZone.inZone = true;
            inZone = false;
            Destroy(gameObject);
        }

    }
}

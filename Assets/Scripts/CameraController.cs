using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    public GameObject Player;
    public Light PlayerLight;
    public Light CameraLight;

    [Header("Follow")]
    public float followSpeed;
    public Vector3 offset;
    public Transform startingRoom;
    [Header("Zoom")]
    public float zoomSpeed;
    public float baseCameraSize;
    public float zoomLimit;
    [Header("Dash")]
    public bool currentDashing;
    public float timeBeforeStart;
    [Header("Lighting")]
    public float darkenSpeed;
    public float lightenSpeed;
    public float darkenLimit;
    public float lightIntensityPerm;
    public float lightIntensityPlayerPerm;
    public float darkenAtStart;
    public float darkenAtStartPlayerLight;
    public float playerLightLimit;
    [Header("limits")]
    public float lowerYLimit;
    public float upperYLimit;
    public float lowerXLimit;
    public float upperXLimit;
    [Header("Private Variables")]
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform currentRoom;
    private float dashTimer;
    private float roomMaxX;
    private float roomMaxY;
    private float roomMinX;
    private float roomMinY;
    private bool changingRooms;
    private Camera camera;

   
    
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
       // SetRoom(startingRoom);
        SetCameraSize(baseCameraSize);
       
    }

    void Update()
    {
        #region dashingUpdate
        if (dashTimer > 0)
        {
            dashTimer -= Time.unscaledDeltaTime;
        }
        if (currentDashing && dashTimer <= 0)
        {

            if (CameraLight.intensity > darkenLimit)
            {
                CameraLight.intensity -= Time.unscaledDeltaTime * darkenSpeed;

            }
            if (PlayerLight.intensity < playerLightLimit)
            {
                PlayerLight.intensity += Time.unscaledDeltaTime * lightenSpeed;
            }
            if (camera.orthographicSize > zoomLimit)
            {
                SetCameraSize(camera.orthographicSize -= zoomSpeed * Time.deltaTime);
            }
        }

        #endregion
    }
    private void FixedUpdate()
    {
        #region camera follow
        if (!changingRooms)
        {
            Vector3 targetPosition = Player.transform.position + offset;
            if (targetPosition.x > upperXLimit)
            {
                targetPosition.x = upperXLimit;
            }
            if (targetPosition.x < lowerXLimit)
            {
                targetPosition.x = lowerXLimit;
            }
            if (targetPosition.y > upperYLimit)
            {
                targetPosition.y = upperYLimit;
            }
            if (targetPosition.y < lowerYLimit)
            {
                targetPosition.y = lowerYLimit;
            }
            Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = smoothPosition;
        }
        #endregion
    }

    #region dashing
    public void IsDashing()
    {
        currentDashing = true;
        dashTimer = timeBeforeStart;
        CameraLight.intensity -= darkenAtStart;
        PlayerLight.intensity += darkenAtStartPlayerLight;
    }
    public void FinishedDash()
    {
        currentDashing = false;
        CameraLight.intensity = lightIntensityPerm;
        PlayerLight.intensity = lightIntensityPlayerPerm;
        SetCameraSize(baseCameraSize);
    }
    #endregion
    public void SetCameraSize(float size)
    {
        camera.orthographicSize = size;
    }
    private Vector3 ClampCamera (Vector3 cameraTarget)
    {
        float camHeight = camera.orthographicSize / 2;
        float camWidth = camera.orthographicSize * camera.aspect / 2;
        float maxX = roomMaxX - camWidth;
        float minX = roomMinX + camWidth;
        float maxY = roomMaxY - camHeight;
        float minY = roomMinY + camHeight;

        float newX = Mathf.Clamp(cameraTarget.x, minX, maxX);
        float newY = Mathf.Clamp(cameraTarget.y, minY, maxY);

        return new Vector3(newX, newY, cameraTarget.z);
    }
    public void SetRoom(Transform room)
    {
        currentRoom = room;
        #region findEdgeOfRoom
        SpriteRenderer sRenderer = currentRoom.GetComponent<SpriteRenderer>();
        roomMaxX = (sRenderer.transform.position.x + sRenderer.bounds.size.x) /2;
        roomMinX = (sRenderer.transform.position.x - sRenderer.bounds.size.x) /2;
        roomMaxY = (sRenderer.transform.position.y + sRenderer.bounds.size.y) /2;
        roomMinY = (sRenderer.transform.position.y - sRenderer.bounds.size.y) /2;
        Debug.Log("X max " + roomMaxX);
        Debug.Log("Y max " + roomMaxY);
        Debug.Log("X min " + roomMinX);
        Debug.Log(" min " + roomMinY);
        #endregion
    }
}

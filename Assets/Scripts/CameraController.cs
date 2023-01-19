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
            float camHeight = camera.orthographicSize;
            float camWidth = camera.orthographicSize * camera.aspect;
            Vector3 targetPosition = Player.transform.position + offset;
            if (targetPosition.x < lowerXLimit + camWidth)
            {
                targetPosition.x = lowerXLimit + camWidth;
            }
            if (targetPosition.x > upperXLimit - camWidth)
            {
                targetPosition.x = upperXLimit - camWidth;
            }
            if (targetPosition.y < lowerYLimit + camHeight)
            {
                targetPosition.y = lowerYLimit + camHeight;
            }
            if (targetPosition.y > upperYLimit - camHeight)
            {
                targetPosition.y = upperYLimit - camHeight;
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

}
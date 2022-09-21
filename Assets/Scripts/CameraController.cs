using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    public GameObject Player;
    public Light PlayerLight;
    public Light CameraLight;

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

    public float playerLightLimit;
    [Header("Private Variables")]
    [SerializeField] private Transform cameraPosition;
    private float timer;
    private Camera camera;
   
    
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        #region dashingUpdate
        if (currentDashing && timer <= 0)
        {
            
            if(CameraLight.intensity > darkenLimit)
            {
                CameraLight.intensity -= Time.unscaledDeltaTime * darkenSpeed;

            }
            if(PlayerLight.intensity < playerLightLimit)
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

    #region dashing
    public void IsDashing()
    {
        currentDashing = true;
        timer = timeBeforeStart;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{ 
    public GameObject Player;
    public Light PlayerLight;

    [Header("Zoom")]
    public float zoomSpeed;
    public float baseCameraSize;

    [Header("Lighting")]
    public float darkenSpeed;
    public float darkenLimit;
    public float lightIntensityPerm;

    [Header("Private Variables")]
    [SerializeField] private Transform cameraPosition;
    
    void Start()
    {
       
    }

    void Update()
    {
        
    }

    public void IsDashing()
    {
        
    }
    public void FinishedDash()
    {

    }
    public void SetCameraSize(float size)
    {
        GetComponent<Camera>().orthographicSize = size;
    }
}

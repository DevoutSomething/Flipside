using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float slowdownFactor;
    public float slowdownLength = .1f;
    public bool resettingTime;
    private float timePerm;

    private void Start()
    {
        timePerm = Time.fixedDeltaTime;
    }
    private void Update()
    {
        if (resettingTime)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = timePerm;
        }
    }
    public void SlowDownTime()
    {
        resettingTime = false;
        Time.timeScale = slowdownFactor; //fix this shit
        Time.fixedDeltaTime = Time.timeScale * .02f;

    }
    public void ResetTime()
    {
        resettingTime = true;
    }
}

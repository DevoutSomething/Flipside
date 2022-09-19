
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMovement : MonoBehaviour
{
    public float Speed = 3f;
    public bool IsMoving { get; private set; }

    public OverworldPin CurrentPin { get; private set; }
    private OverworldPin _targetPin;
    private Overworldmap _mapManager;

    public void Initialise(Overworldmap mapManager, OverworldPin startPin)
    {
        _mapManager = mapManager;
        SetCurrentPin(startPin);
    }

    

    private void Update()
    {
        if (_targetPin == null) return;

       
        var currentPosition = transform.position;
        var targetPosition = _targetPin.transform.position;

        
        if (Vector3.Distance(currentPosition, targetPosition) > .02f)
        {
            transform.position = Vector3.MoveTowards(
                currentPosition,
                targetPosition,
                Time.deltaTime * Speed
            );
        }
        else
        {
            if (_targetPin.IsAutomatic)
            {
               
                var pin = _targetPin.GetNextPin(CurrentPin);
                MoveToPin(pin);
            }
            else
            {
                SetCurrentPin(_targetPin);
            }
        }
    }
    public void TrySetDirection(Direction direction)
    {
       
        var pin = CurrentPin.GetPinInDirection(direction);

        
        if (pin == null) return;
        MoveToPin(pin);
    }


   
    private void MoveToPin(OverworldPin pin)
    {
        _targetPin = pin;
        IsMoving = true;
    }


    
    public void SetCurrentPin(OverworldPin pin)
    {
        CurrentPin = pin;
        _targetPin = null;
        transform.position = pin.transform.position;
        IsMoving = false;
        //_mapManager.UpdateGui();
    }









































}

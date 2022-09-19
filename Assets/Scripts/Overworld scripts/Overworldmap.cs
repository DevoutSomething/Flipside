using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overworldmap : MonoBehaviour
{
	public OverworldMovement Character;
	public OverworldPin StartPin;
	public Text SelectedLevelText;

	private void Start()
	{
		Character.Initialise(this, StartPin);
	}

	private void Update()
	{
		
		if (Character.IsMoving) return;

		CheckForInput();
	}

	private void CheckForInput()
	{
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			Character.TrySetDirection(Direction.Up);
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			Character.TrySetDirection(Direction.Down);
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			Character.TrySetDirection(Direction.Left);
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			Character.TrySetDirection(Direction.Right);
		}
	}





































}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public enum Direction
{
	Up,
	Down,
	Left,
	Right
}
/* 
 #region name [Header("name")] 
	#endregion  
*/
public class OverworldPin : MonoBehaviour
{
	public OverworldPin UpPin;
	public OverworldPin DownPin;
	public OverworldPin LeftPin;
	public OverworldPin RightPin;

	private Dictionary<Direction, OverworldPin> _pinDirections;



	private void Start()
	{
	
		_pinDirections = new Dictionary<Direction, OverworldPin>
		{
			{ Direction.Up, UpPin },
			{ Direction.Down, DownPin },
			{ Direction.Left, LeftPin },
			{ Direction.Right, RightPin }
		};

	
	}

	public OverworldPin GetPinInDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				return UpPin;
			case Direction.Down:
				return DownPin;
			case Direction.Left:
				return LeftPin;
			case Direction.Right:
				return RightPin;
			default:
				throw new ArgumentOutOfRangeException("direction", direction, null);
		}
	}

























}

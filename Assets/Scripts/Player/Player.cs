using UnityEngine;
using System.Collections;

public class Player : Unit
{
	void Start()
	{
		controller = new PlayerController();
	}
}

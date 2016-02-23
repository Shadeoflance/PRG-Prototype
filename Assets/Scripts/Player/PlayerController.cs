using UnityEngine;
using System.Collections;

public class PlayerController : IController
{
	public bool NeedAttack ()
	{
		return Input.GetButton("Attack");
	}

	public bool NeedJump ()
	{
		return Input.GetButton("Jump");
	}

	public bool NeedDash ()
	{
		return Input.GetButton("Dash");
	}

	public bool NeedItem ()
	{
		return Input.GetButton("Item");
	}

	public Vector2 NeedVel ()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	public void Update ()
	{

	}
}

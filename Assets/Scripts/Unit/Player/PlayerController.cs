using UnityEngine;
using System.Collections;

public class PlayerController : IController
{
    Player player;
    public PlayerController(Player player)
    {
        this.player = player;
    }
	public bool NeedAttack ()
	{
		return Input.GetButton("Attack");
	}

	public bool NeedJump ()
	{
		return Input.GetButtonDown("Jump");
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
        if (Input.GetButtonDown("Jump"))
            player.eventManager.InvokeHandlers("jumpButtonDown");
        if (Input.GetButtonUp("Jump"))
            player.eventManager.InvokeHandlers("jumpButtonUp");
        if(Input.GetButtonDown("Attack"))
            player.eventManager.InvokeHandlers("attackButtonDown");
        if (Input.GetButtonUp("Attack"))
            player.eventManager.InvokeHandlers("attackButtonUp");
	}
}

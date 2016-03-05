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
        if (Input.GetButtonDown("Item"))
            player.eventManager.InvokeHandlers("itemButtonDown");
        if (Input.GetButtonDown("Dash"))
            player.eventManager.InvokeHandlers("dashButtonDown");
        if (Input.GetKeyDown(KeyCode.R))
            player.transform.position = new Vector3(0, 0, 0);
	}
}

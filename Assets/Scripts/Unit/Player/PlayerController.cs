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
		return Input.GetButton("Jump");
	}

	public Vector2 NeedVel ()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	public void Update ()
	{
        if (Input.GetButtonDown("Jump"))
            player.eventManager.InvokeHandlers("jumpButtonDown", null);
        if (Input.GetButtonUp("Jump"))
            player.eventManager.InvokeHandlers("jumpButtonUp", null);
        if(Input.GetButtonDown("Attack"))
            player.eventManager.InvokeHandlers("attackButtonDown", null);
        if (Input.GetButtonUp("Attack"))
            player.eventManager.InvokeHandlers("attackButtonUp", null);
        if (Input.GetButtonDown("Item"))
            player.eventManager.InvokeHandlers("itemButtonDown", null);
        if (Input.GetButtonDown("Dash"))
            player.eventManager.InvokeHandlers("dashButtonDown", null);
        if (Input.GetButtonDown("Bomb"))
            player.eventManager.InvokeHandlers("bombButtonDown", null);
	}
}

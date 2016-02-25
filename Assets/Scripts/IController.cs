using UnityEngine;
using System.Collections;

public interface IController : IUpdatable
{
	bool NeedAttack();
	bool NeedJump();
	bool NeedDash();
	bool NeedItem();
	Vector2 NeedVel();
}

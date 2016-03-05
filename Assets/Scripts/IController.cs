using UnityEngine;
using System.Collections;

public interface IController : IUpdatable
{
	bool NeedAttack();
	bool NeedJump();
	Vector2 NeedVel();
}
